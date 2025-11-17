using CryptoTrading.Core.Entities;

namespace CryptoTrading.Core.Interfaces;

public interface IKlineRepository
{
    Task BulkUpsertAsync(IEnumerable<BinanceKline> klines);
}