# ADS Backend-Refactoring Task

Refactoring einer bestehenden .NET-Anwendung zur Erstellung eines monatlichen Verkaufsberichts.

## Tech Stack

- C#
- .NET 10
- xUnit
- System.Text.Json

## Ausgangssituation

Der ursprüngliche `ReportService` enthielt mehrere Verantwortlichkeiten innerhalb einer Methode:

- Lesen und Deserialisieren der JSON-Datei
- Filtern der Verkaufsdaten
- Währungsumrechnung
- Berechnung des Gesamtumsatzes
- Erstellen und Schreiben des Reports

Ziel war es, diese Verantwortlichkeiten klar zu trennen und die Business-Logik besser wartbar und testbar zu machen.

## Struktur

- `Models/Sale.cs` – Datenmodell für Verkäufe
- `Repositories/SalesRepository.cs` – Lesen und Deserialisieren der Verkaufsdaten
- `Services/CurrencyConverter.cs` – Umrechnung unterstützter Währungen in USD
- `Services/ReportService.cs` – Filterung und Berechnung des monatlichen Umsatzes
- `Writers/ReportWriter.cs` – Erstellung der Report-Datei
- `ADS.Refactoring.Tests` – Unit Tests für Business-Logik und Währungsumrechnung

## Wichtige Refactoring-Entscheidungen

### Separation of Concerns

Die unterschiedlichen Verantwortlichkeiten wurden auf separate Klassen verteilt. Dadurch ist die Business-Logik unabhängig von Datei-I/O und Report-Ausgabe testbar.

### Decimal für Beträge

Für Geldbeträge wird `decimal` anstelle von `double` verwendet, um typische Floating-Point-Ungenauigkeiten bei finanziellen Berechnungen zu vermeiden.

### Explizite Fehlerbehandlung

Nicht unterstützte Währungen werden nicht still ignoriert, sondern führen zu einer `NotSupportedException`. Dadurch können falsche Gesamtumsätze vermieden werden.

### Testbarkeit

Die Währungsumrechnung und die Berechnung des monatlichen Gesamtumsatzes werden mit xUnit Unit Tests überprüft.

## Anwendung starten

```bash
dotnet run --project ADS.Refactoring
```

## Tests ausführen

```bash
dotnet test ADS.Refactoring.slnx
```

## Build

```bash
dotnet build ADS.Refactoring.slnx
```

- Die enthaltene `sales.json` dient als Beispieldatensatz zur Demonstration der Funktionalität.
