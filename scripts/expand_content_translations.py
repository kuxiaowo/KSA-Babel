"""Materialize explicit full-sentence material/reaction/grain translations."""
from pathlib import Path
import csv,json

ROOT=Path(__file__).resolve().parents[1]
with (ROOT/'translations/multilingual/content.tsv').open(encoding='utf-8',newline='') as f:
    translations=list(csv.DictReader(f,delimiter='|'))
mapped={r['source']:r for r in translations}
if len(mapped)!=len(translations):raise ValueError('Duplicate content translation')
locales=[k for k in translations[0] if k!='source']
rows=[json.loads(s) for s in (ROOT/'translations/reviewed-5638.jsonl').read_text(encoding='utf-8').splitlines()]
source=[r for r in rows if r['section']=='content' and r['review_status']=='reviewed']
for locale in locales:
    output=[]
    for r in source:
        if r['chinese'] not in mapped:raise ValueError('Missing content translation: '+r['chinese'])
        output.append({'id':r['id'],'translation':mapped[r['chinese']][locale],
          'origin':'Independent KSA material/reaction/grain translation 2026-10-24',
          'review_status':'ai-reviewed','notes':'Full caption/description reviewed against English, Chinese and physical material context; original identifiers retained. Native-language review pending.'})
    path=ROOT/f'translations/multilingual/{locale}/content.jsonl'
    path.parent.mkdir(parents=True,exist_ok=True)
    path.write_text(''.join(json.dumps(r,ensure_ascii=False)+'\n' for r in output),encoding='utf-8')
print('Content:',len(source),'records in each of',len(locales),'languages')
