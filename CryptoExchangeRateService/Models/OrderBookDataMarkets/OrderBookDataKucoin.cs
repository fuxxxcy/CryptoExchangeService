namespace CryptoExchangeRateService.Models.OrderBookDataMarkets;

public class OrderBookDataKucoin
{
    public OrderBookDataItemKucoin Data { get; set; }
}

public class OrderBookDataItemKucoin
{
    public List<List<string>> Bids { get; set; }
    public List<List<string>> Asks { get; set; }
}