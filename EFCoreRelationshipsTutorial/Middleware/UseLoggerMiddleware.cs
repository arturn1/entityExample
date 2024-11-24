using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

public static class UseLoggerExtensions
{
    public static IApplicationBuilder UseLogger(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<UseLoggerMiddleware>();
    }
}

public class UseLoggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<UseLoggerMiddleware> _logger;
    private static readonly ConcurrentBag<WebSocket> WebSockets = new();
    private const string LogFilePath = "logs.txt";

    public UseLoggerMiddleware(RequestDelegate next, ILogger<UseLoggerMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.WebSockets.IsWebSocketRequest && context.Request.Path == "/logs")
        {
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            WebSockets.Add(webSocket);
            _logger.LogInformation("WebSocket connection established.");

            try
            {
                // Envia o histórico de logs ao cliente
                await SendLogHistoryAsync(webSocket);

                // Mantém a conexão ativa
                await KeepConnectionAliveAsync(webSocket);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("WebSocket connection error: {Message}", ex.Message);
            }
            finally
            {
                WebSockets.TryTake(out webSocket); // Remove conexão fechada
                _logger.LogInformation("WebSocket connection closed.");
            }

            return;
        }

        var stopwatch = Stopwatch.StartNew();
        var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
        var clientIp = context.Connection.RemoteIpAddress?.ToString();
        var route = context.Request.Path;
        var method = context.Request.Method;
        var actionId = Guid.NewGuid().ToString();
        var sessionId = GetSessionIdFromToken(context.User);

        using (_logger.BeginScope(new { UserId = userId, ActionId = actionId, SessionId = sessionId }))
        {
            try
            {
                var log = new LogModel("start", actionId, userId, sessionId, method, clientIp, route, null, 0, DateTime.UtcNow);
                await BroadcastAndSaveLogAsync(log);

                await _next(context);

                log = new LogModel("finished", actionId, userId, sessionId, method, clientIp, route, context.Response.StatusCode, stopwatch.ElapsedMilliseconds, DateTime.UtcNow);
                await BroadcastAndSaveLogAsync(log);
            }
            catch (Exception ex)
            {
                var log = new LogModel("error", actionId, userId, sessionId, method, clientIp, route, null, stopwatch.ElapsedMilliseconds, DateTime.UtcNow);
                await BroadcastAndSaveLogAsync(log);
                throw;
            }
            finally
            {
                stopwatch.Stop();
            }
        }
    }

    private async Task BroadcastAndSaveLogAsync(LogModel log)
    {
        var logJson = JsonSerializer.Serialize(log);

        // Salva o log no arquivo
        await SaveLogToFileAsync(logJson);

        // Envia o log para todos os clientes conectados
        var tasks = WebSockets.ToList().Select(async ws =>
        {
            if (ws.State == WebSocketState.Open)
            {
                try
                {
                    var buffer = Encoding.UTF8.GetBytes(logJson);
                    await ws.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Failed to send log message to WebSocket: {Message}", ex.Message);
                }
            }
            else
            {
                WebSockets.TryTake(out ws); // Remove a conexão fechada
                _logger.LogInformation("Removed closed WebSocket connection.");
            }
        });

        await Task.WhenAll(tasks);
    }

    private async Task SaveLogToFileAsync(string log)
    {
        try
        {
            await File.AppendAllTextAsync(LogFilePath, log + Environment.NewLine);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to save log to file: {Message}", ex.Message);
        }
    }

    private async Task SendLogHistoryAsync(WebSocket webSocket)
    {
        try
        {
            if (File.Exists(LogFilePath))
            {
                var logs = await File.ReadAllLinesAsync(LogFilePath);
                foreach (var log in logs.TakeLast(100))
                {
                    if (webSocket.State == WebSocketState.Open)
                    {
                        var buffer = Encoding.UTF8.GetBytes(log);
                        await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to send log history: {Message}", ex.Message);
        }
    }

    private async Task KeepConnectionAliveAsync(WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];
        while (webSocket.State == WebSocketState.Open)
        {
            try
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                }
            }
            catch (WebSocketException ex)
            {
                _logger.LogWarning("WebSocket error: {Message}", ex.Message);
                break;
            }
        }
    }

    private string GetSessionIdFromToken(ClaimsPrincipal user)
    {
        var sessionIdClaim = user?.FindFirst("sid");
        return sessionIdClaim?.Value ?? "UnknownSession";
    }

    private class GeoLocationInfo
    {
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? Country { get; set; }
        public string? Loc { get; set; }
        public double Latitude => Loc != null ? double.Parse(Loc.Split(',')[0]) : 0;
        public double Longitude => Loc != null ? double.Parse(Loc.Split(',')[1]) : 0;
    }
}

public record LogModel(
    string action,
    string actionId,
    string userId,
    string session,
    string method,
    string? ip,
    string? route,
    int? statusCode,
    long duration,
    DateTime time
);
