# KSA Babel：StarMap / Borea 安装与迁移

本项目九语种正式包为 **KsaBabel-0.2.2.zip**，从 [KSA Babel Releases](https://github.com/kuxiaowo/KSA-Babel/releases) 获取；旧公共目录中的 v0.1.6 属于原 KSA Chinese 项目。只适配 Windows x64 的 KSA **2026.10.19.5638**，需要 .NET 10 x64 Runtime 和 **StarMap 0.4.8**。包不附带加载器、启动器或游戏文件。代码和译文为 MIT；Noto Sans SC 字体子集为 OFL-1.1，来源见包内 THIRD_PARTY.md 和许可文件。新增语言为 AI 翻译与复核预览，尚无独立母语审校。

0.2.1 使用独立模组 ID **`KsaBabel`**，安装目录和包名前缀均为 `KsaBabel`，插件程序集为 `KsaBabel.dll`。**必须先禁用旧 `KsaChinese` 模组；旧模组仍启用时，Babel 拒绝激活。** 备份个人档案后再迁移，不要同时启用两个 Babel 副本。

## Borea

KSA Babel 的 [Ahwoo 帖子](https://forums.ahwoo.com/forums/kitten-space-agency/mod-releases/ksa-babel-0-2-1-nine-language-localization-preview-for-ksa-5638.1454/)已发布，[收录 PR #227](https://github.com/KSAModding/content-index/pull/227)已合并。Borea 刷新目录后可在默认 stable 通道获取 `KsaBabel 0.2.2` 正式版。0.2.1 保留为历史 testing 预览版，原收录证据见[发布记录](reports/community-publication-20261024.md)。原项目的 KsaChinese 条目与 v0.1.6 仍属于独立的纯中文版本。

也可以在 Borea 创建实例、安装 StarMap，把 ZIP 中的整个 `KsaBabel` 文件夹放到该实例的 `mods` 目录。禁用旧 `KsaChinese` 后启用 `KsaBabel`，再由 Borea 启动；手动安装的文件需要使用 Borea 的扫描/接管功能才能由它管理更新。不要把便携包作为标准模组导入。已合并的社区索引条目声明了与 `KsaChinese` 的 `conflict` 依赖。

通过 Borea 管理迁移时，先备份档案，再从该实例移除旧 KsaChinese 模组后安装 Babel：目录冲突会阻止安装，仅禁用旧模组仍不足以通过管理器规划。移除模组时保留档案数据。

## 直接使用 StarMap

从 [StarMap 官方发布](https://github.com/StarMapLoader/StarMap/releases/tag/0.4.8) 安装加载器，配置其 GameLocation 指向游戏。把 ZIP 中的 `KsaBabel` 文件夹放入所选玩家档案的 `mods` 文件夹；默认档案为 `Documents/My Games/Kitten Space Agency`，Borea 实例则使用自己的目录。

在游戏的模组管理界面禁用旧 `KsaChinese` 并启用 `KsaBabel`，或在该档案的 `manifest.toml` 中将旧 `KsaChinese` 条目的 `enabled` 改为 `false`，再加入以下条目，保留已有 Core 和其他模组条目：

```toml
[[mods]]
id = "KsaBabel"
enabled = true
```

通过 StarMap 启动。使用 `KSA.exe` 启动不会执行汉化插件。

## 从旧便携包迁移

1. 关闭游戏。备份旧 `KsaChinese/Profile`，其中可能有你的存档、载具和设置。
2. 在使用的档案中禁用旧 `KsaChinese`。将游戏目录中的旧 `Content/KsaChinese` 整个文件夹移出 `Content` 作为备份，不要删除含个人档案的启动器目录。Babel 不会代替你卸载或修改旧模组。
3. 安装标准模组包到 `mods/KsaBabel`，启用 `KsaBabel`，使用 Borea 或独立 StarMap 启动。不要同时保留 `Content/KsaBabel` 和实例 `mods/KsaBabel`。新的 Borea 实例不会自动读取旧便携档案；需要旧存档时，使用 Borea 的导入功能并先做好备份。
4. 确认迁移成功后，可移走旧启动器目录；在此之前保留其 Profile。不要直接删除含个人数据的整个 `KsaChinese` 目录。

## 设置、更新和卸载

0.2.2 的语言菜单即时切换 English、简体中文、日本語、Русский、Deutsch、Français、Italiano、Español 和 Português (Brasil)。菜单选择保存到**当前档案**的 `KsaBabel/settings.json`，不同实例互不影响，模组更新不会覆盖它。

首启时，仅在 `KsaBabel/settings.json` 不存在的情况下，才从**同一档案**旧 `KsaChinese/settings.json` 导入有效的 `locale`。不覆盖已有新文件，不修改旧文件，也不跨档案查找；旧文件缺失、无效或语言不受支持时使用包内 language.json 的默认值。因此，使用新建 Borea 实例或默认新便携档案不会自动导入另一个档案的设置或存档。日志位于当前档案 `logs/KsaBabel.log`；模组自带词典和字体从实际 DLL 目录读取，支持 Borea 的共享模组文件目录。

由 Borea 管理的模组使用 Borea 更新、禁用或卸载；手动安装则在游戏关闭后移走 `mods/KsaBabel` 并移除/禁用对应清单条目。卸载保留当前档案的个人设置和存档；若要重置语言，可单独移走 `KsaBabel/settings.json`。注意：新文件不存在而旧有效设置仍在时，下次启动会再次导入旧 `locale`；如需保留明确的新选择，可直接在游戏菜单中选择语言。

本项目是部分汉化。未翻译文字回退原文；未知游戏版本或不匹配的依赖哈希会禁用汉化。全部界面、旧版更新正文、原生工具库、异常文字和贴图印字仍有未完成范围。跨模组界面翻译与其他汉化插件并用未获保证。
