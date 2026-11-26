using ADS.Refactoring.Services;

namespace ADS.Refactoring.Tests;

public class CurrencyConverterTests
{
    private readonly CurrencyConverter _converter = new();

    [Fact]
    public void ConvertToUsd_Usd_ReturnsSameAmount()
    {
        var result = _converter.ConvertToUsd(100m, "USD");
        Assert.Equal(100m, result);
    }

    [Fact]
    public void ConvertToUsd_Eur_ConvertsCorrectly()
    {
        var result = _converter.ConvertToUsd(100m, "EUR");
        Assert.Equal(110m, result);
    }

    [Fact]
    public void ConvertToUsd_Gbp_ConvertsCorrectly()
    {
        var result = _converter.ConvertToUsd(100m, "GBP");
        Assert.Equal(130m, result);
    }

    [Fact]
    public void ConvertToUsd_UnsupportedCurrency_Throws()
    {
        Assert.Throws<NotSupportedException>(() => _converter.ConvertToUsd(100m, "CHF"));
    }

    [Theory]
    [InlineData("eur")]
    [InlineData("Eur")]
    public void ConvertToUsd_CurrencyCodeIsCaseInsensitive(string currency)
    {
        var result = _converter.ConvertToUsd(100m, currency);
        Assert.Equal(110m, result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void ConvertToUsd_MissingCurrency_Throws(string currency)
    {
        Assert.Throws<ArgumentException>(() => _converter.ConvertToUsd(100m, currency));
    }
}