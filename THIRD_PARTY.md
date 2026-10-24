# 来源与许可

KSA Babel 从 kuxiaowo/KSA-CHINESE 的当前多语言工作快照独立整理，保留
Copyright (c) 2026 KSA Chinese contributors 及其 MIT 许可。来源：
https://github.com/kuxiaowo/KSA-CHINESE 。原始基线提交与迁移前文件哈希见
reports/project-origin.json；快照也包含当时尚未提交的多语言工作，不能把这些
改动误称为已存在于原仓库的某个公开提交。

本项目在 5638 重新适配、复核；5602 的旧版本单独保留；不是上游官方发行。

0.2.0 多语言预览继续保留原有许可。日语核心区段的 1523 条初始同键候选另取自
LiX-Works/KsaUiLanguages 的 MIT 固定提交 `61779119c18c994b8bf59458d2979100c9855dc3`，
逐记录来源见 translations/multilingual/ja-JP/core.jsonl；其余新增记录为本项目独立翻译，
标记 AI 复核，尚非母语审校。KSP 1.12.5 多语言资源仅在本机作术语参考，未复制整套
KSP 词典或说明文字进入运行包；公开镜像来源与核对局限见 0.2.0 报告。

- 部分插件代码、初始 1625 条译文：LiX-Works/KsaUiLanguages，MIT，提交 `b504774158cc03b582a0e75bfe94330cb841f11d`。来源：https://github.com/LiX-Works/KsaUiLanguages 。保留 `LICENSES/KsaUiLanguages-MIT.txt`。上游旧版本验证不构成本项目验证。
- 加载器：StarMap 0.4.8，MIT。来源：https://github.com/StarMapLoader/StarMap/releases/tag/0.4.8 。下载 ZIP SHA-256：`5324240f15d6841835158940057359ef011149bbc30b4d50f7b27480df7b2d5c`。保留 `LICENSES/StarMap-MIT.txt`。
- Harmony：随该 StarMap 包中的 `0Harmony.dll`，MIT，保留 `LICENSES/Harmony-MIT.txt`。
- 字体：Google Noto Sans SC，SIL OFL 1.1。源文件 SHA-256：`a3041811a78c361b1de50f953c805e0244951c21c5bd412f7232ef0d899af0da`。来源：https://github.com/google/fonts/tree/main/ofl/notosanssc 。生成字体已重新命名家族，保留版权、字体许可证和来源记录。
- KSA 本体 DLL、Core 文件、反编译源码、KSP 字典只作本机检查或术语参考，不在发行包和源码包中分发。
- 历史验证截图来自原项目已公开的测试记录，仅用于展示游戏界面和验证范围；画面中的游戏美术、标识和 UI 归原权利人，不由本项目按 MIT 重新授权。

运行需要已安装的 Microsoft .NET 10 x64 Runtime；本包不包含 .NET 安装器、SDK 或游戏运行时。
