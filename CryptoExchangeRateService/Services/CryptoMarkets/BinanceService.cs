using CryptoExchangeRateService.EnvironmentSettings;
using CryptoExchangeRateService.Helpers;
using CryptoExchangeRateService.Models;
using CryptoExchangeRateService.Models.OrderBookDataMarkets;
using CryptoExchangeRateService.Services.Interfaces;

namespace CryptoExchangeRateService.Services.CryptoMarkets;

public class BinanceService : BaseMarketService<OrderBookDataBinance>, ICryptoMarketService
{
    private readonly ExchangeClient _exchangeClient;
    
    public BinanceService(ExchangeClient exchangeClient, ILogger<BaseMarketService<OrderBookDataBinance>> logger) : base(logger)
    {
        _exchangeClient = exchangeClient;
    }

    public async Task<ExchangeRate> GetExchangeRateAsync(GetRatesModelRequest getRates)
    {
        ExchangeRate exchangeRate = new ExchangeRate();
        bool isReversed = false;
        
        var uriBuilder = new UriBuilder($"{CryptoMarketUrls.BinanceApi}/api/v3/depth")
        {
            Query = $"limit=10&symbol={getRates.BaseCurrency.ToUpper()}{getRates.QuoteCurrency.ToUpper()}"
        };

        var response = await _exchangeClient.GetOrderBookAsync<OrderBookDataBinance>(uriBuilder.Uri);
        
        if (response is null || response.Asks == null || response.Bids == null)
        {
            uriBuilder.Query = $"limit=10&symbol={getRates.QuoteCurrency.ToUpper()}{getRates.BaseCurrency.ToUpper()}";
            response = await _exchangeClient.GetOrderBookAsync<OrderBookDataBinance>(uriBuilder.Uri);

            isReversed = true;
        }

        if (response is null || response.Asks == null || response.Bids == null)
        {
            return null;
        }
        
        var avgPrice = isReversed ? (1 / GetAveragePrice(response.Asks)) : GetAveragePrice(response.Bids);

        if (avgPrice is null)
        {
            return null;
        }

        exchangeRate.ExchangeName = "Binance";
        exchangeRate.Rate = avgPrice.Value;

        return exchangeRate;
    }

    protected virtual decimal? GetAveragePrice(List<List<string>> orderBook)
    {
        return base.GetAveragePrice(orderBook);
    }
}