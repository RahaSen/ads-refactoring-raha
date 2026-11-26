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

Der unveränderte Ausgangscode liegt zum Vergleich unter `docs/original/ReportService.cs.txt`.

## Gefundene Fehler im Originalcode

Neben der Struktur enthielt der Ausgangscode mehrere Fehler, die im Zuge des Refactorings behoben wurden:

- **Kompiliert nicht:** `total` wird ohne Initialisierung verwendet, `lines` ist nicht definiert (gemeint ist `reportRows`), und nach dem Listen-Initializer sowie nach `Console.WriteLine(...)` fehlt jeweils das Semikolon.
- **Report wird in der Schleife geschrieben:** Die Datei wird pro Verkauf neu geschrieben. Für einen Monat ohne Verkäufe entsteht dadurch gar kein Report.
- **Unbekannte Währungen werden still ignoriert:** Der Gesamtumsatz wäre in diesem Fall falsch, ohne dass es auffällt.
- **`double` für Geldbeträge:** führt zu Rundungsfehlern.
- **Fehlende Null-Prüfung:** `JsonSerializer.Deserialize` kann `null` liefern.
- **Tippfehler im Titel:** Die schließende Klammer in `({month}/{year}` fehlt.

## Bewusste Abweichungen vom Original

- Für einen Monat ohne Verkäufe wird ein Report mit Gesamtumsatz `0.00` geschrieben.
- Der Betrag wird immer mit zwei Nachkommastellen und Punkt als Dezimaltrenner ausgegeben, unabhängig von der Spracheinstellung des Rechners.
- Der Monat wird im Titel und im Dateinamen zweistellig ausgegeben (`report_2026_05.txt`).
- Die Report-Datei wird neben der Anwendung abgelegt (`AppContext.BaseDirectory`), nicht im aktuellen Arbeitsverzeichnis.

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

Die Währungsumrechnung, die Berechnung des monatlichen Gesamtumsatzes und die Report-Ausgabe werden mit xUnit Unit Tests überprüft. Abgedeckt sind auch die Randfälle: Monat ohne Verkäufe, ungültiger Monat, nicht unterstützte oder fehlende Währung und abweichende Schreibweise des Währungscodes.

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
