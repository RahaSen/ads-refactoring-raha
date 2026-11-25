using ADS.Refactoring.Models;

namespace ADS.Refactoring.Services;

public sealed class ReportService
{
    private readonly CurrencyConverter _currencyConverter;

    public ReportService(CurrencyConverter currencyConverter)
    {
        _currencyConverter = currencyConverter;
    }

    public decimal CalculateMonthlyTotalUsd(
        IEnumerable<Sale> sales,
        int year,
        int month)
    {
        ArgumentNullException.ThrowIfNull(sales);

        if (month is < 1 or > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(month));
        }

        return sales
            .Where(s => s.Date.Year == year && s.Date.Month == month)
            .Sum(s => _currencyConverter.ConvertToUsd(
                s.Amount,
                s.Currency));
    }
}