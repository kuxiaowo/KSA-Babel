# KSA Babel · 喵语塔

[English](README.md) · [简体中文](docs/readme/README.zh-CN.md) · [日本語](docs/readme/README.ja-JP.md) · [Русский](docs/readme/README.ru-RU.md) · [Deutsch](docs/readme/README.de-DE.md) · [Français](docs/readme/README.fr-FR.md) · [Italiano](docs/readme/README.it-IT.md) · [Español](docs/readme/README.es-ES.md) · [Português (Brasil)](docs/readme/README.pt-BR.md)

<p align="center"><img src="docs/branding/ksa-babel-cover-preview-v3.png" alt="KSA Babel · 喵语塔" width="512"></p>

A multilingual localization framework for **Kitten Space Agency**, loaded through StarMap. KSA Babel provides language packs, an in-game language menu, display adapters and a shared font subset. It does not replace the game executable or game resources.

KSA Babel is an independent multilingual project derived from [KSA-CHINESE](https://github.com/kuxiaowo/KSA-CHINESE). The current version is the **0.2.3 stable release**, supporting only **KSA 2026.10.25.5707 on Windows x64** with the inspected file hashes and **StarMap 0.4.8**.

**Translation review and English mode:** The maintainer has personally confirmed only the Simplified Chinese translation. The Japanese, Russian, German, French, Italian, Spanish and Brazilian Portuguese packs were translated/reviewed by GPT in the KSA context, using KSP aerospace terminology as reference; they await independent native-speaker review. Some initial Japanese translations came from the credited MIT-licensed KsaUiLanguages project. English mode displays KSA's original English text and retains the mod's added `Language` menu. The 11 dynamic entries support multilingual text composition and original-English fallback; they do not mean 11 rewritten game texts.

**Chinese-focused edition:** [KSA Chinese / KSA-CHINESE](https://github.com/kuxiaowo/KSA-CHINESE), mod ID `KsaChinese`, offers English / Simplified Chinese switching. Its current release is [v0.1.7](https://github.com/kuxiaowo/KSA-CHINESE/releases/tag/v0.1.7), with standard archive `KsaChinese-0.1.7.zip`; it targets the inspected Windows x64 KSA 2026.10.25.5707 build with StarMap 0.4.8. It has its own [Borea catalog entry](https://github.com/KSAModding/content-index/blob/main/listings/KsaChinese.toml). Babel is the independent nine-language mod with ID `KsaBabel`. Choose the appropriate edition and do not enable both in the same profile.

0.2.3 adds 265 records for KSA 5707: space weather and CME, HDR, crew radiation/recovery/retirement, shielding and pressure, and 156 changelog entries. All new translations, including Chinese, have AI context review; they have not received maintainer or independent native-speaker sign-off. Earlier maintainer confirmation applies only to the older Chinese scope. See the 0.2.3 validation report for bounded runtime evidence. [0.2.3](reports/0.2.3-5707适配与验证.md)

## Languages and scope

| Language | Locale | Game text |
| --- | --- | --- |
| English | `en-US` | Original game text; added language menu |
| 简体中文 | `zh-CN` | 5,051 records + 11 dynamic templates |
| 日本語 | `ja-JP` | 5,051 records + 11 dynamic templates |
| Русский | `ru-RU` | 5,051 records + 11 dynamic templates |
| Deutsch | `de-DE` | 5,051 records + 11 dynamic templates |
| Français | `fr-FR` | 5,051 records + 11 dynamic templates |
| Italiano | `it-IT` | 5,051 records + 11 dynamic templates |
| Español | `es-ES` | 5,051 records + 11 dynamic templates |
| Português (Brasil) | `pt-BR` | 5,051 records + 11 dynamic templates |

Seven additional languages each cover **5,051 records** from the existing reviewed Chinese scope, plus **11 dynamic templates**. English uses the game's original text. Record completeness is **not full-game coverage**. New translations have AI context review, **not independent native-speaker sign-off**. Older changelogs, native library/error text, texture lettering and some UI states remain outside the reviewed scope. Narrow controls and other resolutions still need broader visual review.

The reviewed scope includes UI, tooltips, parts, substances and phases, gauges and selected changelogs. Model numbers, units, placeholders, resource/widget/save IDs, enum data and player-defined names are preserved. Unknown builds are rejected; untranslated text falls back to the original. The Noto Sans SC subset covers the inventoried characters, but does not guarantee arbitrary player names or every Japanese regional glyph form.

## Install

Download from [KSA Babel Releases](https://github.com/kuxiaowo/KSA-Babel/releases). The 0.2.3 packages are stable releases.

- **Existing StarMap/Borea:** use `KsaBabel-0.2.3.zip`; put its `KsaBabel` folder in the selected profile's `mods` directory and enable that mod. See [standard installation](README-生态安装.md).
- **Bundled launcher:** extract `KsaBabel-0.2.3-5707-Portable.zip` into the folder containing `KSA.exe`, then run `KsaBabel/KsaBabelLauncher.exe`. See [portable installation](README-便携Mod.md). Requires .NET 10 x64 Runtime.

From 0.2.1, the mod ID, installation directories and archive prefix are **`KsaBabel`**; the plugin assembly is `KsaBabel.dll` and the portable launcher is `KsaBabel/KsaBabelLauncher.exe`. Language settings are stored in the current profile's `KsaBabel/settings.json`. On first startup, only if that new file does not exist, Babel imports a valid `locale` from the same profile's old `KsaChinese/settings.json`. Existing new settings are never overwritten and the old file is not changed.

**Disable the old mod first:** Babel refuses activation while `KsaChinese` is enabled. Keep only one enabled Babel copy; do not keep both `Content/KsaBabel` and profile `mods/KsaBabel`. Back up existing profiles before migrating. Personal saves are not included or automatically moved; settings import never searches another profile.

The [Ahwoo announcement](https://forums.ahwoo.com/forums/kitten-space-agency/mod-releases/ksa-babel-0-2-1-nine-language-localization-preview-for-ksa-5638.1454/) is published, and [Borea index PR #227](https://github.com/KSAModding/content-index/pull/227) has been merged. See the [publication record](reports/community-publication-20261024.md) for release stamping and public-snapshot verification. The existing KsaChinese catalog entry and v0.1.7 release belong to the separate Chinese-focused project. KSA Babel 0.2.3 is a stable release, available through the default stable channel. Seven additional translations still await independent native-speaker review.

For migration managed by Borea, back up the profile and remove the old KsaChinese mod from that instance before installing Babel: the catalog conflict blocks installation even when the old mod is disabled. Removing the mod must preserve profile data.

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

The generated font is already included. Rebuilding it requires fontTools and `.tools/NotoSansSC-variable.ttf` matching the SHA-256 in [FONT-SOURCE](assets/KsaBabel/FONT-SOURCE.txt); source changes require review. The StarMap download identity and third-party licenses are recorded in [THIRD_PARTY](THIRD_PARTY.md).

## Evidence and contribution

- [Stable release 0.2.2 and validation](reports/0.2.2-正式发布与验证.md)
- [Nine-language scope and verification](reports/0.2.0-九语种翻译与验证.md)
- [Project origin and imported-file fingerprints](reports/project-origin.json)
- [Migration and publication notes](reports/KSA-Babel-项目迁移与公开验证.md)
- [Independent ID and manager validation (0.2.1)](reports/0.2.1-独立ID与管理器验证.md)

Reports for 0.1.x describe the original KSA Chinese project and historical builds. They are retained as provenance, not evidence that Babel was previously published or indexed. GUI evidence is bounded to the recorded test scene, locale switches, settings/HUD screenshots and fixed numeric image regions; it is not exhaustive visual acceptance.

Contributions should identify the locale, English source, game context and suggested wording. Native-speaker review is especially useful. Development and commits use **`developing → pull request → main`**.

## License

Code and authored translations: **MIT**, preserving KSA Chinese and KsaUiLanguages copyright notices. Font: **SIL OFL 1.1**. The portable package includes StarMap and Harmony under their MIT licenses. See [LICENSE](LICENSE), [LICENSES](LICENSES) and [THIRD_PARTY](THIRD_PARTY.md).

KSA and KSP game assets, original dictionaries, decompiled sources and personal data are not distributed. KSP was a local terminology reference; its complete dictionary is not a product input or part of the release.
