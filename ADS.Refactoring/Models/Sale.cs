namespace ADS.Refactoring.Models;

public sealed class Sale
{
    public DateTime Date { get; init; }
    public decimal Amount { get; init; }
    public string Currency { get; init; } = string.Empty;
}