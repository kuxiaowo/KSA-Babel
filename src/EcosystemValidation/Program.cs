// Local integration runner against unmodified Borea v0.5.1 source components.
// Local candidate transport and a synthetic old version; this is not public catalogue acceptance.
using System.Net;
using System.Security.Cryptography;
using System.Text.Json;
using Borea.Cli;
using Borea.Composition;
using Borea.Core.Game;
using Borea.Core.Instances;
using Borea.Core.Index;
using Borea.Core.Mods;
using Borea.Core.Settings;
using Borea.Core.Planning;
using Borea.Network.Index;
using Borea.Storage.Index;
using Borea.Storage.Paths;
using Borea.Storage.Settings;
using Borea.Core.Launch;
using Borea.Storage.Launch;

if (args.Length is not (3 or 4) || (args.Length == 4 && args[3] != "--management-only"))
    throw new ArgumentException("Usage: EcosystemValidation <project> <fresh work/test-profile-... root> <isolated game copy> [--management-only]");
bool managementOnly = args.Contains("--management-only");
var launchRecorder = new LaunchPlanRecorder();
var project = Path.GetFullPath(args[0]);
var root = Path.GetFullPath(args[1]);
var game = Path.GetFullPath(args[2]);
var workPrefix = Path.Combine(project, "work") + Path.DirectorySeparatorChar;
if (!root.StartsWith(workPrefix, StringComparison.OrdinalIgnoreCase)
    || !root[workPrefix.Length..].Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)[0].StartsWith("test-profile-", StringComparison.OrdinalIgnoreCase)
    || Directory.Exists(Path.Combine(root, "Instances")))
    throw new Exception("Use a fresh workspace work/test-profile-* root.");
for (string? cursor = root; cursor is not null && cursor.Length >= project.Length; cursor = Path.GetDirectoryName(cursor))
    if (Directory.Exists(cursor) && (File.GetAttributes(cursor) & FileAttributes.ReparsePoint) != 0)
        throw new Exception("Fixture profile must not traverse junctions or symbolic links.");
if (!File.Exists(Path.Combine(game, "KSA.dll"))) throw new Exception("Isolated game copy is missing KSA.dll");
Directory.CreateDirectory(root);
var fixturePath = Path.Combine(project, "work/ecosystem-candidate/fixture-manifest.json");
using var fixtureDocument = JsonDocument.Parse(File.ReadAllText(fixturePath));
var fixture = fixtureDocument.RootElement;
var transportFiles = fixture.GetProperty("transport_files").Deserialize<Dictionary<string, string>>()!;
var records = new List<object>();
var checks = new List<string>();
var paths = new GamePathProvider(null, boreaRoot: root);
await new FileBoreaSettingsRepository(paths).SaveAsync(new BoreaSettings(game, releaseChannel: ReleaseChannel.Testing));

async Task<BoreaServices> Graph()
{
    var reader = new ContentIndexReader(new GamePathProvider(null, boreaRoot: root));
    var fallback = new ContentIndexModRepository(new ReaderSnapshotProvider(reader));
    return await BoreaServices.BuildAsync(root, new CandidateTransport(transportFiles), fallback,
        new NoCandidates(), processStarter: managementOnly ? launchRecorder : null,
        sharedProfileRoot: Path.Combine(root, "unused-shared-profile"));
}
async Task Command(params string[] command)
{
    using var output = new StringWriter();
    using var error = new StringWriter();
    int code = await BoreaCli.RunAsync(command, async _ => CliServices.From(await Graph()), output, error);
    records.Add(new { command, code, output = Clean(output.ToString()), error = Clean(error.ToString()) });
    Console.WriteLine(string.Join(' ', command) + ": " + code);
    if (code != 0) throw new Exception(error + "\n" + output);
}
void Check(bool ok, string message)
{
    if (!ok) throw new Exception(message);
    checks.Add(message);
}
string Clean(string value) => value.Replace(project, "<project>", StringComparison.OrdinalIgnoreCase)
    .Replace(game, "<isolated-game>", StringComparison.OrdinalIgnoreCase)
    .Replace(Path.Combine(Directory.GetParent(project)!.FullName, "KSA-CHINESE"), "<legacy-project>", StringComparison.OrdinalIgnoreCase);
string Digest(string path) => Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path))).ToLowerInvariant();
Check(Digest(fixture.GetProperty("current_archive").GetString()!) == fixture.GetProperty("current_archive_sha256").GetString(), "Current release archive still matches prepared candidate");
Check(Digest(fixture.GetProperty("previous_archive").GetString()!) == fixture.GetProperty("previous_archive_sha256").GetString(), "Synthetic previous archive still matches prepared fixture");
await Command("index", "refresh");
await Command("index", "validate", "--json");
await Command("search", "KsaBabel", "--json");
await Command("instance", "create", "Babel A", "--json");
await Command("instance", "create", "Babel B", "--json");
await Command("loader", "install", "StarMap", "--version", "0.4.8");
await Command("install", "KsaBabel", "--instance", "Babel A", "--version", "0.2.0");
await Command("install", "KsaBabel", "--instance", "Babel B", "--version", "0.2.1");

using var graph = await Graph();
var instances = await graph.Instances.GetAllAsync();
var first = instances.Single(i => i.Name == "Babel A");
var second = instances.Single(i => i.Name == "Babel B");
var release = (await graph.OfflineContentIndex.GetReleaseAsync("KsaBabel", ModVersion.Parse("0.2.1")))!;
var cleanPlanTarget = new Instance("Compatibility probe", InstanceSource.Custom.Value);
foreach (var build in new[] { "2026.10.13.5602", "2026.10.19.5639" })
{
    var plan = await graph.InstallPlanner.PlanAsync(new InstallPlanningRequest(cleanPlanTarget,
        [new RequestedMod(release, InstallReason.Manual, true)], graph.OfflineContentIndex,
        GameVersion.Parse(build), OsPlatform.Windows));
    Check(!plan.IsReady, "Borea blocks unsupported game build " + build);
}
foreach (var platform in new[] { OsPlatform.Linux, OsPlatform.MacOs })
{
    var plan = await graph.InstallPlanner.PlanAsync(new InstallPlanningRequest(cleanPlanTarget,
        [new RequestedMod(release, InstallReason.Manual, true)], graph.OfflineContentIndex,
        GameVersion.Parse("2026.10.19.5638"), platform));
    Check(plan.Warnings.Any(warning => warning.Kind == PlanningMessageKind.Platform), "Borea warns about unverified platform " + platform);
}
var oldRelease = (await graph.OfflineContentIndex.GetReleaseAsync("KsaChinese", ModVersion.Parse("0.1.6")))!;
var conflictTarget = new Instance("Old-ID planning probe", InstanceSource.Custom.Value);
conflictTarget.AddMod(new InstalledMod("KsaChinese", oldRelease.Version, InstallReason.Manual, DateTimeOffset.UtcNow, oldRelease));
var conflictPlan = await graph.InstallPlanner.PlanAsync(new InstallPlanningRequest(conflictTarget,
    [new RequestedMod(release, InstallReason.Manual, true)], graph.OfflineContentIndex,
    GameVersion.Parse("2026.10.19.5638"), OsPlatform.Windows));
Check(!conflictPlan.IsReady && conflictPlan.Conflicts.Count > 0, "Borea planner blocks KsaBabel with installed old KsaChinese ID");
records.Add(new { conflictPlanning = conflictPlan.Conflicts.Select(message => message.Kind.ToString()).ToArray() });
string Profile(Instance instance) => graph.Paths.GetInstanceRoot(instance.InstanceId);
string Settings(Instance instance) => Path.Combine(Profile(instance), "KsaBabel/settings.json");
string LegacySettings(Instance instance) => Path.Combine(Profile(instance), "KsaChinese/settings.json");
string Dll(Instance instance) => Path.Combine(Profile(instance), "mods/KsaBabel/KsaBabel.dll");
Check(Digest(Dll(first)) == fixture.GetProperty("previous_dll_sha256").GetString(), "First instance installed synthetic 0.2.0 DLL bytes");
Check(Digest(Dll(second)) == fixture.GetProperty("current_dll_sha256").GetString(), "Second instance installed actual 0.2.1 candidate DLL bytes");
Check(Digest(Dll(first)) != Digest(Dll(second)), "Old and new version payload DLL hashes differ");
if (managementOnly)
{
    // File sentinels test ownership boundaries, not game-save parsing or a rendered game.
    Directory.CreateDirectory(Path.GetDirectoryName(Settings(first))!);
    Directory.CreateDirectory(Path.GetDirectoryName(LegacySettings(first))!);
    File.WriteAllText(Settings(first), "{\"locale\":\"en-US\"}\n");
    File.WriteAllText(LegacySettings(first), "{\"locale\":\"zh-CN\"}\n");
    string save = Path.Combine(Profile(first), "saves", "ownership-sentinel.txt");
    Directory.CreateDirectory(Path.GetDirectoryName(save)!);
    File.WriteAllText(save, "Local ownership sentinel, not a playable game save.\n");
    var preserved = new[] { Settings(first), LegacySettings(first), save }.ToDictionary(path => path, Digest);
    await Command("disable", "KsaBabel", "--instance", "Babel A");
    Check(!await graph.ModState.IsActiveAsync(first.InstanceId, "KsaBabel"), "Disable writes inactive KsaBabel manifest entry");
    await Command("enable", "KsaBabel", "--instance", "Babel A");
    Check(await graph.ModState.IsActiveAsync(first.InstanceId, "KsaBabel"), "Enable restores active KsaBabel manifest entry");
    await Command("update", "KsaBabel", "--instance", "Babel A", "--channel", "testing");
    var installed = (await graph.Instances.GetByIdAsync(first.InstanceId))!.Mods.Single(mod => mod.ModId == "KsaBabel");
    Check(installed.Version.ToString() == "0.2.1", "Actual Borea update installs version 0.2.1 from synthetic 0.2.0");
    Check(Digest(Dll(first)) == fixture.GetProperty("current_dll_sha256").GetString(), "Updated DLL equals actual candidate bytes");
    Check(preserved.All(pair => Digest(pair.Key) == pair.Value), "Update preserves preference, legacy preference and save sentinel bytes");
    var modLoader = (await graph.OfflineContentIndex.GetListingAsync("StarMap"))!;
    var planResult = graph.Launcher.Launch((await graph.Instances.GetByIdAsync(first.InstanceId))!, modLoader);
    Check(planResult.Started && launchRecorder.Plans.Count == 1, "Upstream launcher reaches dry process boundary; no OS game process created");
    var plan = launchRecorder.Plans.Single();
    Check(string.Equals(Path.GetFullPath(plan.Executable), Path.GetFullPath(Path.Combine(root, "Loaders/StarMap/StarMap.exe")), StringComparison.OrdinalIgnoreCase), "Launch plan selects installed StarMap executable");
    Check(plan.Arguments.Contains(Profile(first)) || plan.EnvironmentVariables.Values.Contains(Profile(first)), "Launch plan passes selected isolated profile");
    records.Add(new { dryLaunchPlan = new { executable = Clean(plan.Executable),
        arguments = plan.Arguments.Select(Clean).ToArray(), workingDirectory = Clean(plan.WorkingDirectory),
        environment = plan.EnvironmentVariables.ToDictionary(pair => pair.Key, pair => Clean(pair.Value)) }, osProcessStarted = false });
    await Command("remove", "KsaBabel", "--instance", "Babel A");
    Check(!Directory.Exists(Path.Combine(Profile(first), "mods/KsaBabel")), "Uninstall removes owned KsaBabel mod directory");
    Check(!(await graph.Instances.GetByIdAsync(first.InstanceId))!.Mods.Any(mod => mod.ModId == "KsaBabel"), "Uninstall removes installed version record");
    Check(preserved.All(pair => File.Exists(pair.Key) && Digest(pair.Key) == pair.Value), "Uninstall preserves preference, legacy preference and save sentinel bytes");
    Check(Digest(Dll(second)) == fixture.GetProperty("current_dll_sha256").GetString(), "Uninstall leaves second instance payload intact");
    Check(!Directory.Exists(Path.Combine(root, "unused-shared-profile")), "No shared or personal profile was created");
    var headlessReport = new { boreaVersion = "0.5.1", sourceCommit = "284d5cf929c8ede99320f8afc451fa44f12aeadd",
        verifiedAtUtc = DateTime.UtcNow, mode = "management-only; real filesystem and manager operations, recorded launch plan only",
        currentVersion = "0.2.1", modId = "KsaBabel", pluginSha256 = fixture.GetProperty("current_dll_sha256").GetString(),
        transport = "local archives and candidate index; unmodified upstream source components",
        previousFixture = fixture.GetProperty("previous_fixture").GetString(),
        releaseChannel = "testing", testSave = "ownership sentinel only, not a game-format save",
        graphicalGameLaunchedInThisRun = false, publicCatalogAccepted = false, desktopGuiAccepted = false, forumPublished = false,
        checks, records };
    File.WriteAllText(Path.Combine(project, "reports/ecosystem-validation.json"), JsonSerializer.Serialize(headlessReport, new JsonSerializerOptions { WriteIndented = true }) + "\n");
    Console.WriteLine("PASS: " + checks.Count + " headless manager checks; no game process created");
    return;
}
var immutable = Digest(Path.Combine(Profile(second), "mods/KsaBabel/language.json"));
var settingsSeed = new[] { Path.Combine(project, "work/test-profile-babel-020/settings.toml"),
    Path.Combine(Directory.GetParent(project)!.FullName, "KSA-CHINESE/work/test-profile-5638-015/settings.toml") }.FirstOrDefault(File.Exists)
    ?? throw new Exception("An isolated settings.toml seed is required to bypass first-run setup.");
foreach (var instance in instances)
{
    File.Copy(settingsSeed, Path.Combine(Profile(instance), "settings.toml"), overwrite: true);
    var driver = Path.Combine(Profile(instance), "mods/SmokeDriver");
    Directory.CreateDirectory(driver);
    File.Copy(Path.Combine(project, "work/build/SmokeDriver/SmokeDriver.dll"), Path.Combine(driver, "SmokeDriver.dll"));
    File.WriteAllText(Path.Combine(driver, "mod.toml"), "name = \"Workspace ecosystem test driver\"\nassets = []\n[StarMap]\nEntryAssembly = \"SmokeDriver\"\n");
    await graph.ModState.AddEntryAsync(instance.InstanceId, "SmokeDriver", true);
    await graph.ModState.PutGameContentFirstAsync(instance.InstanceId, game);
}
var loader = (await graph.OfflineContentIndex.GetListingAsync("StarMap"))!;
async Task Launch(Instance instance, string mode)
{
    string smokeLog = Path.Combine(Profile(instance), "smoke.log");
    long start = File.Exists(smokeLog) ? new FileInfo(smokeLog).Length : 0;
    string? previousMode = Environment.GetEnvironmentVariable("KSA_SMOKE_ECOSYSTEM");
    string? previousProject = Environment.GetEnvironmentVariable("KSA_SMOKE_PROJECT_ROOT");
    try
    {
        Environment.SetEnvironmentVariable("KSA_SMOKE_ECOSYSTEM", mode);
        Environment.SetEnvironmentVariable("KSA_SMOKE_PROJECT_ROOT", project);
        var launch = graph.Launcher.Launch((await graph.Instances.GetByIdAsync(instance.InstanceId))!, loader);
        using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(3));
        launch = await graph.Launcher.WatchStartAsync(instance, launch, timeout.Token);
        if (!launch.Started) throw new Exception(launch.Message + "\n" + string.Join('\n', launch.Output));
        var exit = await graph.Launcher.WatchExitAsync(instance, launch, timeout.Token);
        if (exit.ExitCode != 0) throw new Exception("Borea runtime exit " + exit.ExitCode + "\n" + string.Join('\n', exit.Output));
        Check(true, "Actual Borea launch exited zero: " + instance.Name + " / " + mode);
        using var logFile = new FileStream(smokeLog, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        logFile.Seek(start, SeekOrigin.Begin);
        using var logReader = new StreamReader(logFile);
        string log = logReader.ReadToEnd();
        Check(!log.Contains("ERROR=") && log.Contains("ecosystem-completed=" + mode), "Runtime driver completed: " + mode);
        if (mode != "disabled")
            Check(log.Contains(Path.GetFullPath(Dll(instance)), StringComparison.OrdinalIgnoreCase), "Game loaded plugin from chosen Borea instance: " + instance.Name + " / " + mode);
        records.Add(new { runtime = mode, instance = instance.Name, exitCode = exit.ExitCode,
            smokeLog = Clean(log), output = Clean(string.Join('\n', exit.Output)) });
        Console.WriteLine("Runtime " + instance.Name + " / " + mode + ": " + exit.ExitCode);
    }
    finally
    {
        Environment.SetEnvironmentVariable("KSA_SMOKE_ECOSYSTEM", previousMode);
        Environment.SetEnvironmentVariable("KSA_SMOKE_PROJECT_ROOT", previousProject);
    }
}
Directory.CreateDirectory(Path.GetDirectoryName(LegacySettings(first))!);
File.WriteAllText(LegacySettings(first), "{\"locale\":\"en-US\"}\n");
var legacyHash = Digest(LegacySettings(first));
await Launch(first, "migrate-en-to-zh");
Check(File.ReadAllText(Settings(first)).Contains("zh-CN"), "First launch imported legacy English and saved Chinese to KsaBabel settings");
Check(Digest(LegacySettings(first)) == legacyHash, "Migration preserves original KsaChinese preference bytes");
Check(!File.Exists(Settings(second)) && !File.Exists(LegacySettings(second)), "First instance migration did not create settings in second instance");
await Launch(second, "verify-zh");
Check(immutable == Digest(Path.Combine(Profile(second), "mods/KsaBabel/language.json")), "Shared mod defaults were not mutated");
await Launch(first, "zh-to-en");
Check(File.ReadAllText(Settings(first)).Contains("en-US"), "First instance saved English outside managed mod files");
var saves = Directory.EnumerateFiles(Path.Combine(Profile(first), "saves"), "*", SearchOption.AllDirectories).ToDictionary(p => p, Digest);
Check(saves.Count > 0, "Test save exists before disable and version upgrade");
var settingBeforeUpdate = Digest(Settings(first));
await Command("disable", "KsaBabel", "--instance", "Babel A");
await Launch(first, "disabled");
await Command("enable", "KsaBabel", "--instance", "Babel A");
await Command("update", "KsaBabel", "--instance", "Babel A", "--channel", "testing", "--dry-run");
await Command("update", "KsaBabel", "--instance", "Babel A", "--channel", "testing");
var updated = (await graph.Instances.GetByIdAsync(first.InstanceId))!.Mods.Single(mod => mod.ModId == "KsaBabel");
Check(updated.Version.ToString() == "0.2.1", "Actual Borea update changed installed version from synthetic 0.2.0 to 0.2.1");
Check(Digest(Dll(first)) == fixture.GetProperty("current_dll_sha256").GetString(), "Actual Borea update installed new candidate DLL bytes");
Check(Digest(Settings(first)) == settingBeforeUpdate, "Actual version update preserved profile preference bytes");
Check(saves.All(pair => File.Exists(pair.Key) && Digest(pair.Key) == pair.Value), "Disable and actual version update preserved test save bytes");
await Launch(first, "en-to-zh");
Check(File.ReadAllText(Settings(first)).Contains("zh-CN"), "Updated plugin restored English, loaded save and saved Chinese again");
// A manifest-only old-ID mod deliberately bypasses install planning to exercise the runtime early guard.
var legacyMod = Path.Combine(Profile(second), "mods/KsaChinese");
Directory.CreateDirectory(legacyMod);
File.WriteAllText(Path.Combine(legacyMod, "mod.toml"), "name = \"LOCAL empty old-ID conflict fixture; no old translation code\"\nassets = []\n");
await graph.ModState.AddEntryAsync(second.InstanceId, "KsaChinese", true);
await Launch(second, "conflict");
await graph.ModState.SetInactiveAsync(second.InstanceId, "KsaChinese");
await Launch(second, "verify-zh");
var settingsBeforeRemove = Digest(Settings(first));
await Command("remove", "KsaBabel", "--instance", "Babel A");
Check(!Directory.Exists(Path.Combine(Profile(first), "mods/KsaBabel")), "Borea removed owned KsaBabel mod directory");
Check(Digest(Settings(first)) == settingsBeforeRemove, "Uninstall preserves profile preference bytes");
Check(Digest(LegacySettings(first)) == legacyHash, "Update and uninstall preserve original legacy preference bytes");
Check(saves.All(pair => File.Exists(pair.Key) && Digest(pair.Key) == pair.Value), "Disable, actual update and uninstall preserve test save bytes");
Check(File.Exists(Dll(second)), "Removing first instance mod preserves second instance");
await Launch(first, "disabled");
Check(!Directory.Exists(Path.Combine(root, "unused-shared-profile")), "No shared/personal game profile was created");
var report = new { boreaVersion = "0.5.1", sourceCommit = "284d5cf929c8ede99320f8afc451fa44f12aeadd", verifiedAtUtc = DateTime.UtcNow,
    transport = "local archives and candidate index using unmodified upstream source components",
    previousFixture = fixture.GetProperty("previous_fixture").GetString(),
    currentVersion = "0.2.1", releaseChannel = "testing; both local versions are prereleases", modId = "KsaBabel", assembly = "KsaBabel.dll",
    conflictRuntimeFixture = "Manifest-only empty KsaChinese ID; proves Babel early guard, not coexistence with a full old translator",
    publicIndexSubmitted = false, publicCatalogAccepted = false, desktopGuiAccepted = false, forumPublished = false,
    checks, records };
File.WriteAllText(Path.Combine(project, "reports/ecosystem-validation.json"), JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true }) + "\n");
Console.WriteLine("PASS: " + checks.Count + " ecosystem checks");

sealed class NoCandidates : IInstallCandidateSource
{
    public IReadOnlyList<string> GetGameDirectories() => [];
    public IReadOnlyList<string> GetLoaderDirectories() => [];
}

// Implements the upstream test seam. It records the handover without starting a process.
sealed class LaunchPlanRecorder : IProcessStarter
{
    public List<LaunchPlan> Plans { get; } = new();
    public IStartedProcess Start(LaunchPlan plan) { Plans.Add(plan); return new RecordedProcess(); }
    private sealed class RecordedProcess : IStartedProcess
    {
        public int Id => 42;
        public bool HasExited => true;
        public int? ExitCode => 0;
        public IReadOnlyList<string> RecentOutput => [];
        public Task<bool> WaitForExitAsync(TimeSpan timeout, CancellationToken cancellationToken = default) => Task.FromResult(true);
        public void Dispose() { }
    }
}
sealed class ReaderSnapshotProvider(IContentIndexReader reader) : IContentIndexSnapshotProvider
{
    public Task<ContentIndexSnapshot> GetSnapshotAsync(CancellationToken cancellationToken = default) => reader.ReadAsync(cancellationToken);
}
sealed class CandidateTransport(IReadOnlyDictionary<string, string> files) : HttpMessageHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var url = request.RequestUri!.AbsoluteUri;
        if (request.Method != HttpMethod.Get || !files.TryGetValue(url, out var file))
            throw new HttpRequestException("Unexpected integration-test request: " + request.Method + " " + url);
        var content = new ByteArrayContent(await File.ReadAllBytesAsync(file, ct));
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.EndsWith(".json") ? "application/json" : "application/zip");
        return new HttpResponseMessage(HttpStatusCode.OK) { Content = content };
    }
}
