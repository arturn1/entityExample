using System.Text.Json;
using StackExchange.Redis;

public class RedisService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly HttpClient _httpClient;

    public RedisService(IConnectionMultiplexer redis, HttpClient httpClient)
    {
        _redis = redis;
        _httpClient = httpClient;
    }

    public async Task<string> GetValueAsync(string key)
    {
        var db = _redis.GetDatabase();
        return await db.StringGetAsync(key);
    }

    public async Task SetValueAsync(string key, string value)
    {
        var db = _redis.GetDatabase();
        await db.StringSetAsync(key, value);
    }

    public async Task AddProduct(Product product)
    {
        // URL do endpoint onde será feita a requisição
        var url = "http://127.0.0.1:3000/add-product";

        // Serializa o produto para JSON
        var productJson = JsonSerializer.Serialize(product);

        // Configura o conteúdo da requisição com o cabeçalho adequado
        var content = new StringContent(productJson, System.Text.Encoding.UTF8, "application/json");

        try
        {
            // Faz a requisição POST para o endpoint
            var response = await _httpClient.PostAsync(url, content);

            // Verifica o status da resposta
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Produto adicionado com sucesso!");
            }
            else
            {
                Console.WriteLine($"Falha ao adicionar produto. Status: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao enviar requisição: {ex.Message}");
        }
    }

    // Adicionar um produto na fila 'products'
    public async Task AddProductToQueue(Product product)
    {

        var db = _redis.GetDatabase();

        // Serializa o job no formato correto esperado pelo Bull
        var jobJson = JsonSerializer.Serialize(product);

        // Adiciona o job na lista 'bull:products:wait' para o Bull processar
        await db.ListLeftPushAsync("productscsharp", jobJson);
    }

    // Consumir um produto da fila 'products'
    public async Task<Product> ConsumeProductFromQueue()
    {
        var db = _redis.GetDatabase();
        var productJson = await db.ListRightPopAsync("productscsharp");  // Remove da fila 'products' (RPOP)
        if (!productJson.IsNullOrEmpty)
        {
            return JsonSerializer.Deserialize<Product>(productJson);  // Converte JSON de volta para o objeto Produto
        }
        return null;
    }

    public async Task<List<Product>> ConsumeAllProductsFromQueue()
    {
        var products = new List<Product>();
        while (true)
        {
            var product = await ConsumeProductFromQueue();
            if (product == null) break;

            products.Add(product);
        }

        return products;
    }

}
