using ADS.Refactoring.Models;
using ADS.Refactoring.Services;

namespace ADS.Refactoring.Tests;

public class ReportServiceTests
{
    [Fact]
    public void CalculateMonthlyTotalUsd_FiltersCorrectMonth()
    {
        var sales = new List<Sale>
        {
            new()
            {
                Date = new DateTime(2026, 11, 5),
                Amount = 100m,
                Currency = "USD"
            },
            new()
            {
                Date = new DateTime(2026, 11, 10),
                Amount = 100m,
                Currency = "EUR"
            },
            new()
            {
                Date = new DateTime(2026, 11, 20),
                Amount = 100m,
                Currency = "GBP"
            },
            new()
            {
                Date = new DateTime(2026, 10, 10),
                Amount = 500m,
                Currency = "USD"
            }
        };

        var service = new ReportService(new CurrencyConverter());

        var result = service.CalculateMonthlyTotalUsd(
                sales,
                2026,
                11);

        Assert.Equal(340m, result);
    }

    [Fact]
    public void CalculateMonthlyTotalUsd_NoSalesInMonth_ReturnsZero()
    {
        var sales = new List<Sale>
        {
            new()
            {
                Date = new DateTime(2026, 10, 10),
                Amount = 500m,
                Currency = "USD"
            }
        };

        var service = new ReportService(new CurrencyConverter());

        var result = service.CalculateMonthlyTotalUsd(sales, 2026, 11);

        Assert.Equal(0m, result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(13)]
    public void CalculateMonthlyTotalUsd_InvalidMonth_Throws(int month)
    {
        var service = new ReportService(new CurrencyConverter());

        Assert.Throws<ArgumentOutOfRangeException>(
            () => service.CalculateMonthlyTotalUsd(new List<Sale>(), 2026, month));
    }

    [Fact]
    public void CalculateMonthlyTotalUsd_UnsupportedCurrencyInMonth_Throws()
    {
        var sales = new List<Sale>
        {
            new()
            {
                Date = new DateTime(2026, 11, 5),
                Amount = 100m,
                Currency = "CHF"
            }
        };

        var service = new ReportService(new CurrencyConverter());

        Assert.Throws<NotSupportedException>(
            () => service.CalculateMonthlyTotalUsd(sales, 2026, 11));
    }

    [Fact]
    public void CalculateMonthlyTotalUsd_UnsupportedCurrencyInOtherMonth_IsIgnored()
    {
        var sales = new List<Sale>
        {
            new()
            {
                Date = new DateTime(2026, 11, 5),
                Amount = 100m,
                Currency = "USD"
            },
            new()
            {
                Date = new DateTime(2026, 10, 5),
                Amount = 100m,
                Currency = "CHF"
            }
        };

        var service = new ReportService(new CurrencyConverter());

        var result = service.CalculateMonthlyTotalUsd(sales, 2026, 11);

        Assert.Equal(100m, result);
    }
}