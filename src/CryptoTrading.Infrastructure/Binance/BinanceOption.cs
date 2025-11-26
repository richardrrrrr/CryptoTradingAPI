using System.ComponentModel.DataAnnotations;

namespace CryptoTrading.Infrastructure.Binance
{
    public class BinanceOptions
    {
        [Required]
        public string ApiKey { get; set; } = string.Empty;
        [Required]
        public string ApiSecret { get; set; } = string.Empty;
        public bool UseTestnet { get; set; } = false;
    }
}


