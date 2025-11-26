using System.Data;
using CryptoTrading.Core.Entities;
using CryptoTrading.Core.Interfaces;
using CryptoTrading.Core.Models;
using CryptoTrading.Infrastructure;
using CryptoTrading.Infrastructure.Data;
using Dapper;

namespace CryptoTrading.Infrastructure.Repositories;

public class BinanceRepository(CryptoTradingDbContext dbContext)
    : EFIntegrationBase(dbContext),
        IBinanceRepository
{
    public async Task BulkUpsertAsync(IEnumerable<Kline> klines)
    {
        const string sql =
            @"
IF NOT EXISTS (
    SELECT 1 FROM BinanceKline
    WHERE Symbol = @Symbol AND Interval = @Interval AND OpenTime = @OpenTime
)
BEGIN
    INSERT INTO BinanceKline
    (Symbol, Interval, OpenTime, CloseTime, OpenPrice, HighPrice, LowPrice, ClosePrice, Volume, Trades)
    VALUES
    (@Symbol, @Interval, @OpenTime, @CloseTime, @OpenPrice, @HighPrice, @LowPrice, @ClosePrice, @Volume, @Trades)
END
";

        await _conn.ExecuteAsync(sql, klines.Select(MapToDbParams));
    }

    // === Mapping ===

    private static object MapToDbParams(Kline k) =>
        new
        {
            k.Symbol,
            k.Interval,
            k.OpenTime,
            k.CloseTime,
            k.OpenPrice,
            k.HighPrice,
            k.LowPrice,
            k.ClosePrice,
            k.Volume,
            k.Trades,
        };
}
