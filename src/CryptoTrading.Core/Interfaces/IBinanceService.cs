using CryptoTrading.Core.Entities;
using CryptoTrading.Core.Models;

namespace CryptoTrading.Core.Interfaces
{
    public interface IBinanceService
    {
        Task<decimal?> GetLatestPriceAsync(string symbol);
        Task<IReadOnlyList<Kline>> GetKlinesAsync(KlineRequest request);
    }
}
