using CryptoTrading.Core.Entities;
using CryptoTrading.Core.Models;

namespace CryptoTrading.Core.Interfaces;

public interface IBinanceRepository
{
    Task BulkUpsertAsync(IEnumerable<Kline> klines);
}
