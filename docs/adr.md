# ADR 001: Verwendung des Strategy Pattern zum Lesen verschiedener Dateitypen

**Status:** Akzeptiert

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

# ADR 002: Verwendung von OpenAI zur automatisierten Quizgenerierung

**Status:** Akzeptiert

---

## Kontext

Ein zentrales Feature der Anwendung besteht darin, aus hochgeladenen Inhalten automatisch Quizfragen zu generieren. Eine klassische Implementierung mit regelbasierten Algorithmen wäre unflexibel, aufwendig in der Pflege und qualitativ limitiert.

---

## Entscheidung

Wir haben uns entschieden, das Sprachmodell **OpenAI GPT** über die öffentliche API zu integrieren, um Inhalte zu analysieren und daraus Fragen zu erstellen.  
Die Klasse `CardDeckGenerationServiceOpenAI` kapselt diese Funktionalität und sendet vorbereitete Texte zur Analyse an das Modell. Das Resultat wird anschließend als Fragenset zurückgegeben und an den Benutzer weitergereicht.

---

## Gründe für diese Entscheidung

- **Hohe Qualität** der generierten Inhalte durch leistungsfähige KI
- **Minimale Entwicklungszeit**, da keine eigene NLP-Logik notwendig ist
- **Einfache Erweiterbarkeit**: Parametrisierung des Prompts erlaubt flexible Anpassung des Outputs
- **Skalierbarkeit** durch Auslagerung der Rechenleistung an einen externen Dienst

---

## Alternativen

- **Regelbasierte Fragegenerierung im Backend**  
  _Nachteil_: Eingeschränkte Qualität und hoher Wartungsaufwand

- **Verwendung von Open-Source-Modellen lokal**  
  _Nachteil_: Komplexe Infrastruktur, hoher Ressourcenverbrauch

---

## Konsequenzen

- Abhängigkeit von einem externen Anbieter (OpenAI)
- Notwendigkeit zur Absicherung und Verwaltung von API-Schlüsseln
- Laufende Kosten abhängig vom Nutzungsvolumen
- Fehlerbehandlung und Ratenbegrenzung müssen berücksichtigt werden

---
# ADR 003: Verwendung von Dependency Injection (DI) für lose Kopplung der Services

**Status:** Akzeptiert

---

## Kontext

Die Anwendung verwendet mehrere Services (z. B. `FileService`, `CardDeckGenerationServiceOpenAI`, diverse `IFileReaderStrategy`-Implementierungen), die voneinander abhängig sind. Um die Wiederverwendbarkeit, Testbarkeit und Wartbarkeit dieser Komponenten zu verbessern, sollte eine flexible Möglichkeit zur Verwaltung von Abhängigkeiten gewählt werden.

---

## Entscheidung

Wir haben uns entschieden, das in .NET integrierte **Dependency Injection (DI)**-Framework zu verwenden, um Serviceklassen automatisch bereitzustellen und ihre Abhängigkeiten aufzulösen. Alle benötigten Services und Strategien werden im `Program.cs` registriert und zur Laufzeit injiziert.

---

## Gründe für diese Entscheidung

- **Lose Kopplung**: Komponenten kennen nur ihre Abstraktionen (Interfaces), nicht deren Implementierungen.
- **Testbarkeit**: Abhängigkeiten können in Tests einfach durch Mocks ersetzt werden.
- **Erweiterbarkeit**: Neue Strategien oder Services lassen sich leicht integrieren.
- **Standard in .NET Core**: Kein zusätzlicher externer DI-Container erforderlich.

---

## Alternativen

- **Manuelles Instanziieren von Abhängigkeiten im Code**  
  _Nachteil_: Schwer wartbar, schlechtere Testbarkeit, enge Kopplung der Komponenten.

- **Verwendung eines Drittanbieter-DI-Containers (z. B. Autofac)**  
  _Nachteil_: Höhere Komplexität ohne erkennbaren Mehrwert für dieses Projekt.

---

## Konsequenzen

- Notwendigkeit, alle Services korrekt im DI-Container zu registrieren
- Die Struktur der Klassen muss auf Konstruktorinjektion ausgelegt sein
- Bessere Trennung von Verantwortlichkeiten und klarere Codebasis

---
