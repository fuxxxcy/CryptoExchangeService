using CryptoExchangeRateService.EnvironmentSettings;
using CryptoExchangeRateService.Helpers;
using CryptoExchangeRateService.Models;
using CryptoExchangeRateService.Models.OrderBookDataMarkets;
using CryptoExchangeRateService.Services.Interfaces;

namespace CryptoExchangeRateService.Services.CryptoMarkets;

public class KucoinService : BaseMarketService<OrderBookDataKucoin>, ICryptoMarketService
{
    private readonly ExchangeClient _exchangeClient;

    public KucoinService(ExchangeClient exchangeClient, ILogger<BaseMarketService<OrderBookDataKucoin>> logger) : base(logger)
    {
        _exchangeClient = exchangeClient;
    }
    
    public async Task<ExchangeRate> GetExchangeRateAsync(GetRatesModelRequest getRates)
    { 
        var uriBuilder = new UriBuilder(CryptoMarketUrls.KucoinApi) 
        { 
            Path = "/api/v1/market/orderbook/level2_20", 
            Query = $"symbol={getRates.BaseCurrency.ToUpper()}-{getRates.QuoteCurrency.ToUpper()}"
        };
        
        var response = await _exchangeClient.GetPriceAsync<OrderBookDataKucoin>(uriBuilder.Uri);

        if (response is null)
        {
            return null;
        }
        
        var avgPrice = GetAveragePrice(response);

        if (avgPrice is null)
        {
            return null;
        }

        var exchangeRate = new ExchangeRate
        {
            ExchangeName = "Kucoin",
            Rate = avgPrice.Value
        };

        return exchangeRate;
    }
    
    protected virtual decimal? GetAveragePrice(OrderBookDataKucoin model)
    {
        return GetAveragePrice(model.Data.Bids, model.Data.Asks);
    }
}