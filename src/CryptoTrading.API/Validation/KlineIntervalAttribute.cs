using System.ComponentModel.DataAnnotations;

namespace CryptoTrading.API.Validaton;

public class KlineIntervalAttribute : ValidationAttribute
{
    //StringComparer.OrdinalIgnoreCase：讓比較不分大小寫，例如 "1H" 也能通過。
    private static readonly HashSet<string> _allowed = new(StringComparer.OrdinalIgnoreCase)
    {
        "1m",
        "5m",
        "15m",
        "1h",
        "1d",
    };

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
            return new ValidationResult("Interval is required.");
        if (value is not string s)
            return new ValidationResult("Interval must be a string.");

        if (!_allowed.Contains(s))
            return new ValidationResult(
                $"Interval '{s}' is not supported. Allowed: 1m, 5m, 15m, 1h, 1d."
            );

        return ValidationResult.Success;
    }
}
