# KSA Babel 社区索引准备

当前模板为 `KsaBabel.toml.in`，对应独立 ID `KsaBabel` 和 0.2.1 起的发布。模板尚缺真实论坛 URL，不能直接提交。`scripts/prepare_listing.py` 只在填入发布后的真实 Ahwoo 帖子地址时生成最终条目。

新的 Babel 论坛中英文草稿及申请内容见 [babel-publication-draft.json](babel-publication-draft.json)，供维护者审阅，尚未发布。草稿明确区分游戏原有英文、新增语言菜单、本人确认的中文与 GPT 参考 KSP 术语生成的七种新增语言，并列出纯中文项目仓库、ID、v0.1.6 发布及独立目录条目。下方历史帖子属于原中文项目。

接入前验证与范围见 [0.2.1 报告](../reports/0.2.1-独立ID与管理器验证.md)。`scripts/prepare_ecosystem_validation.py` 使用本机 `.tools/content-index`、`.tools/content-index-releases` 与已构建标准包生成忽略目录中的候选索引；不向外部服务提交元数据。Python 需要上游 tools/requirements.txt 中的依赖，优先使用 conda。

`src/EcosystemValidation` 引用 `.tools/Borea-source` 的原版 v0.5.1 源组件。使用 ArtifactsPath 隔离所有项目的构建产物，不修改上游源码：

```powershell
dotnet build src/EcosystemValidation -c Release -p:ArtifactsPath=D:/KSA-Babel/work/ecosystem-artifacts
dotnet work/ecosystem-artifacts/bin/EcosystemValidation/release/Borea.Cli.Tests.dll D:/KSA-Babel D:/KSA-Babel/work/test-profile-borea-new D:/KSA-Test-Game --management-only
```

项目根、全新 work/test-profile-* 目录与隔离游戏副本必须换成自己的路径。`--management-only` 实际执行文件安装、启停、更新和卸载，但以记录器替代 OS 进程边界，不启动游戏窗口；其中 saves 哨兵只验证所有权保留，不是游戏存档加载测试。省略该参数会进入独立档案的真实游戏验证，会打开游戏窗口；窗口被最小化时可能暂停渲染，不能把超时或中断写成通过。

候选旧 0.2.0 是当前源码以不同版本构建的本地合成夹具，不是曾公开的 KsaBabel 发行版。合成论坛地址只用于本地规范验证，不能发布。最终正式申请还需真实论坛地址、最终上游检查及收录后的公共目录下载验证。

## 原中文项目的历史材料

`forum-post.en.md`、`index-pr.en.md` 和原 KsaChinese 条目记录 KSA Chinese 0.1.6 的历史，不是 Babel 的发布文案或收录证明。旧条目的论坛、仓库、安装 ID 与当前 Babel 均需区分；不得把原帖地址直接当作新项目已经发帖的证据。
