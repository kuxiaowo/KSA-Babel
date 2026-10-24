"""Refuse stale or incomplete multilingual packages, including old build output."""
from pathlib import Path
import hashlib,json,subprocess,sys

def verify_package_inputs(root):
    subprocess.run([sys.executable,str(root/'scripts/build_multilingual.py'),'--check'],check=True)
    translation=json.loads((root/'reports/multilingual-validation.json').read_text(encoding='utf-8'))
    if translation.get('complete') is not True or translation.get('errors'):
        raise ValueError('Multilingual translation validation is incomplete')
    validation=json.loads((root/'reports/5638-bootstrap-validation.json').read_text(encoding='utf-8'))
    digest=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
    plugin=root/'work/build/KsaBabel/KsaBabel.dll'
    if validation.get('pluginSha256')!=digest(plugin):raise ValueError('Plugin has no matching completed headless validation')
    fingerprints=validation.get('buildInputs',{})
    inputs=[p for p in (root/'src/KsaChinese').glob('*') if p.suffix in ('.cs','.csproj')]
    inputs += [p for p in (root/'src/Launcher').glob('*') if p.suffix in ('.cs','.csproj')]
    inputs += [p for p in (root/'assets/KsaBabel').rglob('*') if p.is_file()]
    expected={p.relative_to(root).as_posix():digest(p) for p in inputs}
    if fingerprints != expected:raise ValueError('Sources/assets changed since successful headless validation')
    launcher_files=validation.get('launcherFiles',{})
    required={'KsaBabelLauncher'+suffix for suffix in ('.exe','.dll','.deps.json','.runtimeconfig.json')}
    if set(launcher_files)!=required:raise ValueError('Launcher has no completed validation fingerprints')
    for name,expected_digest in launcher_files.items():
        launcher=root/'work/build/Launcher'/name
        if not launcher.is_file() or digest(launcher)!=expected_digest:raise ValueError('Stale or unvalidated launcher: '+name)
    for source in (root/'assets/KsaBabel').rglob('*'):
        if not source.is_file():continue
        built=root/'work/build/KsaBabel'/source.relative_to(root/'assets/KsaBabel')
        if not built.is_file() or digest(source)!=digest(built):raise ValueError('Stale built asset: '+source.name)
