namespace CryptoExchangeRateService.Models;

public class EstimateModelRequest
{
    public decimal InputAmount { get; set; }
    public string InputCurrency { get; set; }
    public string OutputCurrency { get; set; }
}