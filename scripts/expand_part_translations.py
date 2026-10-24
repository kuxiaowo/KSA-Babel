"""Expand reviewed whole-caption templates, preserving each model designation.

Templates apply only to the enumerated part-name records, never arbitrary UI.
Translation grammar and placeholder order are explicit per language.
"""
from pathlib import Path
import collections
import csv
import json
import re

ROOT = Path(__file__).resolve().parents[1]
MODEL = re.compile(r'[A-Za-z0-9][A-Za-z0-9.\-]*')
with (ROOT/'translations/multilingual/part-templates.tsv').open(encoding='utf-8',newline='') as stream:
    records=list(csv.DictReader(stream,delimiter='|'))
templates={row['source']:row for row in records}
if len(records)!=len(templates):raise ValueError('Duplicate part template')
locales=[x for x in records[0] if x!='source']
source=[json.loads(line) for line in (ROOT/'translations/reviewed-5638.jsonl').read_text(encoding='utf-8').splitlines()]
parts=[r for r in source if r['section']=='parts' and r['review_status']=='reviewed']
output={locale:[] for locale in locales}
used=set()
for row in parts:
    values=MODEL.findall(row['chinese'])
    index=[0]
    def slot(match):
        result='{'+str(index[0])+'}';index[0]+=1;return result
    template=MODEL.sub(slot,row['chinese'])
    if template not in templates:raise ValueError('Untranslated part template: '+template)
    used.add(template)
    for locale in locales:
        translated_template=templates[template][locale]
        if collections.Counter(re.findall(r'\{\d+\}',template)) != collections.Counter(re.findall(r'\{\d+\}',translated_template)):
            raise ValueError('Model-slot mismatch: '+locale+' '+template)
        translation=translated_template.format(*values)
        for value,count in collections.Counter(values).items():
            if translation.count(value)<count:raise ValueError('Lost model designation: '+value)
        output[locale].append({'id':row['id'],'translation':translation,
          'origin':'Independent KSA part-caption template translation 2026-10-24',
          'review_status':'ai-reviewed','notes':'Explicit whole-caption template in part-templates.tsv; retain model/designation slots. Native-language visual review pending.'})
for locale, rows in output.items():
    path=ROOT/'translations/multilingual'/locale/'parts.jsonl';path.parent.mkdir(parents=True,exist_ok=True)
    path.write_text(''.join(json.dumps(r,ensure_ascii=False)+'\n' for r in rows),encoding='utf-8')
print(json.dumps({'templates':len(templates),'used_templates':len(used),'parts_per_locale':len(parts),'locales':locales}))
