using System.Text.Json;
using ADS.Refactoring.Models;

namespace ADS.Refactoring.Repositories;

public sealed class SalesRepository
{
    public IReadOnlyList<Sale> Load(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(
                $"Sales file was not found: {filePath}");
        }

        var json = File.ReadAllText(filePath);

        var sales = JsonSerializer.Deserialize<List<Sale>>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return sales
            ?? throw new InvalidDataException(
                "The sales file could not be deserialized.");
    }
}