# KSA Babel · 喵语塔

[English](../../README.md) · [简体中文](README.zh-CN.md) · [日本語](README.ja-JP.md) · [Русский](README.ru-RU.md) · [Deutsch](README.de-DE.md) · [Français](README.fr-FR.md) · [Italiano](README.it-IT.md) · [Español](README.es-ES.md) · [Português (Brasil)](README.pt-BR.md)

Ein Framework zur mehrsprachigen Lokalisierung von **Kitten Space Agency**, das über StarMap geladen wird. KSA Babel bietet Sprachpakete, eine Sprachauswahl im Spiel, Anpassungen der Textdarstellung und einen gemeinsamen reduzierten Schriftzeichensatz. Die ausführbare Datei und die Ressourcen des Spiels werden nicht ersetzt.

KSA Babel ist ein eigenständiges mehrsprachiges Projekt, das aus [KSA-CHINESE](https://github.com/kuxiaowo/KSA-CHINESE) hervorgegangen ist. Die aktuelle Version ist die **Vorschauversion 0.2.0**. Sie unterstützt ausschließlich **KSA 2026.10.19.5638** unter Windows x64 mit den geprüften Datei-Hashes. Als Loader wird **StarMap 0.4.8** verwendet.

## Sprachen und Umfang

English · 简体中文 · 日本語 · Русский · Deutsch · Français · Italiano · Español · Português (Brasil)

Die sieben zusätzlichen Sprachen enthalten jeweils **4.786 Einträge** aus dem bereits geprüften Umfang der chinesischen Übersetzung sowie **11 dynamische Vorlagen**. Für Englisch wird der Originaltext des Spiels verwendet. Ein vollständiger Satz dieser Einträge bedeutet **keine vollständige Übersetzung des gesamten Spiels**. Die neuen Übersetzungen wurden mithilfe von KI im Kontext geprüft, **aber nicht unabhängig von Muttersprachlern abgenommen**. Ältere Änderungsprotokolle, Texte nativer Bibliotheken und Fehlermeldungen, Schrift auf Texturen und einige Zustände der Benutzeroberfläche liegen weiterhin außerhalb des geprüften Umfangs. Schmale Bedienelemente und andere Auflösungen benötigen weitere Sichtprüfungen.

Der aktuelle Umfang umfasst die Benutzeroberfläche, Hinweise, Bauteile, Stoffe und ihre Phasen, Instrumente sowie einen Teil der Aktualisierungstexte. Modellbezeichnungen, Einheiten, Platzhalter, Ressourcen-, Bedien- und Spielstand-IDs, Enum-Werte und selbst vergebene Namen bleiben erhalten. Unbekannte Spielversionen werden aus Sicherheitsgründen abgelehnt; bei fehlender Übersetzung erscheint der Originaltext. Die Teilmenge von Noto Sans SC enthält die erfassten Zeichen, garantiert jedoch weder beliebige selbst vergebene Namen noch sämtliche regionalen japanischen Glyphenformen.

## Installation

Lade das Paket unter [KSA Babel Releases](https://github.com/kuxiaowo/KSA-Babel/releases) herunter. Die Pakete der Version 0.2.0 sind Vorschauversionen.

- **StarMap/Borea bereits vorhanden:** Verwende `KsaChinese-0.2.0.zip`. Lege den darin enthaltenen Ordner `KsaChinese` im Verzeichnis `mods` des gewählten Profils ab und aktiviere den Mod. Siehe [Standardinstallation](../../README-生态安装.md).
- **Paket mit Launcher:** Entpacke `KsaChinese-0.2.0-5638-Portable.zip` in den Ordner mit `KSA.exe` und starte anschließend `KsaChinese/KsaChineseLauncher.exe`. Siehe [Installation des portablen Pakets](../../README-便携Mod.md). Benötigt wird die .NET 10 x64 Runtime.

Aus Kompatibilitätsgründen bleiben die interne Mod-ID, der Assemblyname, das Einstellungsverzeichnis und das Archivpräfix bei **`KsaChinese`**. **Aktiviere nur eine Kopie:** Installiere KSA Babel nicht gleichzeitig mit KSA Chinese und behalte nicht gleichzeitig `Content/KsaChinese` und `mods/KsaChinese` im Profil. Sichere vorhandene Profile vor dem Umstieg. Persönliche Spielstände sind nicht enthalten und werden nicht automatisch verschoben.

Für das neue Projekt wurde noch kein eigener Eintrag im öffentlichen Borea-Katalog beantragt. Der KsaChinese-Eintrag im alten Katalog und die Veröffentlichung v0.1.6 gehören zum ursprünglichen Projekt und sind keine Downloadquelle für Babel. Installiere manuell aus den Releases dieses Repositorys. Das Projekt hat keine Nachrichten an das Forum oder die Verantwortlichen des Index gesendet.

## Bauen und prüfen

Verwende das .NET 10 SDK, eine rechtmäßig installierte Kopie der exakt unterstützten Spielversion und [StarMap 0.4.8](https://github.com/StarMapLoader/StarMap/releases/tag/0.4.8). Lege die Loaderdateien unter `.tools/StarMap-0.4.8` ab oder übergib ihren Pfad mit `-LoaderDir`. Die Skripte benötigen Python 3; bevorzugt wird conda.

```powershell
python scripts/build_multilingual.py --check
.\scripts\build.ps1 -GameDir 'D:\Games\Kitten Space Agency' -Validate
python scripts/package_mod.py
python scripts/package_portable.py
```

Statt mit `-GameDir` lässt sich der lokale Spielpfad auch über `KSA_GAME_DIR` festlegen. Spielbinärdateien, `.tools`, lokale Profile und Buildausgaben werden von Git ignoriert. Ein frischer Klon enthält weder das Spiel noch den Loader, den Decompiler, die Eingabedaten für Audits oder KSP-Referenzwörterbücher. Optionale historische Auditskripte benötigen separat vorbereitete lokale Eingabedaten.

Die Übersetzungsentscheidungen stehen in `translations/multilingual/<locale>/*.jsonl`. `build_multilingual.py` prüft IDs, Herkunft, Prüfstatus, Platzhalter und Markup anhand der geprüften Ausgangsdaten; unvollständige oder veraltete Pakete werden abgelehnt. Auch das Erstellen eines Veröffentlichungspakets setzt eine passende, erfolgreiche lokale Buildprüfung und seither unveränderte Quellen und Ressourcen voraus. Die CI prüft Daten und Schriften ohne installierte Spielkopie. Sie kann weder die Kompilierung mit dem lizenzierten Spiel noch eine Abnahme im Spiel durchführen.

Die erzeugte Schrift ist bereits enthalten. Zum erneuten Erstellen werden fontTools und `.tools/NotoSansSC-variable.ttf` benötigt. Dessen SHA-256 muss mit der Angabe in [FONT-SOURCE](../../assets/KsaChinese/FONT-SOURCE.txt) übereinstimmen. Änderungen an der Ausgangsschrift müssen geprüft werden. Angaben zur eindeutigen Identifizierung des StarMap-Downloads und die Lizenzen der Drittanbieter stehen in [THIRD_PARTY](../../THIRD_PARTY.md).

## Nachweise und Mitarbeit

- [Umfang und Prüfung der neun Sprachen](../../reports/0.2.0-九语种翻译与验证.md)
- [Projektherkunft und Fingerabdrücke importierter Dateien](../../reports/project-origin.json)
- [Hinweise zur Projektmigration und Veröffentlichung](../../reports/KSA-Babel-项目迁移与公开验证.md)

Die Berichte zu 0.1.x beschreiben das ursprüngliche Projekt KSA Chinese und dessen frühere Builds. Sie bleiben als Herkunftsnachweise erhalten und belegen nicht, dass Babel bereits früher veröffentlicht oder in einen Katalog aufgenommen wurde. Die GUI-Nachweise beschränken sich auf die dokumentierte Testszene, Sprachwechsel, Screenshots der Einstellungen und des HUD sowie festgelegte Bildbereiche mit Zahlenanzeigen. Sie sind keine umfassende visuelle Abnahme.

Gib bei Übersetzungsvorschlägen die Sprache, den englischen Ausgangstext, den Kontext im Spiel und die vorgeschlagene Formulierung an. Eine Prüfung durch Muttersprachler ist besonders hilfreich. Entwicklung und Commits folgen dem Ablauf **`developing → pull request → main`**.

## Lizenz

Code und selbst verfasste Übersetzungen: **MIT**, unter Beibehaltung der Urheberrechtshinweise von KSA Chinese und KsaUiLanguages. Schrift: **SIL OFL 1.1**. Das portable Paket enthält StarMap und Harmony unter deren MIT-Lizenzen. Siehe [LICENSE](../../LICENSE), [LICENSES](../../LICENSES) und [THIRD_PARTY](../../THIRD_PARTY.md).

Spielinhalte von KSA und KSP, Originalwörterbücher, dekompilierte Quellen und persönliche Daten werden nicht weitergegeben. KSP diente lokal als Terminologiereferenz; sein vollständiges Wörterbuch ist weder Eingabedatensatz des Produkts noch Bestandteil der Veröffentlichung.
