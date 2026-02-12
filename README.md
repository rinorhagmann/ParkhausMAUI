# Parkhaus MAUI App

## Features

* **Parkhaus-Übersicht:** Auflistung von 17 Basler Parkhäusern mit Preis pro Stunde.
* **Kapazitätsanzeige:** Anzeige von verfügbaren Plätzen. Besetzte Parkhäuser werden markiert und gesperrt.
* **Echtzeit-Timer:** Berechnung der Parkdauer und der anfallenden Kosten.
* **Bezahlen:** Es wird überprüft, ob die Zahlung des Preises getätigt wurde, wenn erfolgreich wird die "Ausfahrt" (Beenden) ermöglicht.
* **Verlauf:** Übersicht aller vergangenen Parkvorgänge inkl. Löschfunktion.
* **Datenhaltung:** Speicherung aller Daten (Parkvorgänge, Verlauf, Kapazitäten) in einer lokalen JSON-Datei.

---

## Architektur und eingesetzte Technologien

* **Framework:** .NET MAUI (.NET 10.0)
* **Pattern:** MVVM mit dem Nuget-Paket `CommunityToolkit.Mvvm`
* **Datenformat:** JSON

---

## Projektstruktur

* **Models/:** Datenstrukturen für `ParkingLocation`, `ParkingSession` und `RootData`.
* **ViewModels/:** Logik für die Hauptseite, den aktiven Timer und den Verlauf.
* **Views/:** XAML-Seiten für die Benutzeroberfläche.
* **Services/:** JSON-Datenhaltung und Berechnung.

---

## Nutzungshinweise

* Das Projekt muss entweder über den Android Emulator oder die Windows Machine gestartet werden
* Beim ersten Start wird automatisch eine Datei namens `parkhaeuser_basel.json` im lokalen Anwendungsordner erstellt.
* Um einen neuen Parkvorgang zu starten, darf aktuell kein anderer Parkvorgang aktiv sein.
* Ein Parkhaus kann nur gewählt werden, wenn mindestens ein Platz frei ist.
* Man kann den Parkvorgang nicht beenden, solange man nicht bezahlt hat.

---
© RINOR HAGMANN - 2026
