using CryptoTrading.Core.Models;

namespace CryptoTrading.Core.Interfaces;

public interface IBinanceDataApplicationService
{
    Task SyncKlinesAsync(KlineRequest request, CancellationToken cancellationToken = default);

    Task<decimal?> GetLatestPriceAsync(
        string symbol,
        CancellationToken cancellationToken = default
    );
}
