# KSA Babel：论坛发布与管理器收录记录

日期：2026-10-24。本记录补充接入前验证报告，区分论坛公开、索引接受、发布包生成记录与客户端目录快照。

- [Ahwoo 正式帖子](https://forums.ahwoo.com/forums/kitten-space-agency/mod-releases/ksa-babel-0-2-1-nine-language-localization-preview-for-ksa-5638.1454/)由维护者手动发布，公开作者为 `zhengxin pang`。已读取公开正文，核对仅中文由维护者确认、七种语言由 GPT 参考 KSP 术语翻译、日语 MIT 来源、英文原文模式与纯中文项目链接。
- [content-index PR #227](https://github.com/KSAModding/content-index/pull/227)由 `kuxiaowo` 提交，所有上游检查通过后自动合并。只新增 `listings/KsaBabel.toml`，没有修改 CI、工具或原 `KsaChinese` 条目。合并提交为 `398053eb2e4d4386e24909d956ace9760ee0c1bc`。
- 最终条目副本为 [ecosystem/KsaBabel.toml](../ecosystem/KsaBabel.toml)，包含实际帖子地址、独立 ID、兼容范围、MIT/OFL 许可和与旧 ID 的冲突依赖。
- 公共发布记录和快照：**已验证**。[公共目录快照](https://ksamodding.github.io/content-index-releases/v1/index.json)包含 `KsaBabel` 和 `0.2.1`，发布状态为 `testing`。快照的 authored 源提交为 `398053eb2e4d4386e24909d956ace9760ee0c1bc`，generated 源提交为 `879af32ab3cbfc8660ebb1d9c6faab8043d51aac`；本次下载的快照 SHA-256 为 `9130a10fbaa1819ad8f899dff3f1639cf5501344d1d559b14acb6a8f601786fb`。快照会继续更新，此哈希只标识本次检查。
- 已按公共快照中的真实下载 URL 重新下载标准包，核对 SHA-256、1,584,720 字节、26 个文件、ZIP 完整性和 `KsaBabel` 安装根目录；解压尺寸为 4,580,848 字节。快照的 authored 内容与提交条目完全一致，StarMap 0.4.8、KSA 5638 和旧 ID 冲突信息正确。

在 Borea 刷新目录后查找 `KSA Babel`。0.2.1 是 testing 预览版本，使用允许测试版本的发布通道；不要改装成稳定版记录。通过管理器迁移前移除旧 `KsaChinese` 模组并保留档案数据。此次验证确认公共目录与实际下载包，不宣称重新完成一次 Borea 图形界面安装。

## 最终检查

在上游 `content-index` 提交 `01e5c4b93217d667f69af26edf5f09f0ce930444` 和 `content-index-releases` 提交 `10eab1811ba95f7f62584567fd3ca1214f50bc08` 的工具上，填入实际论坛链接后的以下检查均返回 0：

1. `check_layout.py`
2. `check_schema.py`
3. `check_index.py`
4. `check_license.py`
5. `check_status.py`
6. `check_images.py`
7. `check_packs.py`
8. `check_tags.py`
9. `check_images.py listings/KsaBabel.toml`
10. `check_release.py listings/KsaBabel.toml --releases <content-index-releases>`

最后一项通过网络下载真实托管发布，结果为 `the latest release 0.2.1 stamps cleanly`。标准包为 `KsaBabel-0.2.1.zip`，SHA-256 为 `bed6d6f226d036324e018ced5fd8840584c6ba00b69bdbd8803a4ab17e7cfc8a`。

上游 `layout`、`schema`、`page`、`verdict` 和所有权 `validate` 检查均通过。[上游验证运行](https://github.com/KSAModding/content-index/actions/runs/40104848685)。上游提示 `game_max` 会阻止未来游戏构建；本模组本来就对未知构建和哈希拒绝激活，因此保留精确上限。

结构化状态和检查输出见 [community-publication-20261024.json](community-publication-20261024.json)。本次公开检查不启动游戏或控制电脑，不代替母语审校、完整视觉验收或一次新的 Borea 图形界面安装测试。之前的隔离运行和管理组件证据仍以 [0.2.1 报告](0.2.1-独立ID与管理器验证.md)为准。
