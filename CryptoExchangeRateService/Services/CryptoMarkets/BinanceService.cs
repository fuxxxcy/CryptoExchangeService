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
        var uriBuilder = new UriBuilder($"{CryptoMarketUrls.BinanceApi}/api/v3/depth")
        {
            Query = $"limit=10&symbol={getRates.BaseCurrency.ToUpper()}{getRates.QuoteCurrency.ToUpper()}"
        };

        var response = await _exchangeClient.GetPriceAsync<OrderBookDataBinance>(uriBuilder.Uri);
        
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
            ExchangeName = "Binance",
            Rate = avgPrice.Value
        };

        return exchangeRate;
    }

    protected virtual decimal? GetAveragePrice(OrderBookDataBinance model)
    {
        return GetAveragePrice(model.Bids, model.Asks);
    }
}