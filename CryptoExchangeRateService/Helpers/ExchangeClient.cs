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

    public async Task<TResult> GetPriceAsync<TResult>(Uri requestUri) where TResult : class
    {
        using var client = _httpClientFactory.CreateClient();

        HttpResponseMessage response = await client.GetAsync(requestUri);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogCritical($"HTTP request failed with status code {response.StatusCode}");
        }

        string json = response.Content.ReadAsStringAsync().Result;

        return JsonConvert.DeserializeObject<TResult>(json);

    }
}