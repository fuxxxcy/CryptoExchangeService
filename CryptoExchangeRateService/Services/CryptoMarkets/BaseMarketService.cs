namespace CryptoExchangeRateService.Services.CryptoMarkets;

public abstract class BaseMarketService<TData> where TData : class
{
    private readonly ILogger<BaseMarketService<TData>> _logger;

    protected BaseMarketService(ILogger<BaseMarketService<TData>> logger)
    {
        _logger = logger;
    }

    protected decimal? GetAveragePrice(List<List<string>> orderBook)
    {
        decimal totalVolume = 0;
        decimal totalPriceVolume = 0;
        
        try
        {
            foreach (var order in orderBook)
            {
                decimal price = decimal.Parse(order[0]);
                decimal volume = decimal.Parse(order[1]);
                totalVolume += volume;
                totalPriceVolume += price * volume;
            }

            decimal averagePrice = totalPriceVolume / totalVolume;

            return averagePrice;
        }
        catch (Exception e)
        {
            _logger.LogCritical(e.Message);
            return null;
        }
    }
}