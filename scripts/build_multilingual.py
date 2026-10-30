"""Build independently authored KSA locale packs from per-record decisions.

KSP dictionaries are reference-only and are never an input to this pack builder.
The existing reviewed Chinese corpus defines scope, not full-game coverage.
"""
from pathlib import Path
import argparse
import collections
import hashlib
import json
import re

ROOT = Path(__file__).resolve().parents[1]
LOCALES = {
    'ja-JP': '日本語', 'ru-RU': 'Русский', 'de-DE': 'Deutsch',
    'fr-FR': 'Français', 'it-IT': 'Italiano', 'es-ES': 'Español',
    'pt-BR': 'Português (Brasil)',
}
BRACES = re.compile(r'\{[^{}]+\}')
PRINTF = re.compile(r'%(?:[-+ #0]*\d*(?:\.\d+)?(?:ll|l|h)?[diuoxXfFeEgGaAcsp])')
MARKUP = re.compile(r'<[^>]*>')

def main():
    parser = argparse.ArgumentParser()
    parser.add_argument('--allow-incomplete', action='store_true',
                        help='Write coverage report only; never emit incomplete runtime packs.')
    parser.add_argument('--check', action='store_true', help='Check all generated packs against decisions without rewriting them.')
    args = parser.parse_args()
    game_version = json.loads((ROOT/'assets/KsaBabel/compatibility.json').read_text(encoding='utf-8'))['version']
    game_build = game_version.rsplit('.', 1)[-1]
    source = [json.loads(line) for line in (ROOT/f'translations/reviewed-{game_build}.jsonl').read_text(encoding='utf-8').splitlines()]
    source = {row['id']: row for row in source if row['review_status'] == 'reviewed'}
    sections = list(json.loads((ROOT/'assets/KsaBabel/Locales/zh-CN.json').read_text(encoding='utf-8')))
    sections = [s for s in sections if s not in ('locale','displayName','fontFile','dynamic')]
    dynamic = json.loads((ROOT/'translations/multilingual/dynamic.json').read_text(encoding='utf-8'))['entries']
    report = {'source_entries': len(source), 'source_game': game_version,
              'meaning': 'Record completeness against reviewed Chinese scope; not full-game or native-speaker acceptance.',
              'locales': {}, 'errors': []}
    packs = {}
    for locale, name in LOCALES.items():
        decisions = {}
        for path in sorted((ROOT/'translations/multilingual'/locale).glob('*.jsonl')):
            for n, line in enumerate(path.read_text(encoding='utf-8-sig').splitlines(), 1):
                if not line.strip(): continue
                row = json.loads(line)
                if row['id'] in decisions:
                    raise ValueError(f'Duplicate decision: {locale} {row["id"]} ({path.name}:{n})')
                decisions[row['id']] = row
        unknown = sorted(set(decisions)-set(source))
        missing = sorted(set(source)-set(decisions))
        pack = {'locale':locale, 'displayName':name, 'fontFile':'KsaUiNotoSC.ttf'}
        pack.update({s:{} for s in sections})
        pack['dynamic'] = {key:values[locale] for key,values in dynamic.items()}
        for key, values in dynamic.items():
            if collections.Counter(BRACES.findall(values['en-US'])) != collections.Counter(BRACES.findall(values[locale])):
                report['errors'].append([locale,key,'dynamic placeholders'])
        identical = []
        by_section = collections.Counter()
        for ident, row in decisions.items():
            if ident not in source: continue
            original = source[ident]
            value = row['translation']
            if not isinstance(value,str) or not value.strip():
                report['errors'].append([locale,ident,'empty translation']); continue
            if not row.get('origin') or row.get('review_status') not in ('ai-reviewed','reviewed'):
                report['errors'].append([locale,ident,'missing provenance/review status'])
            en = original['english']
            if original['section'] != 'parts':
                patterns = [('placeholders',BRACES),('markup',MARKUP)]
                if original['section'] != 'history': patterns.append(('printf',PRINTF))
                for label,pattern in patterns:
                    expected_tokens, actual_tokens = pattern.findall(en), pattern.findall(value)
                    if (expected_tokens != actual_tokens if label=='markup' else
                        collections.Counter(expected_tokens) != collections.Counter(actual_tokens)):
                        report['errors'].append([locale,ident,label])
                if en.endswith(': ') and not value.endswith(': '):
                    report['errors'].append([locale,ident,'ASCII colon-space layout delimiter'])
            if value == en: identical.append(ident)
            pack[original['section']][original['key']] = value
            by_section[original['section']] += 1
        report['locales'][locale] = {'decisions':len(decisions),'missing':missing,'unknown':unknown,
            'sections':dict(by_section),'identical_to_english':identical,
            'review_status':'AI reviewed; awaiting independent native-language and visual review'}
        if unknown: report['errors'].append([locale,'unknown source IDs',unknown])
        packs[locale] = pack
    report_path = ROOT/'reports/multilingual-validation.json'
    report_path.write_text(json.dumps(report,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
    complete = all(not r['missing'] and not r['unknown'] for r in report['locales'].values())
    if report['errors'] or not complete:
        print(json.dumps({'complete':False,'counts':{k:v['decisions'] for k,v in report['locales'].items()},'errors':len(report['errors'])}))
        if not args.allow_incomplete: raise SystemExit('Incomplete or invalid decisions: inspect reports/multilingual-validation.json')
        return
    for locale, pack in packs.items():
        path = ROOT/f'assets/KsaBabel/Locales/{locale}.json'
        content=json.dumps(pack,ensure_ascii=False,indent=2)+'\n'
        if args.check:
            if not path.is_file() or path.read_text(encoding='utf-8')!=content:
                raise SystemExit('Stale generated locale: '+locale)
        else: path.write_text(content,encoding='utf-8')
        report['locales'][locale]['runtime_sha256'] = hashlib.sha256(path.read_bytes()).hexdigest()
    for locale in ('en-US','zh-CN'):
        path = ROOT/f'assets/KsaBabel/Locales/{locale}.json'
        pack = json.loads(path.read_text(encoding='utf-8'))
        expected_dynamic = {key:values[locale] for key,values in dynamic.items()}
        if args.check:
            if pack.get('dynamic')!=expected_dynamic:raise SystemExit('Stale dynamic templates: '+locale)
        else:
            pack['dynamic']=expected_dynamic
            path.write_text(json.dumps(pack,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
    report['complete'] = True
    report_path.write_text(json.dumps(report,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
    print(json.dumps({'complete':True,'locales':len(packs),'entries_per_locale':len(source)}))

if __name__ == '__main__': main()
