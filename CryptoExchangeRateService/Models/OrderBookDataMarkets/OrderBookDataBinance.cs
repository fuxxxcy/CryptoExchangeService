using Newtonsoft.Json;

namespace CryptoExchangeRateService.Models.OrderBookDataMarkets;

public class OrderBookDataBinance
{
    [JsonProperty("bids")]
    public List<List<string>> Bids { get; set; }
    [JsonProperty("asks")]
    public List<List<string>> Asks { get; set; }
}