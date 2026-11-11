KSA Babel · 喵语塔 0.2.4 — 5801 九语种开发版

项目与下载：https://github.com/kuxiaowo/KSA-Babel
独立模组ID、安装目录与包名前缀为KsaBabel；插件程序集为KsaBabel.dll。
便携启动器为KsaBabel/KsaBabelLauncher.exe。
旧KsaChinese启用时Babel拒绝激活；请先禁用旧模组。
本项目已有独立的Borea公共目录条目KsaBabel；0.2.3通过stable通道正式发布。

仅适配 compatibility.json 中检查过的 KSA 2026.11.10.5801 文件。
通过 StarMap 0.5.4 加载，需要 .NET 10 x64 Runtime。
标准包供 StarMap/Borea 安装，见 README-生态安装.md；加载器需单独安装。
标准包名为KsaBabel-0.2.4.zip，便携包名为KsaBabel-0.2.4-5801-Portable.zip。
便携包的启动和卸载说明位于游戏目录下 KsaBabel/README-便携Mod.md；
README-安装卸载.md 为 5602 旧版独立安装流程，不适用于本版。

语言菜单即时切换 English、简体中文、日本語、Русский、Deutsch、Français、
Italiano、Español、Português (Brasil)，默认值由 language.json 决定。
菜单选择保存于当前档案 KsaBabel/settings.json，不写入共享模组文件。
首启仅在新设置文件不存在时，从同一档案旧KsaChinese/settings.json导入有效locale；
不覆盖已有新设置，不修改旧文件，不搜索其他档案，也不自动移动存档。
原生文本从 JSON native 字典通过提供器接入；zh-CN.resx 是同底稿导出的资源表，
resx 不单独激活显示，主要界面也需要代码补丁。
未审、缺失或上下文冲突的译文回退原文；未知游戏或依赖哈希安全禁用。
本版为软件正式发布；翻译覆盖仍有限，七种新增语言各覆盖现有5051条简中审核记录，并有11个动态模板。
此前只有旧版简体中文范围由维护者本人确认；其他七种语言由GPT结合KSA语境、参考KSP航天术语翻译/复核，尚无独立母语审校。
英文模式使用游戏原有英文与新增Language菜单；条目齐全不等于全游戏覆盖或语言质量验收。
初始翻译覆盖与运行验证限制见 reports/0.2.0-九语种翻译与验证.md；
该历史报告不自动证明0.2.3的重新构建验证完成，0.1.x报告也不是Babel的既往发布史。
此前 5638 的 93 条更新正文及上一版 5602 的 135 条正文已翻译；更早正文仍可能回退英文。
实际资源键、物理参数和存档不改；贴图印字、原生工具库和全部界面的视觉验收尚未完成。

部分代码及初始译文、1523 条日语候选来自 KsaUiLanguages（MIT），经本项目重新适配与复核；
字体使用 Noto Sans SC 子集（OFL）。所有原始署名和许可证均保留。
本包不含游戏DLL、原Core资源、反编译源码、KSP字典或个人存档。


5801 补译生命保障、乘员状态、居住区、EVA 物资、规则、编辑器撤销重做、部件变体与 HDR。更新日志共 242 行正文，短残句按原样保留。
