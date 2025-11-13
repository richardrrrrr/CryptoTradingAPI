using Binance.Net;
using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Objects;
using Binance.Net.Objects.Options;
using CryptoExchange.Net.Authentication;
using CryptoTrading.Infrastructure.Binance;
using Microsoft.Extensions.Options;

namespace CryptoTrading.Infrastructure.Binance
{
    public class BinanceService
    {
        private readonly BinanceRestClient _restClient;

        //BinanceRestClient 是 Binance.Net SDK 的核心 REST 客戶端，用它呼叫所有 REST API如最新價格，歷史K線
        public BinanceService(BinanceRestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task<decimal?> GetLatestPriceAsync(string symbol)
        {
            var result = await _restClient.SpotApi.ExchangeData.GetTickerAsync(symbol);
            if (result.Success)
                return result.Data.LastPrice;

            Console.WriteLine($"Error: {result.Error?.Message}");
            return null;
        }
    }
}
