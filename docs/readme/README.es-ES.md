# KSA Babel · 喵语塔

[English](../../README.md) · [简体中文](README.zh-CN.md) · [日本語](README.ja-JP.md) · [Русский](README.ru-RU.md) · [Deutsch](README.de-DE.md) · [Français](README.fr-FR.md) · [Italiano](README.it-IT.md) · [Español](README.es-ES.md) · [Português (Brasil)](README.pt-BR.md)

<p align="center"><img src="../branding/ksa-babel-cover-preview-v3.png" alt="KSA Babel · 喵语塔" width="512"></p>

Un marco de localización multilingüe para **Kitten Space Agency**, cargado mediante StarMap. KSA Babel proporciona paquetes de idiomas, un menú de idiomas dentro del juego, adaptadores de visualización y una fuente compartida con un subconjunto de caracteres. No sustituye el ejecutable ni los recursos del juego.

KSA Babel es un proyecto multilingüe independiente derivado de [KSA-CHINESE](https://github.com/kuxiaowo/KSA-CHINESE). La versión actual es la **versión estable 0.2.2** y solo es compatible con **KSA 2026.10.19.5638 para Windows x64**, con los hashes de archivo comprobados. Utiliza **StarMap 0.4.8** como cargador.

**Revisión y modo inglés:** el autor solo ha confirmado personalmente la traducción al chino simplificado. Japonés, ruso, alemán, francés, italiano, español y portugués brasileño fueron traducidos y revisados por GPT en el contexto de KSA, tomando como referencia la terminología aeroespacial de KSP; todavía falta una revisión independiente de hablantes nativos. Parte de las traducciones japonesas iniciales procede de KsaUiLanguages bajo MIT, con los créditos conservados. El modo inglés muestra el texto inglés original de KSA y mantiene el menú `Language` añadido por el mod. Las 11 plantillas dinámicas sirven para componer texto multilingüe y volver al original, no significan 11 textos ingleses reescritos.

**Edición china:** [KSA Chinese / KSA-CHINESE](https://github.com/kuxiaowo/KSA-CHINESE), ID `KsaChinese`, permite elegir inglés o chino simplificado. La versión actual es [v0.1.6](https://github.com/kuxiaowo/KSA-CHINESE/releases/tag/v0.1.6), archivo estándar `KsaChinese-0.1.6.zip`, para los archivos verificados de KSA 2026.10.19.5638 en Windows x64 y StarMap 0.4.8. Tiene su propia [entrada Borea](https://github.com/KSAModding/content-index/blob/main/listings/KsaChinese.toml). Babel es la mod independiente con nueve idiomas e ID `KsaBabel`. No actives ambos mods en el mismo perfil.

## Idiomas y alcance

English · 简体中文 · 日本語 · Русский · Deutsch · Français · Italiano · Español · Português (Brasil)

Cada uno de los siete idiomas adicionales cubre **4.786 registros** del alcance ya revisado en chino simplificado, más **11 plantillas dinámicas**. El inglés utiliza el texto original del juego. La integridad de los registros **no equivale a traducir todo el juego**. Las nuevas traducciones han pasado una revisión contextual con IA, **no una validación independiente por hablantes nativos**. Los registros de cambios más antiguos, textos de bibliotecas nativas y errores, letras en texturas y algunos estados de la interfaz quedan fuera del alcance revisado. Los controles estrechos y otras resoluciones todavía requieren una revisión visual más amplia.

El alcance de esta versión incluye la interfaz, las ayudas, las piezas, las sustancias y sus fases, los instrumentos y parte del historial de cambios. Se conservan los modelos, las unidades, los parámetros de sustitución, los ID de recursos, controles y partidas, los datos de enumeraciones y los nombres personalizados. Las versiones desconocidas se rechazan de forma segura y los textos sin traducción vuelven al original. El subconjunto de Noto Sans SC cubre los caracteres inventariados, pero no garantiza todos los nombres que pueda introducir el jugador ni todas las formas regionales de los caracteres japoneses.

## Instalación

Descarga los archivos de [KSA Babel Releases](https://github.com/kuxiaowo/KSA-Babel/releases). Los paquetes 0.2.2 son versiones estables.

- **Con StarMap/Borea ya instalado:** utiliza `KsaBabel-0.2.2.zip`; coloca su carpeta `KsaBabel` en el directorio `mods` del perfil seleccionado y activa el mod. Consulta la [instalación estándar](../../README-生态安装.md).
- **Con lanzador incluido:** extrae `KsaBabel-0.2.2-5638-Portable.zip` en la carpeta que contiene `KSA.exe` y ejecuta `KsaBabel/KsaBabelLauncher.exe`. Consulta la [instalación portátil](../../README-便携Mod.md). Requiere .NET 10 x64 Runtime.

A partir de 0.2.1, el ID del mod, los directorios de instalación y el prefijo de los archivos son **`KsaBabel`**; el ensamblado del complemento es `KsaBabel.dll` y el lanzador portátil es `KsaBabel/KsaBabelLauncher.exe`. El idioma se guarda en `KsaBabel/settings.json` del perfil actual. En el primer inicio, solo si ese archivo nuevo no existe, Babel importa un valor `locale` válido del antiguo `KsaChinese/settings.json` del mismo perfil. No se sobrescriben las nuevas preferencias existentes ni se modifica el archivo antiguo.

**Desactiva primero el mod antiguo:** Babel rechaza la activación mientras `KsaChinese` esté activado. Mantén una sola copia activa de Babel y no conserves a la vez `Content/KsaBabel` y `mods/KsaBabel` del perfil. Haz una copia de seguridad de los perfiles existentes antes de migrar. No se incluyen partidas personales ni se trasladan automáticamente; la importación de preferencias no busca en otros perfiles.

[El anuncio en Ahwoo](https://forums.ahwoo.com/forums/kitten-space-agency/mod-releases/ksa-babel-0-2-1-nine-language-localization-preview-for-ksa-5638.1454/) está publicado y la [PR #227 del catálogo de Borea](https://github.com/KSAModding/content-index/pull/227) se ha integrado. Consulta el [registro de publicación](../../reports/community-publication-20261024.md) para conocer el estado del procesamiento de la versión y de la verificación de la instantánea pública. La entrada KsaChinese existente y la versión v0.1.6 pertenecen al proyecto independiente dedicado al chino. KSA Babel 0.2.2 es una versión estable, disponible en el canal stable predeterminado. Las siete traducciones adicionales siguen pendientes de revisión independiente por hablantes nativos.

Para migrar mediante Borea, haz una copia del perfil y elimina la antigua mod KsaChinese de la instancia antes de instalar Babel. El conflicto del catálogo impide la instalación aunque la mod anterior esté desactivada. Conserva los datos del perfil.

## Compilación y validación

Utiliza .NET 10 SDK, una copia instalada legalmente de la versión exacta del juego admitida y [StarMap 0.4.8](https://github.com/StarMapLoader/StarMap/releases/tag/0.4.8). Coloca los archivos del cargador en `.tools/StarMap-0.4.8` o especifica `-LoaderDir`. Los scripts de Python requieren Python 3; se recomienda conda.

```powershell
python scripts/build_multilingual.py --check
.\scripts\build.ps1 -GameDir 'D:\Games\Kitten Space Agency' -Validate
python scripts/package_mod.py
python scripts/package_portable.py
```

Puedes indicar la ruta local del juego mediante `KSA_GAME_DIR` en lugar de `-GameDir`. Los binarios del juego, `.tools`, los perfiles locales y los resultados de compilación están excluidos del control de versiones. Una copia recién clonada no contiene el juego, el cargador, el descompilador, los datos de entrada de las auditorías ni los diccionarios de referencia de KSP. Los scripts opcionales de auditorías históricas necesitan sus datos locales preparados por separado.

Las decisiones de traducción están en `translations/multilingual/<locale>/*.jsonl`. `build_multilingual.py` comprueba los ID, la procedencia, el estado de revisión, los parámetros de sustitución y el marcado frente a la base revisada; rechaza los paquetes incompletos o desactualizados. El empaquetado también exige una validación local de compilación correcta y correspondiente a fuentes y recursos sin cambios posteriores. La integración continua comprueba los datos portables y la fuente; no puede compilar contra una instalación del juego con licencia ni validar la experiencia de juego.

La fuente generada ya está incluida. Para regenerarla hacen falta fontTools y `.tools/NotoSansSC-variable.ttf` con el SHA-256 indicado en [FONT-SOURCE](../../assets/KsaBabel/FONT-SOURCE.txt); los cambios del archivo fuente requieren revisión. La identidad de la descarga de StarMap y las licencias de terceros constan en [THIRD_PARTY](../../THIRD_PARTY.md).

## Evidencias y contribuciones

- [Alcance y verificación de los nueve idiomas](../../reports/0.2.0-九语种翻译与验证.md)
- [Origen del proyecto y huellas de los archivos importados](../../reports/project-origin.json)
- [Notas de migración y publicación](../../reports/KSA-Babel-项目迁移与公开验证.md)
- [Verificación del ID y del gestor 0.2.1](../../reports/0.2.1-独立ID与管理器验证.md)

Los informes de 0.1.x describen el proyecto original KSA Chinese y compilaciones históricas. Se conservan para documentar la procedencia, no como prueba de publicaciones o inclusiones anteriores de Babel en catálogos. Las evidencias de la interfaz gráfica se limitan a la escena de prueba registrada, los cambios de idioma, las capturas de ajustes/HUD y regiones fijas de la imagen con valores numéricos; no constituyen una validación visual exhaustiva.

Las contribuciones deben identificar el idioma, el texto original en inglés, el contexto dentro del juego y la redacción propuesta. La revisión por hablantes nativos resulta especialmente útil. El desarrollo y los commits siguen el flujo **`developing → pull request → main`**.

## Licencia

Código y traducciones de autoría propia: **MIT**, conservando los avisos de derechos de autor de KSA Chinese y KsaUiLanguages. Fuente: **SIL OFL 1.1**. El paquete portátil incluye StarMap y Harmony bajo sus licencias MIT. Consulta [LICENSE](../../LICENSE), [LICENSES](../../LICENSES) y [THIRD_PARTY](../../THIRD_PARTY.md).

No se distribuyen recursos de KSA o KSP, diccionarios originales, código descompilado ni datos personales. KSP se utilizó como referencia terminológica local; su diccionario completo no es un dato de entrada del producto ni forma parte de la distribución.
