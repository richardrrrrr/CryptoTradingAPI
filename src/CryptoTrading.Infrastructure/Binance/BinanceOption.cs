using System.ComponentModel.DataAnnotations;

namespace CryptoTrading.Infrastructure
{
    public class BinanceOptions
    {
        [Required]
        public string ApiKey { get; set; } = string.Empty;

        [Required]
        public string ApiSecret { get; set; } = string.Empty;

        public EnvironmentOption Environment { get; set; } = new();

        // 自動根據環境名稱判斷是否是 Testnet
        public bool UseTestnet =>
            string.Equals(Environment?.Name, "testnet", StringComparison.OrdinalIgnoreCase);
    }

    public class EnvironmentOption
    {
        public string Name { get; set; } = "live"; // 預設 live
    }
}
