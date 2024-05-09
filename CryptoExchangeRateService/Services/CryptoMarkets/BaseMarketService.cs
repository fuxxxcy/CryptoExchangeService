namespace CryptoExchangeRateService.Services.CryptoMarkets;

public abstract class BaseMarketService<TData> where TData : class
{
    private readonly ILogger<BaseMarketService<TData>> _logger;

    protected BaseMarketService(ILogger<BaseMarketService<TData>> logger)
    {
        _logger = logger;
    }

    protected decimal? GetAveragePrice(List<List<string>> bids, List<List<string>> asks)
    {
        decimal totalBidVolume = 0;
        decimal totalAskVolume = 0;
        decimal totalBidPriceVolume = 0;
        decimal totalAskPriceVolume = 0;
        
        try
        {
            foreach (var bid in bids)
            {
                decimal price = decimal.Parse(bid[0]);
                decimal volume = decimal.Parse(bid[1]);
                totalBidVolume += volume;
                totalBidPriceVolume += price * volume;
            }
            
            foreach (var ask in asks)
            {
                decimal price = decimal.Parse(ask[0]);
                decimal volume = decimal.Parse(ask[1]);
                totalAskVolume += volume;
                totalAskPriceVolume += price * volume;
            }
            
            decimal averagePrice = (totalBidPriceVolume + totalAskPriceVolume) / (totalBidVolume + totalAskVolume);

            return averagePrice;
        }
        catch (Exception e)
        {
            _logger.LogCritical(e.Message);
            return null;
        }
    }
}