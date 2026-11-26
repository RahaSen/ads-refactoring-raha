using System.Globalization;
using ADS.Refactoring.Repositories;
using ADS.Refactoring.Services;
using ADS.Refactoring.Writers;

var year = 2026;
var month = 11;

var salesFilePath =
    Path.Combine(AppContext.BaseDirectory, "sales.json");

var reportFilePath =
    Path.Combine(
        AppContext.BaseDirectory,
        $"report_{year}_{month:D2}.txt");

try
{
    var salesRepository = new SalesRepository();
    var currencyConverter = new CurrencyConverter();
    var reportService = new ReportService(currencyConverter);
    var reportWriter = new ReportWriter();

    var sales = salesRepository.Load(salesFilePath);

    var totalUsd =
        reportService.CalculateMonthlyTotalUsd(
            sales,
            year,
            month);

    reportWriter.Write(
        reportFilePath,
        year,
        month,
        totalUsd);

    Console.WriteLine(
        $"Report generated successfully: {reportFilePath}");

    Console.WriteLine(
        $"Total revenue in USD: {totalUsd.ToString("F2", CultureInfo.InvariantCulture)}");
}
catch (Exception ex)
{
    Console.Error.WriteLine(
        $"Report generation failed: {ex.Message}");

    Environment.ExitCode = 1;
}