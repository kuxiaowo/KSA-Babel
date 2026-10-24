using System.Diagnostics;

internal static partial class Program
{
    static void LauncherChecks()
    {
        string probe = Path.Combine(Fixture, "launcher-check-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(probe);
        foreach (string suffix in new[] { ".exe", ".dll", ".deps.json", ".runtimeconfig.json" })
            File.Copy(Path.Combine(Root, "work/build/Launcher", "KsaBabelLauncher" + suffix),
                Path.Combine(probe, "KsaBabelLauncher" + suffix));
        string manifest = Path.Combine(probe, "compatibility.json");
        File.Copy(Path.Combine(Root, "assets/KsaBabel/compatibility.json"), manifest);
        int Probe(string directory)
        {
            var start = new ProcessStartInfo("dotnet") { UseShellExecute = false, CreateNoWindow = true,
                RedirectStandardOutput = true, RedirectStandardError = true };
            start.ArgumentList.Add(Path.Combine(probe, "KsaBabelLauncher.dll"));
            start.ArgumentList.Add("--check");
            start.ArgumentList.Add(directory);
            using var process = Process.Start(start)!;
            if (!process.WaitForExit(30000)) { process.Kill(entireProcessTree: true); throw new Exception("Launcher check timed out"); }
            return process.ExitCode;
        }
        Check(Probe(Game) == 0, "独立命名启动器接受实际5638文件指纹");
        Check(Probe(Path.Combine(probe, "missing-game")) != 0, "启动器拒绝缺失游戏且不启动加载器");
        string invalid = Path.Combine(probe, "invalid-game");
        Directory.CreateDirectory(invalid);
        File.WriteAllText(Path.Combine(invalid, "KSA.dll"), "fixture, not a game binary");
        File.WriteAllText(manifest, "{\"files\":{\"KSA.dll\":\"invalid-digest\"}}");
        Check(Probe(invalid) != 0, "启动器拒绝错误游戏哈希且不启动加载器");
    }
}
