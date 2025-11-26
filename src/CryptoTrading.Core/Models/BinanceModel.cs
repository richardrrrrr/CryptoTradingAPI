namespace CryptoTrading.Core.Models;

public class KlineRequest
{
    public string Symbol { get; set; } = null!;
    public KlineIntervalValue Interval { get; set; } = KlineIntervalValue.FifteenMinutes;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}

public class Kline
{
    public string Symbol { get; set; } = null!;
    public KlineIntervalValue Interval { get; set; } = KlineIntervalValue.FifteenMinutes;
    public DateTime OpenTime { get; set; }
    public DateTime CloseTime { get; set; }

    public decimal OpenPrice { get; set; }
    public decimal HighPrice { get; set; }
    public decimal LowPrice { get; set; }
    public decimal ClosePrice { get; set; }

    public decimal Volume { get; set; }
    public int? Trades { get; set; }
}

public sealed class KlineIntervalValue
{
    public string Code { get; }

    private KlineIntervalValue(string code)
    {
        Code = code;
    }

    // 預先定義有效的 interval
    public static readonly KlineIntervalValue OneMinute = new("1m");
    public static readonly KlineIntervalValue FiveMinutes = new("5m");
    public static readonly KlineIntervalValue FifteenMinutes = new("15m");
    public static readonly KlineIntervalValue OneHour = new("1h");
    public static readonly KlineIntervalValue OneDay = new("1d");

    public static KlineIntervalValue From(string code)
    {
        return code switch
        {
            "1m" => OneMinute,
            "5m" => FiveMinutes,
            "15m" => FifteenMinutes,
            "1h" => OneHour,
            "1d" => OneDay,
            _ => throw new ArgumentException($"Invalid Kline interval: {code}"),
        };
    }

    public override string ToString() => Code;

    public override bool Equals(object? obj)
    {
        return obj is KlineIntervalValue other && Code == other.Code;
    }

    public override int GetHashCode()
    {
        return Code.GetHashCode();
    }
}
