using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class RedisController : ControllerBase
{
    private readonly RedisService _redisService;

    public RedisController(RedisService redisService)
    {
        _redisService = redisService;
    }

    // Endpoint para definir um valor no Redis
    [HttpPost("set")]
    public async Task<IActionResult> SetValue([FromQuery] string key, [FromQuery] string value)
    {
        await _redisService.SetValueAsync(key, value);
        return Ok($"Valor '{value}' definido para a chave '{key}' no Redis.");
    }

    // Endpoint para buscar um valor no Redis
    [HttpGet("get")]
    public async Task<IActionResult> GetValue([FromQuery] string key)
    {
        var value = await _redisService.GetValueAsync(key);
        if (string.IsNullOrEmpty(value))
        {
            return NotFound($"Chave '{key}' não encontrada no Redis.");
        }
        return Ok(value);
    }

    [HttpPost("add-products")]
    public IActionResult AddProductsToQueue()
    {
        var product = new Product
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Product " + DateTime.Now.ToString("HH:mm:ss"),
            Price = new Random().Next(10, 100)
        };

        _ = _redisService.AddProductToQueue(product);

        return Ok($"Adicionando produto: {product.Id}");
    }

    // Consome um produto da fila 'products' a cada 1,5 segundos
    [HttpPost("consume-products")]
    public async Task<IActionResult> ConsumeProductsFromQueue()
    {
        var product = await _redisService.ConsumeProductFromQueue();
        if (product == null)
            return Ok("Nenhum produto na fila.");

        return Ok($"Consumindo produto. Produto consumido: {product.Id}");
    }

    [HttpPost("consume-all-products")]
    public async Task<IActionResult> ConsumeAllProductsFromQueue()
    {
        var products = await _redisService.ConsumeAllProductsFromQueue();
        if (products == null || !products.Any())
            return Ok("Nenhum produto na fila.");

        return Ok(new
        {
            Message = "Todos os produtos consumidos.",
            Products = products
        });
    }
}
