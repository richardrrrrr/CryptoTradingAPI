using CryptoTrading.API.Validaton;

namespace CryptoTrading.API.Contracts;

public class GetKlinesQuery
{
    public string Symbol { get; set; } = null!;

    [KlineInterval]
    public string Interval { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}
