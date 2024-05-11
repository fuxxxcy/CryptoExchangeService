using Newtonsoft.Json;

namespace CryptoExchangeRateService.Helpers;

public class ExchangeClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ExchangeClient> _logger;
    
    public ExchangeClient(IHttpClientFactory httpClientFactory, ILogger<ExchangeClient> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<TResult> GetOrderBookAsync<TResult>(Uri requestUri) where TResult : class
    {
        using var client = _httpClientFactory.CreateClient();

        HttpResponseMessage response = await client.GetAsync(requestUri);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogCritical($"HTTP request failed with status code {response.StatusCode}");
            return null;
        }

        string json = response.Content.ReadAsStringAsync().Result;
        var result = JsonConvert.DeserializeObject<TResult>(json);
        
        return result;
    }
}