# KSA Babel · 喵语塔

[English](../../README.md) · [简体中文](README.zh-CN.md) · [日本語](README.ja-JP.md) · [Русский](README.ru-RU.md) · [Deutsch](README.de-DE.md) · [Français](README.fr-FR.md) · [Italiano](README.it-IT.md) · [Español](README.es-ES.md) · [Português (Brasil)](README.pt-BR.md)

Un marco de localización multilingüe para **Kitten Space Agency**, cargado mediante StarMap. KSA Babel proporciona paquetes de idiomas, un menú de idiomas dentro del juego, adaptadores de visualización y una fuente compartida con un subconjunto de caracteres. No sustituye el ejecutable ni los recursos del juego.

KSA Babel es un proyecto multilingüe independiente derivado de [KSA-CHINESE](https://github.com/kuxiaowo/KSA-CHINESE). La versión actual es la **preliminar 0.2.0** y solo es compatible con **KSA 2026.10.19.5638 para Windows x64**, con los hashes de archivo comprobados. Utiliza **StarMap 0.4.8** como cargador.

## Idiomas y alcance

English · 简体中文 · 日本語 · Русский · Deutsch · Français · Italiano · Español · Português (Brasil)

Cada uno de los siete idiomas adicionales cubre **4.786 registros** del alcance ya revisado en chino simplificado, más **11 plantillas dinámicas**. El inglés utiliza el texto original del juego. La integridad de los registros **no equivale a traducir todo el juego**. Las nuevas traducciones han pasado una revisión contextual con IA, **no una validación independiente por hablantes nativos**. Los registros de cambios más antiguos, textos de bibliotecas nativas y errores, letras en texturas y algunos estados de la interfaz quedan fuera del alcance revisado. Los controles estrechos y otras resoluciones todavía requieren una revisión visual más amplia.

El alcance de esta versión incluye la interfaz, las ayudas, las piezas, las sustancias y sus fases, los instrumentos y parte del historial de cambios. Se conservan los modelos, las unidades, los parámetros de sustitución, los ID de recursos, controles y partidas, los datos de enumeraciones y los nombres personalizados. Las versiones desconocidas se rechazan de forma segura y los textos sin traducción vuelven al original. El subconjunto de Noto Sans SC cubre los caracteres inventariados, pero no garantiza todos los nombres que pueda introducir el jugador ni todas las formas regionales de los caracteres japoneses.

## Instalación

Descarga los archivos de [KSA Babel Releases](https://github.com/kuxiaowo/KSA-Babel/releases). Los paquetes 0.2.0 son versiones preliminares.

- **Con StarMap/Borea ya instalado:** utiliza `KsaChinese-0.2.0.zip`; coloca su carpeta `KsaChinese` en el directorio `mods` del perfil seleccionado y activa el mod. Consulta la [instalación estándar](../../README-生态安装.md).
- **Con lanzador incluido:** extrae `KsaChinese-0.2.0-5638-Portable.zip` en la carpeta que contiene `KSA.exe` y ejecuta `KsaChinese/KsaChineseLauncher.exe`. Consulta la [instalación portátil](../../README-便携Mod.md). Requiere .NET 10 x64 Runtime.

El ID interno del mod, el ensamblado, el directorio de configuración y el prefijo de los archivos siguen siendo **`KsaChinese`** por compatibilidad. **Activa una sola copia:** no instales KSA Babel junto con KSA Chinese ni conserves a la vez `Content/KsaChinese` y `mods/KsaChinese` del perfil. Haz una copia de seguridad de los perfiles existentes antes de migrar. No se incluyen partidas personales ni se trasladan automáticamente.

Este nuevo proyecto todavía no se ha presentado para su inclusión independiente en el catálogo público de Borea. La entrada KsaChinese y la versión v0.1.6 del catálogo anterior pertenecen al proyecto original y no son una vía de descarga de Babel. Instala manualmente desde las versiones publicadas de este repositorio. El proyecto no ha enviado mensajes a los foros ni a los responsables del índice.

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

La fuente generada ya está incluida. Para regenerarla hacen falta fontTools y `.tools/NotoSansSC-variable.ttf` con el SHA-256 indicado en [FONT-SOURCE](../../assets/KsaChinese/FONT-SOURCE.txt); los cambios del archivo fuente requieren revisión. La identidad de la descarga de StarMap y las licencias de terceros constan en [THIRD_PARTY](../../THIRD_PARTY.md).

## Evidencias y contribuciones

- [Alcance y verificación de los nueve idiomas](../../reports/0.2.0-九语种翻译与验证.md)
- [Origen del proyecto y huellas de los archivos importados](../../reports/project-origin.json)
- [Notas de migración y publicación](../../reports/KSA-Babel-项目迁移与公开验证.md)

Los informes de 0.1.x describen el proyecto original KSA Chinese y compilaciones históricas. Se conservan para documentar la procedencia, no como prueba de publicaciones o inclusiones anteriores de Babel en catálogos. Las evidencias de la interfaz gráfica se limitan a la escena de prueba registrada, los cambios de idioma, las capturas de ajustes/HUD y regiones fijas de la imagen con valores numéricos; no constituyen una validación visual exhaustiva.

Las contribuciones deben identificar el idioma, el texto original en inglés, el contexto dentro del juego y la redacción propuesta. La revisión por hablantes nativos resulta especialmente útil. El desarrollo y los commits siguen el flujo **`developing → pull request → main`**.

## Licencia

Código y traducciones de autoría propia: **MIT**, conservando los avisos de derechos de autor de KSA Chinese y KsaUiLanguages. Fuente: **SIL OFL 1.1**. El paquete portátil incluye StarMap y Harmony bajo sus licencias MIT. Consulta [LICENSE](../../LICENSE), [LICENSES](../../LICENSES) y [THIRD_PARTY](../../THIRD_PARTY.md).

No se distribuyen recursos de KSA o KSP, diccionarios originales, código descompilado ni datos personales. KSP se utilizó como referencia terminológica local; su diccionario completo no es un dato de entrada del producto ni forma parte de la distribución.
