# KSA Babel · 喵语塔

[English](../../README.md) · [简体中文](README.zh-CN.md) · [日本語](README.ja-JP.md) · [Русский](README.ru-RU.md) · [Deutsch](README.de-DE.md) · [Français](README.fr-FR.md) · [Italiano](README.it-IT.md) · [Español](README.es-ES.md) · [Português (Brasil)](README.pt-BR.md)

<p align="center"><img src="../branding/ksa-babel-cover-preview-v3.png" alt="KSA Babel · 喵语塔" width="512"></p>

Uma estrutura de localização multilíngue para **Kitten Space Agency**, carregada pelo StarMap. KSA Babel fornece pacotes de idiomas, um menu de idiomas dentro do jogo, adaptadores de exibição e uma fonte compartilhada com um subconjunto de caracteres. Não substitui o executável nem os recursos do jogo.

KSA Babel é um projeto multilíngue independente derivado de [KSA-CHINESE](https://github.com/kuxiaowo/KSA-CHINESE). A versão atual é a **prévia 0.2.1** e é compatível apenas com **KSA 2026.10.19.5638 para Windows x64**, com os hashes de arquivos verificados. O carregador utilizado é o **StarMap 0.4.8**.

**Revisão e modo inglês:** o autor confirmou pessoalmente apenas a tradução para chinês simplificado. Japonês, russo, alemão, francês, italiano, espanhol e português brasileiro foram traduzidos e revisados pelo GPT no contexto do KSA, usando a terminologia aeroespacial do KSP como referência; ainda falta revisão independente de falantes nativos. Parte das traduções japonesas iniciais veio do KsaUiLanguages sob MIT, com os créditos preservados. O modo inglês mostra o texto inglês original do KSA e mantém o menu `Language` adicionado pelo mod. Os 11 modelos dinâmicos servem à composição multilíngue e ao retorno ao original, não representam 11 textos ingleses reescritos.

**Edição chinesa:** [KSA Chinese / KSA-CHINESE](https://github.com/kuxiaowo/KSA-CHINESE), ID `KsaChinese`, oferece inglês e chinês simplificado. A versão atual é [v0.1.6](https://github.com/kuxiaowo/KSA-CHINESE/releases/tag/v0.1.6), arquivo padrão `KsaChinese-0.1.6.zip`, para os arquivos verificados do KSA 2026.10.19.5638 no Windows x64 e StarMap 0.4.8. Tem sua própria [entrada no Borea](https://github.com/KSAModding/content-index/blob/main/listings/KsaChinese.toml). Babel é a prévia independente com nove idiomas e ID `KsaBabel`. Não ative os dois mods no mesmo perfil.

## Idiomas e escopo

English · 简体中文 · 日本語 · Русский · Deutsch · Français · Italiano · Español · Português (Brasil)

Cada um dos sete idiomas adicionais cobre **4.786 registros** do escopo já revisado em chinês simplificado, além de **11 modelos dinâmicos**. O inglês usa o texto original do jogo. A completude dos registros **não significa que todo o jogo esteja traduzido**. As novas traduções passaram por revisão contextual por IA, **não por aprovação independente de falantes nativos**. Históricos de alterações mais antigos, textos de bibliotecas nativas e erros, inscrições em texturas e alguns estados da interface ainda estão fora do escopo revisado. Controles estreitos e outras resoluções ainda exigem uma revisão visual mais ampla.

Esta versão abrange interface, dicas, peças, substâncias e fases da matéria, instrumentos e parte do histórico de alterações. São preservados modelos, unidades, parâmetros de substituição, IDs de recursos, controles e partidas salvas, dados de enumerações e nomes personalizados pelo jogador. Versões desconhecidas são recusadas de forma segura; textos sem tradução usam o original. O subconjunto de Noto Sans SC cobre os caracteres inventariados, mas não garante todos os nomes que o jogador possa digitar nem todas as formas regionais dos caracteres japoneses.

## Instalação

Baixe os arquivos em [KSA Babel Releases](https://github.com/kuxiaowo/KSA-Babel/releases). Os pacotes 0.2.1 são prévias.

- **Com StarMap/Borea já instalado:** use `KsaBabel-0.2.1.zip`; coloque a pasta `KsaBabel` no diretório `mods` do perfil selecionado e ative o mod. Consulte a [instalação padrão](../../README-生态安装.md).
- **Com inicializador incluído:** extraia `KsaBabel-0.2.1-5638-Portable.zip` na pasta que contém `KSA.exe` e execute `KsaBabel/KsaBabelLauncher.exe`. Consulte a [instalação portátil](../../README-便携Mod.md). Requer .NET 10 x64 Runtime.

A partir da versão 0.2.1, o ID do mod, os diretórios de instalação e o prefixo dos arquivos são **`KsaBabel`**; o assembly do plugin é `KsaBabel.dll` e o inicializador portátil é `KsaBabel/KsaBabelLauncher.exe`. O idioma é salvo em `KsaBabel/settings.json` do perfil atual. Na primeira inicialização, somente se esse novo arquivo não existir, o Babel importa um valor válido de `locale` do antigo `KsaChinese/settings.json` do mesmo perfil. As novas configurações já existentes não são sobrescritas e o arquivo antigo não é alterado.

**Desative primeiro o mod antigo:** o Babel recusa a ativação enquanto `KsaChinese` estiver ativado. Mantenha apenas uma cópia ativa do Babel e não conserve simultaneamente `Content/KsaBabel` e `mods/KsaBabel` no perfil. Faça backup dos perfis existentes antes de migrar. Partidas pessoais não são incluídas nem movidas automaticamente; a importação de configurações não procura em outros perfis.

[O anúncio no Ahwoo](https://forums.ahwoo.com/forums/kitten-space-agency/mod-releases/ksa-babel-0-2-1-nine-language-localization-preview-for-ksa-5638.1454/) foi publicado e a [PR #227 do catálogo do Borea](https://github.com/KSAModding/content-index/pull/227) foi integrada. Consulte o [registro de publicação](../../reports/community-publication-20261024.md) para acompanhar o processamento da versão e a verificação do snapshot público. A entrada KsaChinese existente e a versão v0.1.6 pertencem ao projeto separado dedicado ao chinês. KSA Babel 0.2.1 já está no catálogo público como prévia `testing`; use um canal que permita versões de teste.

Para migrar pelo Borea, faça uma cópia do perfil e remova o mod antigo KsaChinese da instância antes de instalar o Babel. O conflito do catálogo bloqueia a instalação mesmo com o mod antigo desativado. Preserve os dados do perfil.

## Compilação e validação

Use .NET 10 SDK, uma cópia legalmente instalada da versão exata do jogo compatível e [StarMap 0.4.8](https://github.com/StarMapLoader/StarMap/releases/tag/0.4.8). Coloque os arquivos do carregador em `.tools/StarMap-0.4.8` ou informe `-LoaderDir`. Os scripts exigem Python 3; o uso de conda é preferencial.

```powershell
python scripts/build_multilingual.py --check
.\scripts\build.ps1 -GameDir 'D:\Games\Kitten Space Agency' -Validate
python scripts/package_mod.py
python scripts/package_portable.py
```

`KSA_GAME_DIR` pode informar o caminho local do jogo no lugar de `-GameDir`. Binários do jogo, `.tools`, perfis locais e saídas de compilação são ignorados pelo controle de versão. Um clone novo não contém o jogo, o carregador, o descompilador, os dados de entrada das auditorias nem os dicionários de referência de KSP. Scripts opcionais de auditorias históricas exigem seus dados locais preparados separadamente.

As decisões de tradução ficam em `translations/multilingual/<locale>/*.jsonl`. `build_multilingual.py` confere IDs, procedência, estado de revisão, parâmetros de substituição e marcação com a base revisada; pacotes incompletos ou desatualizados são rejeitados. O empacotamento também exige validação local de compilação bem-sucedida e correspondente a fontes e recursos sem alterações posteriores. A integração contínua verifica os dados portáveis e a fonte; ela não pode compilar usando uma instalação licenciada do jogo nem aprovar a experiência de jogo.

A fonte gerada já está incluída. Para recriá-la, são necessários fontTools e `.tools/NotoSansSC-variable.ttf` com o SHA-256 indicado em [FONT-SOURCE](../../assets/KsaBabel/FONT-SOURCE.txt); alterações no arquivo-fonte exigem revisão. A identificação do download do StarMap e as licenças de terceiros estão registradas em [THIRD_PARTY](../../THIRD_PARTY.md).

## Evidências e contribuições

- [Escopo e verificação dos nove idiomas](../../reports/0.2.0-九语种翻译与验证.md)
- [Origem do projeto e impressões digitais dos arquivos importados](../../reports/project-origin.json)
- [Notas de migração e publicação](../../reports/KSA-Babel-项目迁移与公开验证.md)
- [Verificação do ID e do gerenciador 0.2.1](../../reports/0.2.1-独立ID与管理器验证.md)

Os relatórios 0.1.x descrevem o projeto original KSA Chinese e compilações históricas. São mantidos como registro de procedência, não como prova de publicações ou inclusões anteriores de Babel em catálogos. As evidências da interface gráfica se limitam à cena de teste registrada, às trocas de idioma, às capturas de configurações/HUD e a regiões fixas da imagem com valores numéricos; não representam uma aprovação visual exaustiva.

As contribuições devem identificar o idioma, o texto original em inglês, o contexto no jogo e a redação sugerida. A revisão de falantes nativos é especialmente útil. O desenvolvimento e os commits seguem **`developing → pull request → main`**.

## Licença

Código e traduções de autoria própria: **MIT**, preservando os avisos de direitos autorais de KSA Chinese e KsaUiLanguages. Fonte: **SIL OFL 1.1**. O pacote portátil inclui StarMap e Harmony sob suas licenças MIT. Consulte [LICENSE](../../LICENSE), [LICENSES](../../LICENSES) e [THIRD_PARTY](../../THIRD_PARTY.md).

Não são distribuídos recursos dos jogos KSA ou KSP, dicionários originais, código descompilado nem dados pessoais. KSP serviu como referência terminológica local; seu dicionário completo não é uma entrada do produto nem faz parte da distribuição.
