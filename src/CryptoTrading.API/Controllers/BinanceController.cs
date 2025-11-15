using CryptoTrading.Infrastructure.Binance;
using CryptoTrading.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CryptoTrading.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BinanceController : ControllerBase
    {
        private readonly IBinanceService _binanceService;
        public BinanceController(IBinanceService binanceService)
        {
            _binanceService = binanceService;
        }

        /// <summary>
        /// 測試：取得最新價格
        /// </summary>
        [HttpGet("price/{symbol}")]
        public async Task<IActionResult> GetLatestPrice(string symbol)
        {
            var price = await _binanceService.GetLatestPriceAsync(symbol);

            if (price == null)
                return BadRequest("Failed to fetch price. Check symbol or credentials.");

            return Ok(new { Symbol = symbol, Price = price });
        }
    }
}