using CryptoTrading.API.Contracts;
using CryptoTrading.Core.Interfaces;
using CryptoTrading.Core.Models;
using CryptoTrading.Infrastructure.Binance;
using Microsoft.AspNetCore.Mvc;

namespace CryptoTrading.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BinanceController(IBinanceDataApplicationService binanceDataApplicationService)
        : ControllerBase
    {
        private readonly IBinanceDataApplicationService _binanceDataApplicationService =
            binanceDataApplicationService;

        /// <summary>
        /// 測試：取得最新價格
        /// </summary>
        [HttpGet("price/{symbol}")]
        public async Task<IActionResult> GetLatestPrice(string symbol)
        {
            var price = await _binanceDataApplicationService.GetLatestPriceAsync(symbol);

            if (price == null)
                return BadRequest("Failed to fetch price. Check symbol or credentials.");

            return Ok(new { Symbol = symbol, Price = price });
        }

        /// <summary>
        /// 取得歷史K線資料
        /// 例如: /api/binance/klines?symbol=BTCUSDT&interval=15m&startTime=2024-01-01T00:00:00Z&endTime=2024-01-01T06:00:00Z
        /// </summary>
        [HttpGet("klines/sync")]
        public async Task<IActionResult> SyncKlines([FromQuery] GetKlinesQuery query)
        {
            
            var request = new KlineRequest
            {
                Symbol = query.Symbol,
                Interval = KlineIntervalValue.From(query.Interval),
                StartTime = query.StartTime,
                EndTime = query.EndTime,
            };

            await _binanceDataApplicationService.SyncKlinesAsync(request);
            return Ok();
        }
    }
}
