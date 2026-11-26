using System.Globalization;
using ADS.Refactoring.Writers;

namespace ADS.Refactoring.Tests;

public class ReportWriterTests
{
    [Fact]
    public void Write_CreatesReportWithExpectedContent()
    {
        var filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.txt");

        try
        {
            new ReportWriter().Write(filePath, 2026, 5, 340m);

            var lines = File.ReadAllLines(filePath);

            Assert.Equal(3, lines.Length);
            Assert.Equal("Monatlicher Verkaufsbericht (05/2026)", lines[0]);
            Assert.Equal("Gesamt Umsatz in USD: 340.00", lines[2]);
        }
        finally
        {
            File.Delete(filePath);
        }
    }

    [Fact]
    public void Write_UsesSameNumberFormatRegardlessOfCulture()
    {
        var filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.txt");
        var originalCulture = CultureInfo.CurrentCulture;

        try
        {
            CultureInfo.CurrentCulture = new CultureInfo("de-DE");

            new ReportWriter().Write(filePath, 2026, 11, 1234.5m);

            var lines = File.ReadAllLines(filePath);

            Assert.Equal("Gesamt Umsatz in USD: 1234.50", lines[2]);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
            File.Delete(filePath);
        }
    }
}
