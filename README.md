# KSA Babel · 喵语塔

[English](README.md) · [简体中文](docs/readme/README.zh-CN.md) · [日本語](docs/readme/README.ja-JP.md) · [Русский](docs/readme/README.ru-RU.md) · [Deutsch](docs/readme/README.de-DE.md) · [Français](docs/readme/README.fr-FR.md) · [Italiano](docs/readme/README.it-IT.md) · [Español](docs/readme/README.es-ES.md) · [Português (Brasil)](docs/readme/README.pt-BR.md)

A multilingual localization framework for **Kitten Space Agency**, loaded through StarMap. KSA Babel provides language packs, an in-game language menu, display adapters and a shared font subset. It does not replace the game executable or game resources.

KSA Babel is an independent multilingual project derived from [KSA-CHINESE](https://github.com/kuxiaowo/KSA-CHINESE). The current version is the **0.2.0 preview**, supporting only **KSA 2026.10.19.5638 on Windows x64** with the inspected file hashes and **StarMap 0.4.8**.

## Languages and scope

| Language | Locale | Game text | README |
| --- | --- | --- | --- |
| English | `en-US` | Original game text + 11 dynamic templates | [English](README.md) |
| 简体中文 | `zh-CN` | 4,786 records + 11 dynamic templates | [简体中文](docs/readme/README.zh-CN.md) |
| 日本語 | `ja-JP` | 4,786 records + 11 dynamic templates | [日本語](docs/readme/README.ja-JP.md) |
| Русский | `ru-RU` | 4,786 records + 11 dynamic templates | [Русский](docs/readme/README.ru-RU.md) |
| Deutsch | `de-DE` | 4,786 records + 11 dynamic templates | [Deutsch](docs/readme/README.de-DE.md) |
| Français | `fr-FR` | 4,786 records + 11 dynamic templates | [Français](docs/readme/README.fr-FR.md) |
| Italiano | `it-IT` | 4,786 records + 11 dynamic templates | [Italiano](docs/readme/README.it-IT.md) |
| Español | `es-ES` | 4,786 records + 11 dynamic templates | [Español](docs/readme/README.es-ES.md) |
| Português (Brasil) | `pt-BR` | 4,786 records + 11 dynamic templates | [Português (Brasil)](docs/readme/README.pt-BR.md) |

Seven additional languages each cover **4,786 records** from the existing reviewed Chinese scope, plus **11 dynamic templates**. English uses the game's original text. Record completeness is **not full-game coverage**. New translations have AI context review, **not independent native-speaker sign-off**. Older changelogs, native library/error text, texture lettering and some UI states remain outside the reviewed scope. Narrow controls and other resolutions still need broader visual review.

The reviewed scope includes UI, tooltips, parts, substances and phases, gauges and selected changelogs. Model numbers, units, placeholders, resource/widget/save IDs, enum data and player-defined names are preserved. Unknown builds are rejected; untranslated text falls back to the original. The Noto Sans SC subset covers the inventoried characters, but does not guarantee arbitrary player names or every Japanese regional glyph form.

## Install

Download from [KSA Babel Releases](https://github.com/kuxiaowo/KSA-Babel/releases). The 0.2.0 packages are previews.

- **Existing StarMap/Borea:** use `KsaChinese-0.2.0.zip`; put its `KsaChinese` folder in the selected profile's `mods` directory and enable that mod. See [standard installation](README-生态安装.md).
- **Bundled launcher:** extract `KsaChinese-0.2.0-5638-Portable.zip` into the folder containing `KSA.exe`, then run `KsaChinese/KsaChineseLauncher.exe`. See [portable installation](README-便携Mod.md). Requires .NET 10 x64 Runtime.

The internal mod ID, assembly, settings directory and archive prefix remain **`KsaChinese`** for compatibility. **Enable only one copy:** do not install KSA Babel alongside KSA Chinese or keep both `Content/KsaChinese` and profile `mods/KsaChinese`. Back up existing profiles before migrating. No personal saves are included or automatically moved.

Babel has not been separately submitted to Borea's public catalog. The existing KsaChinese entry and v0.1.6 release belong to the original project; use this repository's Releases for manual installation. No messages have been sent to forum or index maintainers on behalf of this project.

## Build and validate

Use .NET 10 SDK, a legally installed copy of the exact supported game, and [StarMap 0.4.8](https://github.com/StarMapLoader/StarMap/releases/tag/0.4.8). Place loader files under `.tools/StarMap-0.4.8`, or pass `-LoaderDir`. Python scripts require Python 3; conda is preferred.

```powershell
python scripts/build_multilingual.py --check
.\scripts\build.ps1 -GameDir 'D:\Games\Kitten Space Agency' -Validate
python scripts/package_mod.py
python scripts/package_portable.py
```

`KSA_GAME_DIR` can supply the local game path instead of `-GameDir`. Game binaries, `.tools`, local profiles and build output are ignored. A fresh clone does not contain the game, loader, decompiler, audit inputs or KSP reference dictionaries. Optional historical audit scripts require their separately prepared local inputs.

Language decisions are in `translations/multilingual/<locale>/*.jsonl`. `build_multilingual.py` checks IDs, provenance, review status, placeholders and markup against the reviewed baseline; incomplete or stale packs are rejected. Packaging also requires matching successful local build validation and unchanged source/assets. CI verifies portable data and font checks; it cannot perform licensed-game compilation or gameplay acceptance.

The generated font is already included. Rebuilding it requires fontTools and `.tools/NotoSansSC-variable.ttf` matching the SHA-256 in [FONT-SOURCE](assets/KsaChinese/FONT-SOURCE.txt); source changes require review. The StarMap download identity and third-party licenses are recorded in [THIRD_PARTY](THIRD_PARTY.md).

## Evidence and contribution

- [Nine-language scope and verification](reports/0.2.0-九语种翻译与验证.md)
- [Project origin and imported-file fingerprints](reports/project-origin.json)
- [Migration and publication notes](reports/KSA-Babel-项目迁移与公开验证.md)

Reports for 0.1.x describe the original KSA Chinese project and historical builds. They are retained as provenance, not evidence that Babel was previously published or indexed. GUI evidence is bounded to the recorded test scene, locale switches, settings/HUD screenshots and fixed numeric image regions; it is not exhaustive visual acceptance.

Contributions should identify the locale, English source, game context and suggested wording. Native-speaker review is especially useful. Development and commits use **`developing → pull request → main`**.

## License

Code and authored translations: **MIT**, preserving KSA Chinese and KsaUiLanguages copyright notices. Font: **SIL OFL 1.1**. The portable package includes StarMap and Harmony under their MIT licenses. See [LICENSE](LICENSE), [LICENSES](LICENSES) and [THIRD_PARTY](THIRD_PARTY.md).

KSA and KSP game assets, original dictionaries, decompiled sources and personal data are not distributed. KSP was a local terminology reference; its complete dictionary is not a product input or part of the release.
