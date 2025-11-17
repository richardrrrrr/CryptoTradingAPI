using System;
using System.Collections.Generic;

namespace CryptoTrading.Core.Entities;

public partial class BinanceKline
{
    public long Id { get; set; }

    public string Symbol { get; set; } = null!;

    public string Interval { get; set; } = null!;

    public DateTime OpenTime { get; set; }

    public DateTime CloseTime { get; set; }

    public decimal OpenPrice { get; set; }

    public decimal HighPrice { get; set; }

    public decimal LowPrice { get; set; }

    public decimal ClosePrice { get; set; }

    public decimal Volume { get; set; }

    public int? Trades { get; set; }

    public DateTime CreatedAt { get; set; }
}
