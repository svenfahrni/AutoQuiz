# ADR 001: Verwendung des Strategy Pattern zum Lesen verschiedener Dateitypen

**Status:** Vorgeschlagen

---

## Kontext

Im Fileservice werden Inhalte aus verschiedenen Dateitypen wie `.txt`, `.pdf` und `.docx` gelesen. Die Art, wie Inhalte aus diesen Formaten ausgelesen werden, unterscheidet sich aufgrund unterschiedlicher Bibliotheken und Verarbeitungslogik.  
Es war notwendig, diese Unterschiede zu kapseln, um die Wartbarkeit, Erweiterbarkeit und Testbarkeit der Anwendung zu verbessern.

---

## Entscheidung

Wir haben uns entschieden, das **Strategy Pattern** zur Implementierung des Dateilesens zu verwenden.  
Dazu wurde eine Schnittstelle `IFileReaderStrategy` definiert, welche die Methode `string ReadFileContent(string filePath)` bereitstellt. Für jeden unterstützten Dateityp gibt es eine eigene Implementierung:

- `TxtFileReader` für `.txt`
- `PdfFileReader` für `.pdf`
- `DocxFileReader` für `.docx`

Der `FileService` erhält zur Laufzeit alle registrierten Strategien per Dependency Injection und wählt anhand der Dateierweiterung zur Ausführungszeit die passende aus.

---

## Gründe für diese Entscheidung

- **Offen für Erweiterung, geschlossen für Modifikation (Open/Closed Principle)**: Neue Dateiformate können einfach durch zusätzliche Strategien ergänzt werden, ohne bestehende Logik im `FileService` zu verändern.
- **Saubere Trennung von Verantwortlichkeiten**: Jede Lesestrategie ist nur für ein Format zuständig, wodurch der Code verständlich und wartbar bleibt.
- **Testbarkeit**: Strategien lassen sich einzeln testen oder durch Mocks im `FileService` ersetzen.
- **Flexibilität durch DI**: Die lose Kopplung ermöglicht einfache Integration weiterer Dateitypen ohne Eingriff in die zentrale Geschäftslogik.

---

## Alternativen

- **Direkte Formatbehandlung im `FileService`**  
  _Nachteile_: schwer testbar, schlecht wartbar, verletzt SRP (Single Responsibility Principle).

- **Factory Pattern statt Strategy Pattern**  
  _Nachteil_: Die Factory wäre hier unnötig, da keine komplexe Initialisierung erforderlich ist und die Strategien direkt aufgerufen werden können.

---

## Konsequenzen

- Zusätzlicher Initialaufwand bei der Einführung des Patterns (Schnittstelle + konkrete Klassen).
- Alle unterstützten Formate müssen manuell als Strategien registriert werden.
- Das DI-System (z. B. `Microsoft.Extensions.DependencyInjection`) muss korrekt konfiguriert sein, damit alle Strategien verfügbar sind.

---
