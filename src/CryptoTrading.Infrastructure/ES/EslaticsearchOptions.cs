namespace CryptoTrading.Infrastructure;

public class ElasticsearchOptions
{
    public string Url { get; set; } = "http://localhost:9200";
    public string DefaultIndex { get; set; } = "binance-klines";
}
