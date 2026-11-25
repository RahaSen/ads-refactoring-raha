namespace ADS.Refactoring.Services;

public sealed class CurrencyConverter
{
    private static readonly IReadOnlyDictionary<string, decimal> RatesToUsd =
        new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
        {
            ["USD"] = 1.0m,
            ["EUR"] = 1.1m,
            ["GBP"] = 1.3m
        };

    public decimal ConvertToUsd(decimal amount, string currency)
    {
        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new ArgumentException(
                "Currency must be provided.",
                nameof(currency));
        }

        if (!RatesToUsd.TryGetValue(currency, out var rate))
        {
            throw new NotSupportedException(
                $"Currency '{currency}' is not supported.");
        }

        return amount * rate;
    }
}