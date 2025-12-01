using CryptoTrading.Core.Interfaces;
using CryptoTrading.Core.Models;

namespace CryptoTrading.API.Services;

public class BinanceDataApplicationService(
    IEslaticsearchService eslaticsearchService,
    IBinanceService binanceService,
    IBinanceRepository binanceRepository
) : IBinanceDataApplicationService
{
    private readonly IEslaticsearchService _eslaticsearchService = eslaticsearchService;
    private readonly IBinanceService _binanceService = binanceService;
    private readonly IBinanceRepository _binanceRepository = binanceRepository;

    public async Task<decimal?> GetLatestPriceAsync(
        string symbol,
        CancellationToken cancellationToken = default
    )
    {
        return await _binanceService.GetLatestPriceAsync(symbol);
    }

    public async Task SyncKlinesAsync(
        KlineRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var klines = await _binanceService.GetKlinesAsync(request);

        // 這裡可以做去重、只存新資料等等商業邏輯
        await _binanceRepository.BulkUpsertAsync(klines);
        //同步到 ES
        await _eslaticsearchService.SyncToEsAsync(klines);
    }
}
