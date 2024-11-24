global using EFCoreRelationshipsTutorial.Data;
global using Microsoft.EntityFrameworkCore;
using API.Middleware;
using EFCoreRelationshipsTutorial.Helpers;
using Hangfire; // Adicione Hangfire
using Hangfire.MySql;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
var connMssql = builder.Configuration.GetConnectionString("DefaultConnectionMssql");
var connMysql = builder.Configuration.GetConnectionString("DefaultConnectionMysql");

// Configuração do Hangfire com SQL Server
// builder.Services.AddHangfire(config =>
// {
//     config.UseSqlServerStorage(connMssql); // Utilizando a mesma conexão do EF Core
// });

builder.Services.AddHangfire(config =>
{
    config.UseStorage(
        new MySqlStorage(connMysql, new MySqlStorageOptions
        {
            TablesPrefix = "Hangfire_", // Opcional: Prefixo para as tabelas
            QueuePollInterval = TimeSpan.FromSeconds(15), // Intervalo de polling
            JobExpirationCheckInterval = TimeSpan.FromHours(1), // Intervalo para verificar jobs expirados
            CountersAggregateInterval = TimeSpan.FromMinutes(5), // Intervalo de agregação de contadores
            PrepareSchemaIfNecessary = true, // Criar o esquema de tabelas, se necessário
            DashboardJobListLimit = 5000 // Limite de jobs no dashboard
        })
    );
});

builder.Services.AddHangfireServer(); // Adiciona o servidor Hangfire para processar os jobs

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseMySql(connMysql, ServerVersion.AutoDetect(connMysql));
    //options.UseSqlServer(connMssql);
});

// Configurar a conexão Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var redisConnectionString = builder.Configuration.GetConnectionString("RedisConnection");
    return ConnectionMultiplexer.Connect(redisConnectionString);
});

// Registrar o serviço Redis
builder.Services.AddScoped<RedisService>();

builder.Services.AddScoped<IMapper, Mapper>();
builder.Services.AddScoped<JobService>();
builder.Services.AddHttpClient();

builder.Services.AddControllers();

// Configuração Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseWebSockets();
app.UseLogger();
app.UseErrorHandling();

// Executar migrações iniciais de forma segura com tratamento de erros
using (var serviceScope = app.Services.CreateScope())
{
    try
    {
        var context = serviceScope.ServiceProvider.GetRequiredService<DataContext>();
        context.Database.Migrate(); // Executa a migração
    }
    catch (Exception ex)
    {
        // Loga a exceção para diagnosticar problemas de migração
        Console.WriteLine($"Erro ao aplicar migrações: {ex.Message}");
        throw; // Rethrow se necessário
    }
}

// Configura CORS
app.UseCors(builder =>
        builder
        .WithOrigins("*")
        .AllowAnyMethod()
        .AllowAnyHeader());

// Configure o pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Adiciona o painel Hangfire para monitoramento de jobs
app.UseHangfireDashboard("/hangfire"); // Painel para ver os jobs

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Método de exemplo para agendar um job simples
RecurringJob.AddOrUpdate(() => Console.WriteLine("Hangfire job executado!"), Cron.Minutely);

app.Run();
