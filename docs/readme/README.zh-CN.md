# KSA Babel · 喵语塔

[English](../../README.md) · [简体中文](README.zh-CN.md) · [日本語](README.ja-JP.md) · [Русский](README.ru-RU.md) · [Deutsch](README.de-DE.md) · [Français](README.fr-FR.md) · [Italiano](README.it-IT.md) · [Español](README.es-ES.md) · [Português (Brasil)](README.pt-BR.md)

<p align="center"><img src="../branding/ksa-babel-cover-preview-v3.png" alt="KSA Babel · 喵语塔" width="512"></p>

通过 StarMap 加载的 **Kitten Space Agency 多语言本地化框架**。KSA Babel 提供语言包、游戏内语言菜单、显示适配与共享字体子集，不替换游戏可执行文件或原始资源。

本项目从 [KSA-CHINESE](https://github.com/kuxiaowo/KSA-CHINESE) 独立整理。当前为 **0.2.3 正式版**，仅适配 Windows x64 的 **KSA 2026.10.25.5707**、已检查的文件哈希与 **StarMap 0.4.8**。

**翻译确认与英文模式：**维护者本人仅确认了简体中文译文。日语、俄语、德语、法语、意大利语、西班牙语和巴西葡萄牙语由 GPT 结合 KSA 语境、参考 KSP 航天专业术语翻译与复核，尚无独立母语审校；日语部分初始译文来自已署名的 MIT 项目 KsaUiLanguages。英文模式显示 KSA 原有英文文本，并保留模组新增的 `Language` 语言菜单。11 个动态模板用于多语言文本拼接和原文回退，不表示改写了 11 处游戏英文。

**只需要中文时：**可使用 [KSA Chinese / KSA-CHINESE](https://github.com/kuxiaowo/KSA-CHINESE)，模组 ID 为 `KsaChinese`，支持 English／简体中文切换。当前发布为 [v0.1.7](https://github.com/kuxiaowo/KSA-CHINESE/releases/tag/v0.1.7)，标准包为 `KsaChinese-0.1.7.zip`，适配已检查的 Windows x64 KSA 2026.10.25.5707 与 StarMap 0.4.8，并有独立的 [Borea 目录条目](https://github.com/KSAModding/content-index/blob/main/listings/KsaChinese.toml)。Babel 是 ID 为 `KsaBabel` 的独立九语言项目；按需求选择，同一档案不要同时启用两者。

0.2.3 为 KSA 5707 新增 265 条记录：空间天气与 CME、HDR、乘员辐射/恢复/退役、屏蔽与压力，以及 156 条更新正文。本轮新增译文（含中文）经过 AI 语境复核，尚未经维护者逐条确认或独立母语审校；此前维护者确认只适用于旧版中文范围。运行验证及限制见 0.2.3 报告。 [0.2.3](../../reports/0.2.3-5707适配与验证.md)

## 支持语言与范围

| 语言 | 语言代码 | 游戏文本 |
| --- | --- | --- |
| English | `en-US` | 游戏原有英文；新增语言菜单 |
| 简体中文 | `zh-CN` | 5,051 条记录 + 11 个动态模板 |
| 日本語 | `ja-JP` | 5,051 条记录 + 11 个动态模板 |
| Русский | `ru-RU` | 5,051 条记录 + 11 个动态模板 |
| Deutsch | `de-DE` | 5,051 条记录 + 11 个动态模板 |
| Français | `fr-FR` | 5,051 条记录 + 11 个动态模板 |
| Italiano | `it-IT` | 5,051 条记录 + 11 个动态模板 |
| Español | `es-ES` | 5,051 条记录 + 11 个动态模板 |
| Português (Brasil) | `pt-BR` | 5,051 条记录 + 11 个动态模板 |

七种新增语言各覆盖现有已审中文底稿中的 **5,051 条记录**，另有 **11 个动态模板**。英文使用游戏原文。条目齐全**不等于全游戏覆盖**。新增译文经过 AI 语境复核，**尚无独立母语审校**。较早更新正文、原生库及错误文本、贴图印字和部分界面状态仍在已审范围之外；窄控件与其他分辨率还需要更多视觉检查。

范围包括界面、提示、部件、物质与相态、仪表及部分更新正文。型号、单位、占位符、资源／控件／存档 ID、枚举数据和玩家自定义名称保留；未知版本安全拒绝，缺译文回退原文。Noto Sans SC 子集覆盖已盘点字符，但不保证任意玩家命名或所有日语地区字形。

## 安装

从 [KSA Babel Releases](https://github.com/kuxiaowo/KSA-Babel/releases) 下载。0.2.3 安装包为正式版。

- **已有 StarMap／Borea：** 使用 `KsaBabel-0.2.3.zip`，把其中的 `KsaBabel` 文件夹放入所选档案的 `mods` 目录，并启用该模组。详见[标准安装](../../README-生态安装.md)。
- **随包启动器：** 将 `KsaBabel-0.2.3-5707-Portable.zip` 解压到含 `KSA.exe` 的游戏目录，运行 `KsaBabel/KsaBabelLauncher.exe`。详见[便携安装](../../README-便携Mod.md)。需要 .NET 10 x64 Runtime。

从 0.2.1 起，模组 ID、安装目录和安装包前缀为 **`KsaBabel`**，插件程序集为 `KsaBabel.dll`，便携启动器为 `KsaBabel/KsaBabelLauncher.exe`。语言设置保存在当前档案的 `KsaBabel/settings.json`。首启时，仅在新设置文件不存在的情况下，才从同一档案旧 `KsaChinese/settings.json` 导入有效的 `locale`；不覆盖已有新设置，也不修改旧文件。

**先禁用旧模组：** `KsaChinese` 仍启用时，Babel 拒绝激活。Babel 只保留一个启用副本，不要同时留下 `Content/KsaBabel` 与档案 `mods/KsaBabel`。迁移前备份原有档案；安装包不含个人存档，也不会自动搬动存档，设置导入不会搜索其他档案。

[Ahwoo 帖子](https://forums.ahwoo.com/forums/kitten-space-agency/mod-releases/ksa-babel-0-2-1-nine-language-localization-preview-for-ksa-5638.1454/)已发布，[Borea 收录 PR #227](https://github.com/KSAModding/content-index/pull/227)已合并。发布记录生成与公共快照验证状态见[发布记录](../../reports/community-publication-20261024.md)。旧目录中的 KsaChinese 条目和 v0.1.7 发布属于独立的纯中文项目。 KSA Babel 0.2.3 为正式发布，使用默认的 stable 通道。七种新增译文仍待独立母语审校。

通过 Borea 管理迁移时，先备份档案，再从该实例移除旧 KsaChinese 模组后安装 Babel：目录冲突会阻止安装，仅禁用旧模组仍不足以通过管理器规划。移除模组时保留档案数据。

## 构建与验证

准备 .NET 10 SDK、合法安装的精确适配版本游戏，以及 [StarMap 0.4.8](https://github.com/StarMapLoader/StarMap/releases/tag/0.4.8)。加载器文件放在 `.tools/StarMap-0.4.8`，或通过 `-LoaderDir` 指定。Python 脚本需要 Python 3，优先使用 conda 环境。

```powershell
python scripts/build_multilingual.py --check
.\scripts\build.ps1 -GameDir 'D:\Games\Kitten Space Agency' -Validate
python scripts/package_mod.py
python scripts/package_portable.py
```

可用 `KSA_GAME_DIR` 环境变量代替 `-GameDir` 指定游戏路径。游戏文件、`.tools`、本地档案及构建输出不进入 Git。新克隆不包含游戏、加载器、反编译工具、审计输入或 KSP 参考词典；可选的历史审计脚本需要另外准备相应本地输入。

逐条语言决策位于 `translations/multilingual/<locale>/*.jsonl`。`build_multilingual.py` 按已审底稿检查 ID、来源、审校状态、占位符和标记；不完整或过期的语言包会被拒绝。打包还要求源码、资源与已通过本地验证的构建一致。CI 检查可分发源码、数据和字体，不能执行需要合法本地游戏的编译或游戏验收。

生成的字体已包含在仓库中。重建需要 fontTools，以及哈希匹配 [FONT-SOURCE](../../assets/KsaBabel/FONT-SOURCE.txt) 的 `.tools/NotoSansSC-variable.ttf`；更换字体源需先复核。StarMap 下载标识和第三方许可见 [THIRD_PARTY](../../THIRD_PARTY.md)。

## 证据与贡献

- [九语种范围与验证](../../reports/0.2.0-九语种翻译与验证.md)
- [项目来源与导入文件指纹](../../reports/project-origin.json)
- [迁移与公开验证记录](../../reports/KSA-Babel-项目迁移与公开验证.md)
- [0.2.1 独立 ID 与管理器验证](../../reports/0.2.1-独立ID与管理器验证.md)

0.1.x 报告描述原 KSA Chinese 项目及其历史构建，保留用于来源追溯，不能证明 Babel 之前已发布或被收录。图形验证仅限记录中的测试场景、语言切换、设置／HUD 截图和固定数码图像区域，不代表完整视觉验收。

反馈译文时请提供语言代码、英文原文、游戏语境与建议措辞，欢迎母语审校。开发和提交遵循 **`developing → pull request → main`**。

## 许可

代码和自行编写的译文采用 **MIT**，保留 KSA Chinese 与 KsaUiLanguages 版权声明。字体采用 **SIL OFL 1.1**；便携包内的 StarMap、Harmony 采用各自的 MIT 许可。详见 [LICENSE](../../LICENSE)、[LICENSES](../../LICENSES) 与 [THIRD_PARTY](../../THIRD_PARTY.md)。

不分发 KSA／KSP 原始游戏资源、词典、反编译源码或个人数据。KSP 仅作本地术语参考，整套词典既不是产品构建输入，也不进入发行包。
