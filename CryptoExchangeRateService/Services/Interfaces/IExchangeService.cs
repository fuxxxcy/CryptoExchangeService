using CryptoExchangeRateService.Models;

namespace CryptoExchangeRateService.Services.Interfaces;

public interface IExchangeService
{
    Task<EstimateModelResponse> Estimate(EstimateModelRequest estimateModelRequest);
    Task<IEnumerable<ExchangeRate>> GetRates(GetRatesModelRequest ratesModelRequest);
}