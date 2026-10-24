"""Local Borea candidate: synthetic CURRENT-source 0.2.0 fixture -> real 0.2.1.
The old fixture is NOT a public historical KsaBabel release. Upstream source is read-only.
"""
from datetime import datetime, timezone
from pathlib import Path
import argparse
import hashlib
import json
import re
import shutil
import subprocess
import sys
import tomllib
import zipfile

root = Path(__file__).resolve().parents[1]
legacy = root.parent / "KSA-CHINESE"
parser = argparse.ArgumentParser(description=__doc__)
parser.add_argument("--index", type=Path, default=root / ".tools/content-index")
parser.add_argument("--releases", type=Path, default=root / ".tools/content-index-releases")
parser.add_argument("--game-dir", type=Path, default=legacy / "work/game-5638-015")
parser.add_argument("--loader-dir", type=Path, default=root / ".tools/StarMap-0.4.8")
parser.add_argument("--loader-archive", type=Path, default=legacy / ".tools/StarMap-0.4.8.zip")
parser.add_argument("--archive", type=Path, default=root / "dist/KsaBabel-0.2.1.zip")
options = parser.parse_args()
index, releases = options.index.resolve(), options.releases.resolve()
candidate = root / "work/ecosystem-candidate"
candidate.mkdir(parents=True, exist_ok=True)


def digest(path):
    return hashlib.sha256(path.read_bytes()).hexdigest()


def fresh(name):
    path = candidate / name
    if path.is_symlink() or path.resolve().parent != candidate.resolve():
        raise ValueError("Fixture directories must remain directly inside ignored candidate work")
    if path.exists():
        shutil.rmtree(path)
    path.mkdir()
    return path


archive = options.archive.resolve()
for required in (index / "tools/check_schema.py", releases / "tools/stamp_release.py",
                 options.loader_archive, archive, options.game_dir / "KSA.dll"):
    if not required.is_file():
        raise FileNotFoundError(required)
fixture_output, fixture_obj = fresh("fixture-build"), fresh("fixture-obj")
build_command = ["dotnet", "build", str(root / "src/KsaChinese/KsaChinese.csproj"),
                 "-c", "Release", "--nologo", "-p:Version=0.2.0",
                 f"-p:KsaDir={options.game_dir.resolve()}",
                 f"-p:LoaderDir={options.loader_dir.resolve()}",
                 f"-p:BaseIntermediateOutputPath={fixture_obj}\\",
                 f"-p:OutputPath={fixture_output}\\"]
subprocess.run(build_command, cwd=root, check=True)
shutil.copytree(root / "assets/KsaBabel", fixture_output, dirs_exist_ok=True)
manifest = fixture_output / "mod.toml"
manifest.write_text(re.sub(r'^version\s*=\s*"[^"]+"', 'version = "0.2.0"',
                           manifest.read_text(encoding="utf-8"), flags=re.MULTILINE), encoding="utf-8")
previous_archive = candidate / "KsaBabel-0.2.0-LOCAL-SYNTHETIC-FIXTURE.zip"
with zipfile.ZipFile(previous_archive, "w", zipfile.ZIP_DEFLATED) as bundle:
    for file in sorted(fixture_output.rglob("*")):
        if file.is_file():
            bundle.write(file, "KsaBabel/" + file.relative_to(fixture_output).as_posix())
with zipfile.ZipFile(archive) as bundle:
    current_dll = bundle.read("KsaBabel/KsaBabel.dll")
    current_manifest = tomllib.loads(bundle.read("KsaBabel/mod.toml").decode("utf-8-sig"))
if current_manifest["version"] != "0.2.1" or current_manifest["StarMap"]["EntryAssembly"] != "KsaBabel":
    raise ValueError("Current release must identify KsaBabel 0.2.1")
if hashlib.sha256(current_dll).hexdigest() == digest(fixture_output / "KsaBabel.dll"):
    raise ValueError("Synthetic 0.2.0 and candidate 0.2.1 DLL bytes must differ")
sys.path.insert(0, str(index / "tools"))
import check_schema

template = (root / "ecosystem/KsaBabel.toml.in").read_text(encoding="utf-8")
unfilled = tomllib.loads(template)
errors = list(check_schema.validator().iter_errors(unfilled))
if len(errors) != 1 or list(errors[0].path) != ["links", "forums"]:
    raise ValueError("Unfilled template must fail only for pending forum URL: " + str(errors))
if unfilled["id"] != "KsaBabel" or not any(item.get("id") == "KsaChinese" and item.get("kind") == "conflict" for item in unfilled.get("dependencies", [])):
    raise ValueError("KsaBabel listing must explicitly declare KsaChinese conflict")
# Deliberately synthetic forum URL; NEVER publish this fixture metadata.
fixture = template.replace("__FORUM_THREAD_URL__", "https://forums.ahwoo.com/threads/1/")
validator_index = fresh("validator-index")
shutil.copytree(index, validator_index, dirs_exist_ok=True,
                ignore=shutil.ignore_patterns(".git", "__pycache__", ".pytest_cache"))
(validator_index / "listings/KsaBabel.toml").write_text(fixture, encoding="utf-8", newline="\n")
results = []
for script in ("check_layout.py", "check_schema.py", "check_index.py", "check_license.py", "check_status.py", "check_images.py"):
    done = subprocess.run([sys.executable, "tools/" + script], cwd=validator_index,
                          text=True, encoding="utf-8", capture_output=True)
    results.append(dict(check=script, exit_code=done.returncode, output=done.stdout + done.stderr))
    print(script, done.returncode, (done.stdout + done.stderr).strip())
    done.check_returncode()
sys.path.insert(0, str(releases / "tools"))
import stamp_release
import build_snapshot
import hosts

authored, generated = fresh("authored"), fresh("releases")
for name in ("listings", "packs"):
    (authored / name).mkdir()
(authored / "listings/KsaBabel.toml").write_text(fixture, encoding="utf-8")
for listing_id in ("StarMap", "KsaChinese"):
    shutil.copy2(index / f"listings/{listing_id}.toml", authored / f"listings/{listing_id}.toml")
    if (releases / f"releases/{listing_id}").is_dir():
        shutil.copytree(releases / f"releases/{listing_id}", generated / listing_id)
(authored / "index-status.toml").write_text("spec_version = 1\n", encoding="utf-8")
shutil.copy2(index / "tags.toml", authored / "tags.toml")
versions = releases / "game-versions.json"
game_versions = json.loads(versions.read_text(encoding="utf-8"))["versions"]
if not (generated / "KsaChinese").is_dir():
    # The original published-ID archive is used only as planning metadata here.
    old_archive = legacy / "dist/KsaChinese-0.1.6.zip"
    old_url = "https://github.com/kuxiaowo/KSA-CHINESE/releases/download/v0.1.6/KsaChinese-0.1.6.zip"
    old_facts = dict(tag="v0.1.6", release_date="2026-10-23T00:00:00Z", url=old_url,
                     prerelease=False, content_type="application/zip")
    old_document = stamp_release.stamp(tomllib.loads((authored / "listings/KsaChinese.toml").read_text(encoding="utf-8")),
                                       old_facts, old_archive.read_bytes(), game_versions)
    (generated / "KsaChinese").mkdir()
    (generated / "KsaChinese/0.1.6.json").write_text(stamp_release.serialize(old_document), encoding="utf-8")
documents = []
transport_files = {"https://ksamodding.github.io/content-index-releases/v1/index.json": str(candidate / "index.json"),
                   "https://github.com/StarMapLoader/StarMap/releases/download/0.4.8/StarMap-0.4.8.zip": str(options.loader_archive.resolve())}
for version, payload in (("0.2.0", previous_archive), ("0.2.1", archive)):
    url = f"https://github.com/kuxiaowo/KSA-Babel/releases/download/v{version}/KsaBabel-{version}.zip"
    facts = dict(tag="v" + version, release_date=datetime.now(timezone.utc).isoformat().replace("+00:00", "Z"),
                 url=url, prerelease=True, content_type="application/zip",
                 changelog=f"https://github.com/kuxiaowo/KSA-Babel/releases/tag/v{version}")
    document = stamp_release.stamp(tomllib.loads(fixture), facts, payload.read_bytes(), game_versions)
    if document["install"]["root"] != "KsaBabel" or document["loader"]["min"] != "0.4.8":
        raise ValueError("Unexpected derived installation/loader")
    (generated / "KsaBabel").mkdir(exist_ok=True)
    (generated / f"KsaBabel/{version}.json").write_text(stamp_release.serialize(document), encoding="utf-8")
    documents.append(document)
    transport_files[url] = str(payload)
asset, _ = hosts.GitHubHost("kuxiaowo/KSA-Babel", None, listing_id="KsaBabel")._asset({
    "tag_name": "v0.2.1", "assets": [{"name": name, "state": "uploaded"} for name in ("KsaBabel-0.2.1-5638-Portable.zip", archive.name)]})
if asset is None or asset["name"] != archive.name:
    raise ValueError("The watcher did not select the standard mod archive")
snapshot = build_snapshot.build(authored, generated, versions)
(candidate / "index.json").write_text(build_snapshot.serialize(snapshot), encoding="utf-8")
# Verify that the current listing's since bound did not hide the synthetic old release.
if '"0.2.0"' not in build_snapshot.serialize(snapshot) or '"0.2.1"' not in build_snapshot.serialize(snapshot):
    raise ValueError("Both versioned fixtures must appear in the local snapshot")
fixture_note = "LOCAL SYNTHETIC 0.2.0 versioned build of current source; never publicly released under KsaBabel ID; validates Borea version upgrade mechanics, not historical code migration"
report = dict(upstream_checks=results, install=documents[-1]["install"], loader=documents[-1]["loader"],
              previous_fixture=fixture_note, previous_build_command=build_command,
              previous_archive=str(previous_archive), previous_archive_sha256=digest(previous_archive),
              previous_dll_sha256=digest(fixture_output / "KsaBabel.dll"), current_archive=str(archive),
              current_archive_sha256=digest(archive), current_dll_sha256=hashlib.sha256(current_dll).hexdigest(),
              transport_files=transport_files, standard_archive_selected_over_portable=True,
              real_forum_url_pending=True, fixture_forum_url_used_only_locally=True,
              final_listing_schema_passed=False, public_index_submitted=False,
              dependencies_read_only=[str(index), str(releases)])
(candidate / "fixture-manifest.json").write_text(json.dumps(report, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
public_report = {key: value for key, value in report.items() if key != "transport_files"}
public_report["release_channel"] = "testing; both synthetic 0.2.0 and candidate 0.2.1 are prereleases"
public_report["dependency_commits"] = {
    name: subprocess.check_output(["git", "-C", str(path), "rev-parse", "HEAD"], text=True).strip()
    for name, path in (("content-index", index), ("content-index-releases", releases))}
public_text = json.dumps(public_report, ensure_ascii=False, indent=2)
# Replace both JSON-escaped and literal spellings for portable public evidence.
for actual, replacement in ((str(root), "<project>"), (str(legacy), "<legacy-project>")):
    public_text = public_text.replace(json.dumps(actual)[1:-1], replacement).replace(actual, replacement)
(root / "reports/ecosystem-metadata-validation.json").write_text(public_text + "\n", encoding="utf-8")
print("Local candidate ready; synthetic old version and forum URL are NOT public releases/publication.")
