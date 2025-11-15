namespace CryptoTrading.Core.Interfaces
{
    public interface IBinanceService
    {
        Task<decimal?> GetLatestPriceAsync(string symbol);
    }
}