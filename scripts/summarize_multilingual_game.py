"""Collect bounded real-game evidence; never treat an exit code as UI acceptance."""
from pathlib import Path
import argparse
import hashlib
import json
import re
from PIL import Image

ROOT = Path(__file__).resolve().parents[1]
LOCALES = ['en-US', 'ja-JP', 'ru-RU', 'de-DE', 'fr-FR', 'it-IT', 'es-ES', 'pt-BR', 'zh-CN']
NUMERIC_REGIONS = {'dose': (1705, 162, 1874, 187), 'apoapsis': (1705, 383, 1874, 408),
                   'deltaV': (1005, 978, 1098, 994)}


def digest(path):
    return hashlib.sha256(path.read_bytes()).hexdigest()


def collect(profile, tag, game):
    profile = profile.resolve()
    profile.relative_to(ROOT / 'work')
    if 'test-profile' not in profile.name or not re.fullmatch(r'[a-zA-Z0-9-]+', tag):
        raise ValueError('Only workspace test profiles and simple log tags are supported')
    log = profile / 'smoke.log'
    text = log.read_text(encoding='utf-8')
    result = json.loads((profile / 'multilingual-runtime.json').read_text(encoding='utf-8'))
    if 'ERROR=' in text or 'multilingual-completed; locales=9' not in text:
        raise ValueError('Game did not complete the nine-language fixture')
    if [r['locale'] for r in result['results']] != LOCALES:
        raise ValueError('Unexpected locale results')
    if re.findall(r'screenshot-request=multilingual-([^\s]+)', text) != LOCALES:
        raise ValueError('Missing or duplicate screenshot requests')
    caption_calls = re.findall(r'gauge-caption-passes=([^;]+); count=(\d+)', text)
    if [locale for locale, count in caption_calls] != LOCALES or any(int(count) <= 0 for _, count in caption_calls):
        raise ValueError('Native gauge caption renderer was not exercised in every locale')
    exit_path = ROOT / f'work/runtime/{tag}.exit-code.txt'
    if exit_path.read_text().strip() != '0':
        raise ValueError('Game exit code was not zero')
    stderr = ROOT / f'work/runtime/{tag}.stderr.log'
    if stderr.read_text(encoding='utf-8').strip():
        raise ValueError('Game stderr requires review')
    screenshots = sorted((profile / 'exports/screenshots').glob('*.png'))
    if len(screenshots) != 9:
        raise ValueError('Expected exactly nine actual screenshots')
    build = json.loads((ROOT / 'reports/5638-bootstrap-validation.json').read_text(encoding='utf-8'))
    compatibility = json.loads((ROOT / 'assets/KsaBabel/compatibility.json').read_text(encoding='utf-8'))
    checked_files = {name: digest(game / name) for name in compatibility['files']}
    if checked_files != compatibility['files']:
        raise ValueError('Installed game identity changed')
    records = []
    numeric_reference = None
    for row, screenshot in zip(result['results'], screenshots):
        plugin = Path(row['plugin'])
        expected = profile / 'mods/KsaBabel/KsaBabel.dll'
        if plugin.resolve() != expected.resolve() or digest(plugin) != build['pluginSha256']:
            raise ValueError('Game loaded a stale or shadowing plugin')
        if row['selected'] != row['locale'] or row['actualGameVersion'] != compatibility['version']:
            raise ValueError('Locale selection or game version mismatch')
        with Image.open(screenshot) as source:
            if source.size != (1920, 1080):
                raise ValueError('Fixed numeric regions require 1920x1080 screenshots')
            pixels = source.convert('RGB')
            numeric_hashes = {name: hashlib.sha256(pixels.crop(box).tobytes()).hexdigest()
                              for name, box in NUMERIC_REGIONS.items()}
            for box in NUMERIC_REGIONS.values():
                if sum(min(rgb) > 125 for rgb in pixels.crop(box).getdata()) < 100:
                    raise ValueError('Numeric screenshot region appears empty')
        if numeric_reference is None:
            numeric_reference = numeric_hashes
        if numeric_hashes != numeric_reference:
            raise ValueError('Paused numeric screenshot regions changed with locale')
        row = dict(row, plugin=plugin.relative_to(ROOT).as_posix(), numericPixelSha256=numeric_hashes,
                   screenshot=screenshot.relative_to(ROOT).as_posix(), screenshotSha256=digest(screenshot))
        records.append(row)
    return dict(gameVersion=compatibility['version'], pluginSha256=build['pluginSha256'],
                installedGameFiles=checked_files, profile=profile.relative_to(ROOT).as_posix(),
                exitCode=0, stderrEmpty=True, completedLocales=9, actualScreenshots=9,
                nativeGaugeCaptionPasses={locale: int(count) for locale, count in caption_calls},
                numericRegions=NUMERIC_REGIONS, numericRegionsMatchEnglishPixels=True,
                log=log.relative_to(ROOT).as_posix(), logSha256=digest(log), results=records,
                scope='Real loaded game and display-settings/HUD screenshots at 1920x1080; not exhaustive window, resolution or native-speaker acceptance',
                visualReview='No fresh manual visual sign-off; native text outputs, font selection, screenshots and fixed numeric regions checked')


if __name__ == '__main__':
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--profile', type=Path, required=True)
    parser.add_argument('--tag', required=True)
    parser.add_argument('--game-dir', type=Path, required=True)
    args = parser.parse_args()
    report = collect(args.profile, args.tag, args.game_dir)
    (ROOT / 'reports/multilingual-game-validation.json').write_text(
        json.dumps(report, ensure_ascii=False, indent=2) + '\n', encoding='utf-8')
    print(json.dumps({k: report[k] for k in ['gameVersion', 'completedLocales', 'actualScreenshots', 'exitCode']}))
