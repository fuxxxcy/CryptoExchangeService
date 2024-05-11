using System.Collections.Concurrent;
using CryptoExchangeRateService.Models;
using CryptoExchangeRateService.Services.Interfaces;

namespace CryptoExchangeRateService.Services;

public class ExchangeService : IExchangeService
{
    private readonly IEnumerable<ICryptoMarketService> _exchangeServices;
    
    public ExchangeService(IEnumerable<ICryptoMarketService> exchangeServices)
    {
        _exchangeServices = exchangeServices;
    }

    public async Task<EstimateModelResponse> Estimate(EstimateModelRequest estimateModelRequest)
    {
        GetRatesModelRequest ratesModelRequest = new GetRatesModelRequest
        {
            BaseCurrency = estimateModelRequest.InputCurrency,
            QuoteCurrency = estimateModelRequest.OutputCurrency
        };

        var rates = new ConcurrentBag<ExchangeRate>();
        await Parallel.ForEachAsync(_exchangeServices, async (service, _) => rates.Add(await service.GetExchangeRateAsync(ratesModelRequest)));

        if (rates is null || rates.All(x => x is null))
        {
            return null;
        }

        var exchangeRateWithMaxRate = rates.MaxBy(rate => rate?.Rate);
        
        EstimateModelResponse response = new EstimateModelResponse
        {
            ExchangeName = exchangeRateWithMaxRate.ExchangeName,
            OutputAmount = estimateModelRequest.InputAmount * exchangeRateWithMaxRate.Rate     
        };
        
        return response;
    }

    public async Task<IEnumerable<ExchangeRate>> GetRates(GetRatesModelRequest ratesModelRequest)
    {
        var rates = new ConcurrentBag<ExchangeRate>();
        await Parallel.ForEachAsync(_exchangeServices, async (service, _) => rates.Add(await service.GetExchangeRateAsync(ratesModelRequest)));
        
        return rates;
    }
}