using Binance.Net;
using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Objects;
using Binance.Net.Objects.Options;
using CryptoExchange.Net.Authentication;
using CryptoTrading.Core.Interfaces;
using CryptoTrading.Core.Models;
using CryptoTrading.Infrastructure.Binance;
using Microsoft.Extensions.Options;

namespace CryptoTrading.Infrastructure.Binance
{
    public class BinanceService : IBinanceService
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

        public async Task<IReadOnlyList<Kline>> GetKlinesAsync(KlineRequest request)
        {
            var interval = ToBinanceInterval(request.Interval);
    
            var result = await _restClient.SpotApi.ExchangeData.GetKlinesAsync(
                request.Symbol,
                interval,
                startTime: request.StartTime,
                endTime: request.EndTime
            );

            if (!result.Success)
                throw new Exception(result.Error?.Message);

            return result
                .Data.Select(x => new Kline
                {
                    Symbol = request.Symbol,
                    Interval = request.Interval,
                    OpenTime = x.OpenTime,
                    CloseTime = x.CloseTime,
                    OpenPrice = x.OpenPrice,
                    HighPrice = x.HighPrice,
                    LowPrice = x.LowPrice,
                    ClosePrice = x.ClosePrice,
                    Volume = x.Volume,
                    Trades = x.TradeCount,
                })
                .ToList();
        }

        private KlineInterval ToBinanceInterval(KlineIntervalValue intervalValue)
        {
            return intervalValue.Code switch
            {
                "1m" => KlineInterval.OneMinute,
                "5m" => KlineInterval.FiveMinutes,
                "15m" => KlineInterval.FifteenMinutes,
                "1h" => KlineInterval.OneHour,
                "1d" => KlineInterval.OneDay,
                _ => throw new ArgumentException($"Unsupported interval: {intervalValue.Code}"),
            };
        }
    }
}
