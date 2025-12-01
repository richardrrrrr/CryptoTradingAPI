using CryptoTrading.Core.Models;

namespace CryptoTrading.Core.Interfaces;

public interface IEslaticsearchService
{
    Task SyncToEsAsync(IEnumerable<Kline> klines);
}
