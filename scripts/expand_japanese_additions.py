"""Add independent current-build Japanese captions beside MIT upstream seeds."""
from pathlib import Path
import json

ROOT=Path(__file__).resolve().parents[1]
source=[json.loads(s) for s in (ROOT/'translations/reviewed-5638.jsonl').read_text(encoding='utf-8').splitlines()]
source=[r for r in source if r['review_status']=='reviewed' and r['section'] not in ('parts','content','history','display')]
path=ROOT/'translations/multilingual/ja-JP/core.jsonl'
rows=[json.loads(s) for s in path.read_text(encoding='utf-8').splitlines()]
rows=[r for r in rows if r.get('origin')!='Independent KSA Japanese additions 2026-10-24']
existing={r['id'] for r in rows}
additions=json.loads((ROOT/'translations/multilingual/ja-core-additions.json').read_text(encoding='utf-8'))
for row in source:
    if row['id'] in existing:continue
    if row['english'] not in additions:raise ValueError('Missing Japanese addition: '+row['english'])
    rows.append({'id':row['id'],'translation':additions[row['english']],
      'origin':'Independent KSA Japanese additions 2026-10-24','review_status':'ai-reviewed',
      'notes':'Reviewed against current English, Chinese and physical/UI context; native-language visual review pending.'})
path.write_text(''.join(json.dumps(r,ensure_ascii=False)+'\n' for r in rows),encoding='utf-8')
print('Japanese core records:',len(rows))
