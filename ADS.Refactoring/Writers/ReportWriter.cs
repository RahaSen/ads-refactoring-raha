namespace ADS.Refactoring.Writers;

public sealed class ReportWriter
{
    public void Write(
        string filePath,
        int year,
        int month,
        decimal totalUsd)
    {
        var reportRows = new[]
        {
            $"Monatlicher Verkaufsbericht ({month:D2}/{year})",
            "---------------------------------",
            $"Gesamt Umsatz in USD: {totalUsd:F2}"
        };

        File.WriteAllLines(filePath, reportRows);
    }
}