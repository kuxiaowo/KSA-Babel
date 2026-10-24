"""Check distributable source inputs and the shipped font without game binaries."""
from pathlib import Path
import argparse
import hashlib
import json
import re
import subprocess
from fontTools.ttLib import TTFont

ROOT = Path(__file__).resolve().parents[1]


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--report', type=Path)
    args = parser.parse_args()
    names = subprocess.check_output(
        ['git', 'ls-files', '--cached', '--others', '--exclude-standard', '-z'], cwd=ROOT
    ).decode('utf-8').split('\0')
    paths = [Path(name) for name in names if name]
    errors = []
    fingerprints = {}
    secret_patterns = [r'gh[pousr]_[A-Za-z0-9]{30,}', r'github_pat_[A-Za-z0-9_]{40,}',
                       r'-----BEGIN (?:RSA |EC |OPENSSH )?PRIVATE KEY-----',
                       r'\bAKIA[A-Z0-9]{16}\b']
    forbidden = {'.dll', '.exe', '.zip', '.dmp', '.sqlite', '.db', '.log', '.cfg'}
    for rel in paths:
        path = ROOT / rel
        if not path.is_file() or path.is_symlink():
            errors.append([rel.as_posix(), 'non-regular file']); continue
        if rel.parts[0] in {'.git', '.tools', 'work', 'dist'} or any(
            part in {'bin', 'obj', '__pycache__', 'saves', 'Profile'} for part in rel.parts
        ) or path.suffix.lower() in forbidden:
            errors.append([rel.as_posix(), 'non-distributable path'])
        data = path.read_bytes()
        if rel.as_posix() != 'reports/publication-audit.json':
            fingerprints[rel.as_posix()] = hashlib.sha256(data).hexdigest()
        if path.suffix.lower() in {'.png', '.ttf'}:
            continue
        try:
            text = data.decode('utf-8-sig')
        except UnicodeDecodeError:
            errors.append([rel.as_posix(), 'unexpected binary']); continue
        if any(re.search(pattern, text) for pattern in secret_patterns):
            errors.append([rel.as_posix(), 'credential pattern; value withheld'])
        if re.search(r'(?i)[A-Z]:[\\/]+Users[\\/]+(?!Public[\\/]|Default[\\/])[^\\/\s`<>]+', text):
            errors.append([rel.as_posix(), 'user-specific absolute path'])
    for notice in ('LICENSE', 'assets/KsaChinese/LICENSE.txt'):
        text = (ROOT / notice).read_text(encoding='utf-8')
        if 'KSA Chinese contributors' not in text or 'KSA Babel contributors' not in text:
            errors.append([notice, 'missing inherited or project copyright'])
    for name in ('KsaUiLanguages-MIT.txt', 'NotoSansSC-OFL.txt', 'StarMap-MIT.txt', 'Harmony-MIT.txt'):
        if not (ROOT / 'LICENSES' / name).is_file():
            errors.append(['LICENSES/' + name, 'missing license'])
    font_path = ROOT / 'assets/KsaChinese/Fonts/KsaUiNotoSC.ttf'
    with TTFont(font_path) as font:
        cmap = set(font.getBestCmap())
    required = set()
    for path in (ROOT / 'assets/KsaChinese/Locales').glob('*.json'):
        pack = json.loads(path.read_text(encoding='utf-8'))
        texts = [pack['displayName']]
        for section in pack.values():
            if isinstance(section, dict):
                texts.extend(section.keys()); texts.extend(section.values())
        required.update(ord(char) for text in texts for char in text if ord(char) >= 32)
    missing = sorted(required - cmap)
    if missing:
        errors.append(['font', 'missing locale codepoints: ' + str(missing)])
    report = dict(scope='Distributable source, common credential patterns, explicit private paths, license presence and shipped locale glyphs; not an OCR or full security audit',
                  errors=errors, sourceFiles=len(paths), fontSha256=hashlib.sha256(font_path.read_bytes()).hexdigest(),
                  requiredLocaleCodepoints=len(required), missingCodepoints=missing, filesSha256=fingerprints)
    if args.report:
        args.report.write_text(json.dumps(report, ensure_ascii=False, indent=2) + '\n', encoding='utf-8')
    print(json.dumps({key: report[key] for key in ('sourceFiles', 'requiredLocaleCodepoints', 'missingCodepoints', 'errors')}, ensure_ascii=False))
    if errors:
        raise SystemExit(1)


if __name__ == '__main__':
    main()
