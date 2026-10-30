# KSA Babel · 喵语塔

[English](../../README.md) · [简体中文](README.zh-CN.md) · [日本語](README.ja-JP.md) · [Русский](README.ru-RU.md) · [Deutsch](README.de-DE.md) · [Français](README.fr-FR.md) · [Italiano](README.it-IT.md) · [Español](README.es-ES.md) · [Português (Brasil)](README.pt-BR.md)

<p align="center"><img src="../branding/ksa-babel-cover-preview-v3.png" alt="KSA Babel · 喵语塔" width="512"></p>

**Kitten Space Agency** 向けの多言語ローカライズフレームワークです。StarMap を通じて読み込み、言語パック、ゲーム内の言語選択メニュー、表示を調整するアダプター、共通フォントのサブセットを提供します。ゲームの実行ファイルやリソースは置き換えません。

KSA Babel は [KSA-CHINESE](https://github.com/kuxiaowo/KSA-CHINESE) から独立した多言語プロジェクトです。現在のバージョンは **0.2.3 正式版**で、Windows x64 の **KSA 2026.10.25.5707** と、確認済みのファイルハッシュにのみ対応しています。ローダーは **StarMap 0.4.8** です。

**翻訳確認と英語モード：**作者本人が確認した訳文は簡体字中国語のみです。日本語、ロシア語、ドイツ語、フランス語、イタリア語、スペイン語、ブラジルポルトガル語は、KSP の宇宙航行用語を参考に GPT が KSA の文脈に合わせて翻訳・確認したもので、独立した母語話者の校閲は未実施です。日本語の初期訳の一部は、署名を保持した MIT プロジェクト KsaUiLanguages に由来します。英語モードでは KSA の英語原文を表示し、Mod が追加する `Language` メニューを残します。11 個の動的テンプレートは多言語表示と原文への復帰に使うもので、英語原文を 11 か所書き換えたという意味ではありません。

**中国語向けの版：**[KSA Chinese / KSA-CHINESE](https://github.com/kuxiaowo/KSA-CHINESE) は ID `KsaChinese` で、英語／簡体字中国語を切り替えます。現在の [v0.1.7](https://github.com/kuxiaowo/KSA-CHINESE/releases/tag/v0.1.7) の標準 ZIP は `KsaChinese-0.1.7.zip`。確認済みの Windows x64 KSA 2026.10.25.5707 と StarMap 0.4.8 に対応し、独自の [Borea 登録](https://github.com/KSAModding/content-index/blob/main/listings/KsaChinese.toml)があります。Babel は ID `KsaBabel` の独立した 9 言語 Modです。同じプロファイルで両方を有効にしないでください。

0.2.3 では KSA 5707 向けに265件を追加：宇宙天気とCME、HDR、乗員の被曝・回復・引退、遮蔽と圧力、更新履歴156件。今回の新訳は中国語を含めAIによる文脈確認済みですが、作者の個別確認や独立した母語校閲は未実施です。以前の作者確認は旧版中国語の範囲のみです。実機検証と制限は0.2.3の報告を参照してください。 [0.2.3](../../reports/0.2.3-5707适配与验证.md)

## 対応言語と対象範囲

English · 简体中文 · 日本語 · Русский · Deutsch · Français · Italiano · Español · Português (Brasil)

追加の 7 言語は、それぞれ既存の校閲済み中国語版の範囲に対応する **5,051 件のレコード**と、**11 個の動的テンプレート**を収録しています。英語ではゲームの原文を使用します。レコードが揃っていることは、**ゲーム全体の翻訳完了を意味しません**。新しい訳文には AI による文脈の確認を行いましたが、**独立した母語話者による承認は受けていません**。古い更新履歴、ネイティブライブラリやエラーの文言、テクスチャに描かれた文字、一部の UI 状態は校閲対象外です。幅の狭いコントロールや他の解像度についても、さらに表示確認が必要です。

今回の対象には、インターフェース、ヒント、部品、物質と相、計器、一部の更新履歴本文が含まれます。型番、単位、プレースホルダー、リソース／コントロール／セーブデータの ID、列挙型の値、プレイヤーが付けた名前は保持します。未知のバージョンでは安全のため動作を拒否し、訳文がない場合は原文に戻ります。Noto Sans SC のサブセットは確認済みの文字を収録していますが、任意のプレイヤー名や、日本語で使われるすべての地域字形を保証するものではありません。

## インストール

[KSA Babel Releases](https://github.com/kuxiaowo/KSA-Babel/releases) からダウンロードしてください。0.2.3 のパッケージは正式版です。

- **StarMap/Borea を導入済みの場合：** `KsaBabel-0.2.3.zip` を使用します。中の `KsaBabel` フォルダーを選択したプロファイルの `mods` ディレクトリに配置し、その Mod を有効にしてください。[通常のインストール手順](../../README-生态安装.md)を参照してください。
- **ランチャー同梱版：** `KsaBabel-0.2.3-5707-Portable.zip` を `KSA.exe` があるフォルダーに展開し、`KsaBabel/KsaBabelLauncher.exe` を実行してください。[ポータブル版のインストール手順](../../README-便携Mod.md)を参照してください。.NET 10 x64 Runtime が必要です。

0.2.1 から、Mod ID、インストール先ディレクトリ、アーカイブ名の接頭辞は **`KsaBabel`** です。プラグインのアセンブリは `KsaBabel.dll`、ポータブル版のランチャーは `KsaBabel/KsaBabelLauncher.exe` です。言語設定は現在のプロファイルの `KsaBabel/settings.json` に保存します。初回起動時、新しい設定ファイルが存在しない場合に限り、同じプロファイルにある旧 `KsaChinese/settings.json` から有効な `locale` を取り込みます。既存の新しい設定は上書きせず、旧ファイルも変更しません。

**先に旧 Mod を無効にしてください。** `KsaChinese` が有効な間、Babel は有効化を拒否します。Babel は 1 つのコピーだけを有効にし、`Content/KsaBabel` とプロファイル内の `mods/KsaBabel` を両方残さないでください。移行前に既存のプロファイルをバックアップしてください。個人のセーブデータは同梱せず、自動移動もしません。設定の取り込みで別のプロファイルを検索することもありません。

[Ahwoo の告知](https://forums.ahwoo.com/forums/kitten-space-agency/mod-releases/ksa-babel-0-2-1-nine-language-localization-preview-for-ksa-5638.1454/)を公開し、[Borea 登録 PR #227](https://github.com/KSAModding/content-index/pull/227)がマージされました。リリース情報の生成と公開スナップショットの確認状況は[公開記録](../../reports/community-publication-20261024.md)を参照してください。既存の KsaChinese 項目と v0.1.7 は、別の中国語向けプロジェクトに属します。 KSA Babel 0.2.3 は正式版で、既定の stable チャンネルから利用できます。追加の 7 言語は引き続き独立した母語話者の校閲を必要とします。

Borea で移行する場合はプロファイルをバックアップし、そのインスタンスから旧 KsaChinese を削除してから Babel をインストールしてください。カタログの競合判定は旧 Mod が無効でもインストールを拒否します。プロファイルのデータは保持してください。

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

生成済みのフォントは同梱しています。再生成には fontTools と、[FONT-SOURCE](../../assets/KsaBabel/FONT-SOURCE.txt) に記載された SHA-256 に一致する `.tools/NotoSansSC-variable.ttf` が必要です。フォントの原本を変更する場合は確認が必要です。StarMap のダウンロード元を特定する情報と第三者のライセンスは [THIRD_PARTY](../../THIRD_PARTY.md) に記録しています。

## 検証資料と貢献

- [9 言語の対象範囲と検証](../../reports/0.2.0-九语种翻译与验证.md)
- [プロジェクトの由来と取り込んだファイルのフィンガープリント](../../reports/project-origin.json)
- [移行と公開に関する記録](../../reports/KSA-Babel-项目迁移与公开验证.md)
- [0.2.1 ID と管理ツールの検証](../../reports/0.2.1-独立ID与管理器验证.md)

0.1.x のレポートは、元の KSA Chinese プロジェクトと過去のビルドについて記述したものです。出典を示す資料として保存しており、Babel が以前に公開またはカタログ登録されていたことの証拠ではありません。GUI の検証資料は、記録されたテストシーン、言語切り替え、設定／HUD のスクリーンショット、固定された数値表示の画像領域に限られ、網羅的な表示確認ではありません。

修正を提案する際は、対象言語、英語の原文、ゲーム内の文脈、提案する訳文を明記してください。母語話者による校閲を特に歓迎します。開発とコミットは **`developing → pull request → main`** の流れで行います。

## ライセンス

コードと本プロジェクトで作成した訳文は **MIT** ライセンスで、KSA Chinese と KsaUiLanguages の著作権表示を保持しています。フォントは **SIL OFL 1.1** です。ポータブルパッケージには、それぞれの MIT ライセンスに従って StarMap と Harmony を同梱しています。[LICENSE](../../LICENSE)、[LICENSES](../../LICENSES)、[THIRD_PARTY](../../THIRD_PARTY.md) を参照してください。

KSA と KSP のゲームアセット、元の辞書、逆コンパイルしたソース、個人データは配布しません。KSP はローカル環境で用語を参照するために使用したもので、その完全な辞書を製品の入力データとして使用したり、リリースに含めたりはしていません。
