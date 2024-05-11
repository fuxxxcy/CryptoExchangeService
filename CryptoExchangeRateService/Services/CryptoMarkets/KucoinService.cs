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
        ExchangeRate exchangeRate = new ExchangeRate();
        bool isReversed = false;
        
        var uriBuilder = new UriBuilder(CryptoMarketUrls.KucoinApi) 
        { 
            Path = "/api/v1/market/orderbook/level2_20", 
            Query = $"symbol={getRates.BaseCurrency.ToUpper()}-{getRates.QuoteCurrency.ToUpper()}"
        };
        
        var response = await _exchangeClient.GetOrderBookAsync<OrderBookDataKucoin>(uriBuilder.Uri);

        if (response is null || response.Data.Asks == null || response.Data.Bids == null)
        {
            uriBuilder = new UriBuilder(CryptoMarketUrls.KucoinApi) 
            { 
                Path = "/api/v1/market/orderbook/level2_20", 
                Query = $"symbol={getRates.QuoteCurrency.ToUpper()}-{getRates.BaseCurrency.ToUpper()}"
            };
            response = await _exchangeClient.GetOrderBookAsync<OrderBookDataKucoin>(uriBuilder.Uri);

            isReversed = true;
        }
        
        if (response is null || response.Data.Asks == null || response.Data.Bids == null)
        {
            return null;
        }
        
        var avgPrice = isReversed ? (1 / GetAveragePrice(response.Data.Asks)) : GetAveragePrice(response.Data.Bids);

        if (avgPrice is null)
        {
            return null;
        }

        exchangeRate.ExchangeName = "Kucoin";
        exchangeRate.Rate = avgPrice.Value;

        return exchangeRate;
    }
    
    protected virtual decimal? GetAveragePrice(List<List<string>> orderBook)
    {
        return base.GetAveragePrice(orderBook);
    }
}