namespace CryptoExchangeRateService.Models;

public class GetRatesModelRequest
{
    public string BaseCurrency { get; set; }
    public string QuoteCurrency { get; set; }
}