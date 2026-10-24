# KSA Babel · 喵语塔

[English](../../README.md) · [简体中文](README.zh-CN.md) · [日本語](README.ja-JP.md) · [Русский](README.ru-RU.md) · [Deutsch](README.de-DE.md) · [Français](README.fr-FR.md) · [Italiano](README.it-IT.md) · [Español](README.es-ES.md) · [Português (Brasil)](README.pt-BR.md)

**Kitten Space Agency** 向けの多言語ローカライズフレームワークです。StarMap を通じて読み込み、言語パック、ゲーム内の言語選択メニュー、表示を調整するアダプター、共通フォントのサブセットを提供します。ゲームの実行ファイルやリソースは置き換えません。

KSA Babel は [KSA-CHINESE](https://github.com/kuxiaowo/KSA-CHINESE) から独立した多言語プロジェクトです。現在のバージョンは **0.2.0 プレビュー版**で、Windows x64 の **KSA 2026.10.19.5638** と、確認済みのファイルハッシュにのみ対応しています。ローダーは **StarMap 0.4.8** です。

## 対応言語と対象範囲

English · 简体中文 · 日本語 · Русский · Deutsch · Français · Italiano · Español · Português (Brasil)

追加の 7 言語は、それぞれ既存の校閲済み中国語版の範囲に対応する **4,786 件のレコード**と、**11 個の動的テンプレート**を収録しています。英語ではゲームの原文を使用します。レコードが揃っていることは、**ゲーム全体の翻訳完了を意味しません**。新しい訳文には AI による文脈の確認を行いましたが、**独立した母語話者による承認は受けていません**。古い更新履歴、ネイティブライブラリやエラーの文言、テクスチャに描かれた文字、一部の UI 状態は校閲対象外です。幅の狭いコントロールや他の解像度についても、さらに表示確認が必要です。

今回の対象には、インターフェース、ヒント、部品、物質と相、計器、一部の更新履歴本文が含まれます。型番、単位、プレースホルダー、リソース／コントロール／セーブデータの ID、列挙型の値、プレイヤーが付けた名前は保持します。未知のバージョンでは安全のため動作を拒否し、訳文がない場合は原文に戻ります。Noto Sans SC のサブセットは確認済みの文字を収録していますが、任意のプレイヤー名や、日本語で使われるすべての地域字形を保証するものではありません。

## インストール

[KSA Babel Releases](https://github.com/kuxiaowo/KSA-Babel/releases) からダウンロードしてください。0.2.0 のパッケージはプレビュー版です。

- **StarMap/Borea を導入済みの場合：** `KsaChinese-0.2.0.zip` を使用します。中の `KsaChinese` フォルダーを選択したプロファイルの `mods` ディレクトリに配置し、その Mod を有効にしてください。[通常のインストール手順](../../README-生态安装.md)を参照してください。
- **ランチャー同梱版：** `KsaChinese-0.2.0-5638-Portable.zip` を `KSA.exe` があるフォルダーに展開し、`KsaChinese/KsaChineseLauncher.exe` を実行してください。[ポータブル版のインストール手順](../../README-便携Mod.md)を参照してください。.NET 10 x64 Runtime が必要です。

互換性のため、内部の Mod ID、アセンブリ、設定ディレクトリ、アーカイブ名の接頭辞には **`KsaChinese`** を引き続き使用します。**有効にするのは 1 つだけにしてください。** KSA Babel と KSA Chinese を併用したり、`Content/KsaChinese` とプロファイル内の `mods/KsaChinese` を両方残したりしないでください。移行前に既存のプロファイルをバックアップしてください。個人のセーブデータは同梱せず、自動移動も行いません。

この新プロジェクトについて、Borea の公開カタログへの新規登録はまだ申請していません。旧カタログの KsaChinese 項目と v0.1.6 リリースは元のプロジェクトに属し、Babel のダウンロード先としては使用できません。このリポジトリの Release から手動でインストールしてください。本プロジェクトからフォーラムやインデックスの管理者へのメッセージ送信は行っていません。

## ビルドと検証

.NET 10 SDK、対応バージョンと完全に一致する正規にインストールされたゲーム、および [StarMap 0.4.8](https://github.com/StarMapLoader/StarMap/releases/tag/0.4.8) を使用してください。ローダーのファイルは `.tools/StarMap-0.4.8` に配置するか、`-LoaderDir` で場所を指定します。Python スクリプトには Python 3 が必要です。conda の使用を推奨します。

```powershell
python scripts/build_multilingual.py --check
.\scripts\build.ps1 -GameDir 'D:\Games\Kitten Space Agency' -Validate
python scripts/package_mod.py
python scripts/package_portable.py
```

`-GameDir` の代わりに、環境変数 `KSA_GAME_DIR` でローカルのゲームパスを指定することもできます。ゲームのバイナリ、`.tools`、ローカルのプロファイル、ビルド出力は Git の管理対象外です。新しくクローンしたリポジトリには、ゲーム、ローダー、逆コンパイラー、監査用の入力ファイル、KSP の参照辞書は含まれません。任意で実行する過去の監査スクリプトには、別途用意したローカルの入力ファイルが必要です。

翻訳の判断記録は `translations/multilingual/<locale>/*.jsonl` にあります。`build_multilingual.py` は、校閲済みの基準データに照らして ID、出典、校閲状態、プレースホルダー、マークアップを検査し、不完全なパックや古いパックを拒否します。パッケージの作成にも、対象と一致するローカルビルドの検証成功と、ソースおよびアセットがその後変更されていないことが必要です。CI はゲームのない環境でも実施できるデータとフォントの検査を行いますが、ライセンス付きゲームを必要とするコンパイルや、ゲームプレイの受け入れ確認は実施できません。

生成済みのフォントは同梱しています。再生成には fontTools と、[FONT-SOURCE](../../assets/KsaChinese/FONT-SOURCE.txt) に記載された SHA-256 に一致する `.tools/NotoSansSC-variable.ttf` が必要です。フォントの原本を変更する場合は確認が必要です。StarMap のダウンロード元を特定する情報と第三者のライセンスは [THIRD_PARTY](../../THIRD_PARTY.md) に記録しています。

## 検証資料と貢献

- [9 言語の対象範囲と検証](../../reports/0.2.0-九语种翻译与验证.md)
- [プロジェクトの由来と取り込んだファイルのフィンガープリント](../../reports/project-origin.json)
- [移行と公開に関する記録](../../reports/KSA-Babel-项目迁移与公开验证.md)

0.1.x のレポートは、元の KSA Chinese プロジェクトと過去のビルドについて記述したものです。出典を示す資料として保存しており、Babel が以前に公開またはカタログ登録されていたことの証拠ではありません。GUI の検証資料は、記録されたテストシーン、言語切り替え、設定／HUD のスクリーンショット、固定された数値表示の画像領域に限られ、網羅的な表示確認ではありません。

修正を提案する際は、対象言語、英語の原文、ゲーム内の文脈、提案する訳文を明記してください。母語話者による校閲を特に歓迎します。開発とコミットは **`developing → pull request → main`** の流れで行います。

## ライセンス

コードと本プロジェクトで作成した訳文は **MIT** ライセンスで、KSA Chinese と KsaUiLanguages の著作権表示を保持しています。フォントは **SIL OFL 1.1** です。ポータブルパッケージには、それぞれの MIT ライセンスに従って StarMap と Harmony を同梱しています。[LICENSE](../../LICENSE)、[LICENSES](../../LICENSES)、[THIRD_PARTY](../../THIRD_PARTY.md) を参照してください。

KSA と KSP のゲームアセット、元の辞書、逆コンパイルしたソース、個人データは配布しません。KSP はローカル環境で用語を参照するために使用したもので、その完全な辞書を製品の入力データとして使用したり、リリースに含めたりはしていません。
