# KSA Babel · 喵语塔

[English](../../README.md) · [简体中文](README.zh-CN.md) · [日本語](README.ja-JP.md) · [Русский](README.ru-RU.md) · [Deutsch](README.de-DE.md) · [Français](README.fr-FR.md) · [Italiano](README.it-IT.md) · [Español](README.es-ES.md) · [Português (Brasil)](README.pt-BR.md)

<p align="center"><img src="../branding/ksa-babel-cover-preview-v3.png" alt="KSA Babel · 喵语塔" width="512"></p>

Une infrastructure de localisation multilingue pour **Kitten Space Agency**, chargée par StarMap. KSA Babel fournit des packs de langues, un menu de sélection de la langue en jeu, des adaptations de l’affichage et un sous-ensemble de police commun. Il ne remplace ni l’exécutable ni les ressources du jeu.

KSA Babel est un projet multilingue constitué séparément à partir de [KSA-CHINESE](https://github.com/kuxiaowo/KSA-CHINESE). La version actuelle est la **version stable 0.2.3**, compatible uniquement avec **KSA 2026.10.25.5707 sous Windows x64**, pour les empreintes de fichiers vérifiées ; le chargeur utilisé est **StarMap 0.4.8**.

**Révision et mode anglais :** l’auteur n’a personnellement confirmé que la traduction en chinois simplifié. Le japonais, le russe, l’allemand, le français, l’italien, l’espagnol et le portugais brésilien ont été traduits et révisés par GPT dans le contexte de KSA, en utilisant la terminologie aérospatiale de KSP comme référence ; une révision indépendante par des locuteurs natifs reste à faire. Une partie des premières traductions japonaises provient de KsaUiLanguages sous MIT, avec attribution conservée. Le mode anglais affiche les textes anglais originaux de KSA et conserve le menu `Language` ajouté par le mod. Les 11 modèles dynamiques servent à la composition multilingue et au retour à l’original ; ils ne correspondent pas à 11 textes anglais réécrits.

**Édition chinoise :** [KSA Chinese / KSA-CHINESE](https://github.com/kuxiaowo/KSA-CHINESE), ID `KsaChinese`, permet de choisir entre anglais et chinois simplifié. Version actuelle : [v0.1.7](https://github.com/kuxiaowo/KSA-CHINESE/releases/tag/v0.1.7), archive standard `KsaChinese-0.1.7.zip`, pour les fichiers vérifiés de KSA 2026.10.25.5707 sous Windows x64 et StarMap 0.4.8. Elle possède sa propre [entrée Borea](https://github.com/KSAModding/content-index/blob/main/listings/KsaChinese.toml). Babel est la mod indépendant à neuf langues, ID `KsaBabel`. N’activez pas les deux dans le même profil.

0.2.3 ajoute 265 entrées pour KSA 5707 : météo spatiale et CME, HDR, irradiation, récupération et retraite de l’équipage, blindage et pression, ainsi que 156 notes de mise à jour. Toutes les nouvelles traductions, y compris le chinois, ont une révision contextuelle par IA, sans confirmation personnelle de l’auteur ni validation indépendante par des locuteurs natifs. La confirmation antérieure concerne seulement l’ancien périmètre chinois. Voir le rapport 0.2.3 pour les essais en jeu et leurs limites. [0.2.3](../../reports/0.2.3-5707适配与验证.md)

## Langues et périmètre

English · 简体中文 · 日本語 · Русский · Deutsch · Français · Italiano · Español · Português (Brasil)

Les sept langues supplémentaires couvrent chacune **5 051 entrées** du périmètre chinois déjà révisé, ainsi que **11 modèles de texte dynamique**. L’anglais utilise les textes originaux du jeu. La présence de toutes les entrées **ne signifie pas que tout le jeu est traduit**. Les nouvelles traductions ont fait l’objet d’une révision contextuelle par IA, **sans validation indépendante par des locuteurs natifs**. Les anciens journaux des modifications, les textes des bibliothèques natives et des erreurs, les inscriptions intégrées aux textures et certains états de l’interface restent hors du périmètre révisé. Les commandes étroites et les autres résolutions nécessitent encore une vérification visuelle plus large.

Le périmètre actuel comprend l’interface, les infobulles, les pièces, les substances et leurs phases, les instruments et une partie des notes de mise à jour. Les désignations de modèles, unités, paramètres de substitution, identifiants de ressources, de commandes et de sauvegardes, données d’énumération et noms personnalisés par les joueurs sont conservés. Les versions inconnues sont refusées par sécurité ; les traductions manquantes laissent place au texte original. Le sous-ensemble Noto Sans SC couvre les caractères recensés, sans garantir tous les noms choisis par les joueurs ni toutes les formes régionales des glyphes japonais.

## Installation

Téléchargez les fichiers depuis [les versions de KSA Babel](https://github.com/kuxiaowo/KSA-Babel/releases). Les paquets 0.2.3 sont des versions stables.

- **Avec StarMap/Borea déjà installé :** utilisez `KsaBabel-0.2.3.zip` ; placez son dossier `KsaBabel` dans le répertoire `mods` du profil choisi, puis activez ce mod. Consultez [l’installation standard](../../README-生态安装.md).
- **Avec le lanceur inclus :** extrayez `KsaBabel-0.2.3-5707-Portable.zip` dans le dossier contenant `KSA.exe`, puis lancez `KsaBabel/KsaBabelLauncher.exe`. Consultez [l’installation portable](../../README-便携Mod.md). Le runtime .NET 10 x64 est requis.

À partir de 0.2.1, l’identifiant du mod, les répertoires d’installation et le préfixe des archives sont **`KsaBabel`** ; l’assembly du plugin est `KsaBabel.dll` et le lanceur portable est `KsaBabel/KsaBabelLauncher.exe`. La langue est enregistrée dans `KsaBabel/settings.json` du profil actuel. Au premier démarrage, uniquement si ce nouveau fichier n’existe pas, Babel importe une valeur `locale` valide depuis l’ancien `KsaChinese/settings.json` du même profil. Les nouveaux paramètres existants ne sont jamais écrasés et l’ancien fichier n’est pas modifié.

**Désactivez d’abord l’ancien mod :** Babel refuse de s’activer tant que `KsaChinese` est activé. Ne conservez qu’un seul exemplaire activé de Babel et ne gardez pas simultanément `Content/KsaBabel` et `mods/KsaBabel` dans le profil. Sauvegardez vos profils existants avant toute migration. Aucune sauvegarde personnelle n’est incluse ni déplacée automatiquement ; l’importation des paramètres ne recherche aucun autre profil.

[L’annonce sur Ahwoo](https://forums.ahwoo.com/forums/kitten-space-agency/mod-releases/ksa-babel-0-2-1-nine-language-localization-preview-for-ksa-5638.1454/) est publiée et la [PR #227 du catalogue Borea](https://github.com/KSAModding/content-index/pull/227) a été fusionnée. Consultez le [suivi de publication](../../reports/community-publication-20261024.md) pour la génération des métadonnées de version et la vérification de l’instantané public. L’entrée KsaChinese existante et la version v0.1.7 appartiennent au projet distinct consacré au chinois. KSA Babel 0.2.3 est une version stable, disponible sur le canal stable par défaut. Les sept traductions supplémentaires attendent toujours une révision indépendante par des locuteurs natifs.

Pour migrer avec Borea, sauvegardez le profil et retirez l’ancien mod KsaChinese de l’instance avant d’installer Babel. Le conflit du catalogue bloque l’installation même si l’ancien mod est désactivé. Conservez les données du profil.

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

La police générée est déjà incluse. Sa reconstruction nécessite fontTools et `.tools/NotoSansSC-variable.ttf`, dont le SHA-256 doit correspondre à celui de [FONT-SOURCE](../../assets/KsaBabel/FONT-SOURCE.txt) ; toute modification de la source doit être révisée. L’identité du téléchargement de StarMap et les licences des composants tiers sont consignées dans [THIRD_PARTY](../../THIRD_PARTY.md).

## Éléments de vérification et contributions

- [Périmètre et vérification des neuf langues](../../reports/0.2.0-九语种翻译与验证.md)
- [Origine du projet et empreintes des fichiers importés](../../reports/project-origin.json)
- [Notes de migration et de publication](../../reports/KSA-Babel-项目迁移与公开验证.md)
- [Vérification de l’identifiant et du gestionnaire 0.2.1](../../reports/0.2.1-独立ID与管理器验证.md)

Les rapports 0.1.x décrivent le projet KSA Chinese d’origine et ses compilations historiques. Ils sont conservés pour documenter la provenance, et ne prouvent pas que Babel ait déjà été publié ou inscrit dans un index. Les éléments visuels se limitent à la scène de test consignée, aux changements de langue, aux captures des paramètres et du HUD et à des zones fixes contenant des valeurs numériques ; ils ne constituent pas une validation visuelle exhaustive.

Les contributions doivent préciser la langue, le texte anglais d’origine, le contexte dans le jeu et la formulation proposée. La révision par des locuteurs natifs est particulièrement utile. Le développement et les commits suivent **`developing → pull request → main`**.

## Licence

Code et traductions créées pour le projet : **MIT**, avec conservation des mentions de droits d’auteur de KSA Chinese et de KsaUiLanguages. Police : **SIL OFL 1.1**. Le paquet portable inclut StarMap et Harmony sous leurs licences MIT respectives. Consultez [LICENSE](../../LICENSE), [LICENSES](../../LICENSES) et [THIRD_PARTY](../../THIRD_PARTY.md).

Les ressources des jeux KSA et KSP, leurs dictionnaires originaux, les sources décompilées et les données personnelles ne sont pas distribués. KSP a servi de référence terminologique locale ; son dictionnaire complet n’est ni une donnée d’entrée du produit ni un élément de la version distribuée.
