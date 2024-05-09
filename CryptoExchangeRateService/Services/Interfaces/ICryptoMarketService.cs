using CryptoExchangeRateService.Models;

namespace CryptoExchangeRateService.Services.Interfaces;

public interface ICryptoMarketService
{
    Task<ExchangeRate> GetExchangeRateAsync(GetRatesModelRequest ratesModelRequest);
}