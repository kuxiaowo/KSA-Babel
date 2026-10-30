# KSA Babel 社区索引准备

本轮正式版本为 **0.2.3**，适配精确 KSA 2026.10.25.5707。新增范围及验证见 [0.2.3 报告](../reports/0.2.3-5707适配与验证.md)。下文保留此前目录发布历史。此前正式版本为 **0.2.2**，使用 `stable` 通道。原 0.2.1 保留为历史 testing 预览版；目录会固定已收录版本的通道，因此正式发布采用新版本。构建与打包证据见 [0.2.2 报告](../reports/0.2.2-正式发布与验证.md)。`[releases].since = "0.2.1"` 保留历史发布，不限制后续正式版本。

最终 Babel 条目为 [KsaBabel.toml](KsaBabel.toml)，对应独立 ID `KsaBabel` 和 0.2.1 起的发布，已填入维护者发布的真实 Ahwoo 地址。[收录 PR #227](https://github.com/KSAModding/content-index/pull/227)已通过检查并合并；发布记录和公共目录快照验证状态见[发布记录](../reports/community-publication-20261024.md)。`KsaBabel.toml.in` 保留为生成源模板；`scripts/prepare_listing.py` 用实际论坛 URL 生成最终条目。

维护者已选定九语言文字 v3 封面。[目录封面更新 PR #228](https://github.com/KSAModding/content-index/pull/228)只更新 Babel 的图标与描述图片；记录中的 URL 固定到公开提交，尺寸、字节数与 SHA-256 已通过实际网络下载检查。原图、缩放导出和来源说明见[封面目录](../docs/branding/README.md)。模组二进制、版本号、兼容范围和原 KsaChinese 条目不变。论坛追加封面用的 BBCode 见 [babel-forum-cover.bbcode](babel-forum-cover.bbcode)；当前论坛 API 返回 403，本轮没有修改论坛正文。

已审阅的 Babel 中英文草稿及实际公开链接见 [babel-publication-draft.json](babel-publication-draft.json)。维护者已手动发布[正式帖子](https://forums.ahwoo.com/forums/kitten-space-agency/mod-releases/ksa-babel-0-2-1-nine-language-localization-preview-for-ksa-5638.1454/)；草稿数组保留原审阅文案，实际发布内容以帖子为准。帖子与条目明确区分游戏原有英文、新增语言菜单、本人确认的中文与 GPT 参考 KSP 术语生成的七种新增语言，并列出纯中文项目仓库、ID、v0.1.6 发布及独立目录条目。下方历史帖子属于原中文项目。

接入前验证与范围见 [0.2.1 报告](../reports/0.2.1-独立ID与管理器验证.md)。`scripts/prepare_ecosystem_validation.py` 使用本机 `.tools/content-index`、`.tools/content-index-releases` 与已构建标准包生成忽略目录中的候选索引；不向外部服务提交元数据。Python 需要上游 tools/requirements.txt 中的依赖，优先使用 conda。

`src/EcosystemValidation` 引用 `.tools/Borea-source` 的原版 v0.5.1 源组件。使用 ArtifactsPath 隔离所有项目的构建产物，不修改上游源码：

```powershell
dotnet build src/EcosystemValidation -c Release -p:ArtifactsPath=D:/KSA-Babel/work/ecosystem-artifacts
dotnet work/ecosystem-artifacts/bin/EcosystemValidation/release/Borea.Cli.Tests.dll D:/KSA-Babel D:/KSA-Babel/work/test-profile-borea-new D:/KSA-Test-Game --management-only
```

项目根、全新 work/test-profile-* 目录与隔离游戏副本必须换成自己的路径。`--management-only` 实际执行文件安装、启停、更新和卸载，但以记录器替代 OS 进程边界，不启动游戏窗口；其中 saves 哨兵只验证所有权保留，不是游戏存档加载测试。省略该参数会进入独立档案的真实游戏验证，会打开游戏窗口；窗口被最小化时可能暂停渲染，不能把超时或中断写成通过。

候选旧 0.2.0 是当前源码以不同版本构建的本地合成夹具，不是曾公开的 KsaBabel 发行版。合成论坛地址只用于历史本地规范验证，不用于公开条目。实际论坛地址、最终上游检查与后续公共目录下载验证独立记录在[发布记录](../reports/community-publication-20261024.md)中，历史夹具报告不改写为公开收录证据。

## 原中文项目的历史材料

`forum-post.en.md`、`index-pr.en.md` 和原 KsaChinese 条目记录 KSA Chinese 0.1.6 的历史，不是 Babel 的发布文案或收录证明。旧条目的论坛、仓库、安装 ID 与当前 Babel 均需区分；不得把原帖地址直接当作新项目已经发帖的证据。
