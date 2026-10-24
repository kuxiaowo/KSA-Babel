using System.Reflection;
using System.Text.Json;
using KSA;

namespace KsaChineseSmoke;

public sealed partial class Driver
{
    void EcosystemFrame(string mode, double t)
    {
        var plugin = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(a => a.GetName().Name == "KsaBabel");
        var runtime = plugin?.GetType("KsaChinese.Runtime");
        if (Stage == 0 && t > 2)
        {
            if (mode == "disabled")
            {
                if (plugin is not null) throw new Exception("Disabled mod was loaded.");
                if (KSA.LStrings.SettingsDisplayResolution.Localized != "Resolution")
                    throw new Exception("Disabled mod did not leave native English text.");
                Log("ecosystem-disabled-english");
            }
            else if (mode == "conflict")
            {
                if (runtime is null) throw new Exception("Conflict probe did not load Babel assembly.");
                if ((bool)runtime.GetProperty("IsActive")!.GetValue(null)!)
                    throw new Exception("Babel remained active with old KsaChinese ID enabled.");
                string reason = (string)runtime.GetProperty("DisabledReason")!.GetValue(null)!;
                if (!reason.Contains("KsaChinese")) throw new Exception("Conflict reason did not identify old ID.");
                if (KSA.LStrings.SettingsDisplayResolution.Localized != "Resolution")
                    throw new Exception("Early conflict guard changed native English text.");
                Log("ecosystem-conflict=" + reason + "; plugin=" + plugin!.Location);
            }
            else
            {
                if (runtime is null) throw new Exception("Mod was not loaded.");
                if (!(bool)runtime.GetProperty("IsActive")!.GetValue(null)!)
                    throw new Exception("Babel assembly loaded but runtime was inactive.");
                string expected = mode is "en-to-zh" or "migrate-en-to-zh" or "verify-en" ? "en-US" : "zh-CN";
                string actual = (string)runtime.GetProperty("SelectedLocale")!.GetValue(null)!;
                if (actual != expected) throw new Exception($"Expected persisted locale {expected}, got {actual}");
                Log("ecosystem-initial=" + actual + "; plugin=" + plugin!.Location);
                if (expected == "zh-CN") ContentSnapshot();
                else if (KSA.LStrings.SettingsDisplayResolution.Localized != "Resolution")
                    throw new Exception("Persisted English provider did not restore native text.");
            }
            Shot("ecosystem-" + mode);
            Stage++;
        }
        else if (Stage == 1 && t > 6)
        {
            if (mode is not "disabled" and not "conflict")
            {
                if (mode == "zh-to-en") runtime!.GetMethod("SelectLanguage")!.Invoke(null, ["en-US"]);
                if (mode is "en-to-zh" or "migrate-en-to-zh") runtime!.GetMethod("SelectLanguage")!.Invoke(null, ["zh-CN"]);
                string locale = (string)runtime!.GetProperty("SelectedLocale")!.GetValue(null)!;
                Log("ecosystem-selected=" + locale);
                if (mode == "zh-to-en") GameSaves.MakeUncompressedSave("ksa_chinese_ecosystem_test");
                if (mode == "en-to-zh") GameSaves.LoadSaveGame("ksa_chinese_ecosystem_test");
            }
            Stage++;
        }
        else if (Stage == 2 && t > 13)
        {
            if (mode == "en-to-zh") { ContentSnapshot(); Log("ecosystem-reloaded=" + Program.ControlledVehicle?.Id); }
            Log("ecosystem-completed=" + mode);
            Program.Instance.RequestExit(0);
            Stage++;
        }
    }
}
