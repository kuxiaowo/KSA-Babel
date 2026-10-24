# KSA Babel · 喵语塔

[English](../../README.md) · [简体中文](README.zh-CN.md) · [日本語](README.ja-JP.md) · [Русский](README.ru-RU.md) · [Deutsch](README.de-DE.md) · [Français](README.fr-FR.md) · [Italiano](README.it-IT.md) · [Español](README.es-ES.md) · [Português (Brasil)](README.pt-BR.md)

Une infrastructure de localisation multilingue pour **Kitten Space Agency**, chargée par StarMap. KSA Babel fournit des packs de langues, un menu de sélection de la langue en jeu, des adaptations de l’affichage et un sous-ensemble de police commun. Il ne remplace ni l’exécutable ni les ressources du jeu.

KSA Babel est un projet multilingue constitué séparément à partir de [KSA-CHINESE](https://github.com/kuxiaowo/KSA-CHINESE). La version actuelle est la **préversion 0.2.0**, compatible uniquement avec **KSA 2026.10.19.5638 sous Windows x64**, pour les empreintes de fichiers vérifiées ; le chargeur utilisé est **StarMap 0.4.8**.

## Langues et périmètre

English · 简体中文 · 日本語 · Русский · Deutsch · Français · Italiano · Español · Português (Brasil)

Les sept langues supplémentaires couvrent chacune **4 786 entrées** du périmètre chinois déjà révisé, ainsi que **11 modèles de texte dynamique**. L’anglais utilise les textes originaux du jeu. La présence de toutes les entrées **ne signifie pas que tout le jeu est traduit**. Les nouvelles traductions ont fait l’objet d’une révision contextuelle par IA, **sans validation indépendante par des locuteurs natifs**. Les anciens journaux des modifications, les textes des bibliothèques natives et des erreurs, les inscriptions intégrées aux textures et certains états de l’interface restent hors du périmètre révisé. Les commandes étroites et les autres résolutions nécessitent encore une vérification visuelle plus large.

Le périmètre actuel comprend l’interface, les infobulles, les pièces, les substances et leurs phases, les instruments et une partie des notes de mise à jour. Les désignations de modèles, unités, paramètres de substitution, identifiants de ressources, de commandes et de sauvegardes, données d’énumération et noms personnalisés par les joueurs sont conservés. Les versions inconnues sont refusées par sécurité ; les traductions manquantes laissent place au texte original. Le sous-ensemble Noto Sans SC couvre les caractères recensés, sans garantir tous les noms choisis par les joueurs ni toutes les formes régionales des glyphes japonais.

## Installation

Téléchargez les fichiers depuis [les versions de KSA Babel](https://github.com/kuxiaowo/KSA-Babel/releases). Les paquets 0.2.0 sont des préversions.

- **Avec StarMap/Borea déjà installé :** utilisez `KsaChinese-0.2.0.zip` ; placez son dossier `KsaChinese` dans le répertoire `mods` du profil choisi, puis activez ce mod. Consultez [l’installation standard](../../README-生态安装.md).
- **Avec le lanceur inclus :** extrayez `KsaChinese-0.2.0-5638-Portable.zip` dans le dossier contenant `KSA.exe`, puis lancez `KsaChinese/KsaChineseLauncher.exe`. Consultez [l’installation portable](../../README-便携Mod.md). Le runtime .NET 10 x64 est requis.

L’identifiant interne du mod, l’assembly, le répertoire des paramètres et le préfixe des archives restent **`KsaChinese`** pour assurer la compatibilité. **N’activez qu’un seul exemplaire :** n’installez pas KSA Babel en même temps que KSA Chinese et ne conservez pas simultanément `Content/KsaChinese` et `mods/KsaChinese` dans le profil. Sauvegardez vos profils existants avant toute migration. Aucune sauvegarde personnelle n’est incluse ni déplacée automatiquement.

Le nouveau projet n’a pas encore fait l’objet d’une nouvelle demande d’inscription au catalogue public de Borea. L’entrée KsaChinese de l’ancien catalogue et la version v0.1.6 appartiennent au projet d’origine et ne constituent pas un point de téléchargement de Babel. Installez manuellement les fichiers des versions publiées dans ce dépôt ; aucun message n’a été envoyé aux responsables du forum ou de l’index pour ce projet.

## Compilation et validation

Utilisez le SDK .NET 10, une installation légale de la version exacte du jeu prise en charge et [StarMap 0.4.8](https://github.com/StarMapLoader/StarMap/releases/tag/0.4.8). Placez les fichiers du chargeur dans `.tools/StarMap-0.4.8`, ou indiquez `-LoaderDir`. Les scripts Python nécessitent Python 3 ; conda est recommandé.

```powershell
python scripts/build_multilingual.py --check
.\scripts\build.ps1 -GameDir 'D:\Games\Kitten Space Agency' -Validate
python scripts/package_mod.py
python scripts/package_portable.py
```

`KSA_GAME_DIR` permet de fournir le chemin local du jeu à la place de `-GameDir`. Les fichiers binaires du jeu, `.tools`, les profils locaux et les résultats de compilation sont ignorés par Git. Un clone neuf ne contient ni le jeu, ni le chargeur, ni le décompilateur, ni les données d’entrée des audits, ni les dictionnaires de référence de KSP. Les scripts facultatifs d’audit historique nécessitent leurs propres données locales, préparées séparément.

Les décisions de traduction se trouvent dans `translations/multilingual/<locale>/*.jsonl`. `build_multilingual.py` vérifie les identifiants, la provenance, le statut de révision, les paramètres de substitution et les balises par rapport à la base révisée ; les packs incomplets ou périmés sont refusés. La création des paquets exige également une validation locale réussie correspondant à la compilation, avec des sources et des ressources inchangées. La CI vérifie les données portables et la police ; elle ne peut pas effectuer la compilation nécessitant une copie sous licence du jeu ni valider le fonctionnement en jeu.

La police générée est déjà incluse. Sa reconstruction nécessite fontTools et `.tools/NotoSansSC-variable.ttf`, dont le SHA-256 doit correspondre à celui de [FONT-SOURCE](../../assets/KsaChinese/FONT-SOURCE.txt) ; toute modification de la source doit être révisée. L’identité du téléchargement de StarMap et les licences des composants tiers sont consignées dans [THIRD_PARTY](../../THIRD_PARTY.md).

## Éléments de vérification et contributions

- [Périmètre et vérification des neuf langues](../../reports/0.2.0-九语种翻译与验证.md)
- [Origine du projet et empreintes des fichiers importés](../../reports/project-origin.json)
- [Notes de migration et de publication](../../reports/KSA-Babel-项目迁移与公开验证.md)

Les rapports 0.1.x décrivent le projet KSA Chinese d’origine et ses compilations historiques. Ils sont conservés pour documenter la provenance, et ne prouvent pas que Babel ait déjà été publié ou inscrit dans un index. Les éléments visuels se limitent à la scène de test consignée, aux changements de langue, aux captures des paramètres et du HUD et à des zones fixes contenant des valeurs numériques ; ils ne constituent pas une validation visuelle exhaustive.

Les contributions doivent préciser la langue, le texte anglais d’origine, le contexte dans le jeu et la formulation proposée. La révision par des locuteurs natifs est particulièrement utile. Le développement et les commits suivent **`developing → pull request → main`**.

## Licence

Code et traductions créées pour le projet : **MIT**, avec conservation des mentions de droits d’auteur de KSA Chinese et de KsaUiLanguages. Police : **SIL OFL 1.1**. Le paquet portable inclut StarMap et Harmony sous leurs licences MIT respectives. Consultez [LICENSE](../../LICENSE), [LICENSES](../../LICENSES) et [THIRD_PARTY](../../THIRD_PARTY.md).

Les ressources des jeux KSA et KSP, leurs dictionnaires originaux, les sources décompilées et les données personnelles ne sont pas distribués. KSP a servi de référence terminologique locale ; son dictionnaire complet n’est ni une donnée d’entrée du produit ni un élément de la version distribuée.
