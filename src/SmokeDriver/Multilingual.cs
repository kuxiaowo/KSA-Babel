using System.Reflection;
using System.Text.Json;
using Brutal.ImGuiApi;
using Brutal.Numerics;
using KSA;
using Constants = KSA.Constants;

namespace KsaChineseSmoke;

public sealed partial class Driver
{
    readonly string[] MultilingualLocales = ["en-US", "ja-JP", "ru-RU", "de-DE", "fr-FR", "it-IT", "es-ES", "pt-BR", "zh-CN"];
    readonly List<object> MultilingualResults = new();
    double NextLocaleTime = 2;
    bool LocaleShotPending;
    static long CaptionDraws;
    long LocaleCaptionStart;
    public static void CaptionDrawObserved() => CaptionDraws++;
    static readonly Dictionary<string,int> BodyMetricsFrames = new();
    public static void LocaleBodyMetrics(ReadOnlySpan<char> id, float2 pos, float2 size)
    {
        if (!id.StartsWith("Settings")) return;
        var runtime=AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a=>a.GetName().Name=="KsaChinese")?.GetType("KsaChinese.Runtime");
        string locale=(string?)runtime?.GetProperty("SelectedLocale")?.GetValue(null)??"unknown";
        string key=locale+":"+id.ToString();
        int count=BodyMetricsFrames.GetValueOrDefault(key)+1; BodyMetricsFrames[key]=count;
        if(count is 1 or 100) Log($"body-metrics={key}; frame={count}; pos={pos}; size={size}; scrollX={ImGui.GetScrollX()}; maxX={ImGui.GetScrollMaxX()}");
    }

    void MultilingualFrame(double t)
    {
        if (t < NextLocaleTime) return;
        if (Stage >= MultilingualLocales.Length)
        {
            var path = Path.Combine(Constants.DocumentsFolderPath, "multilingual-runtime.json");
            File.WriteAllText(path, JsonSerializer.Serialize(new {
                scope = "Real loaded 5638 game, all nine locale switches, resource provider and screenshot requests; not exhaustive linguistic acceptance",
                results = MultilingualResults }, new JsonSerializerOptions { WriteIndented = true }));
            Log("multilingual-completed; locales=" + MultilingualResults.Count);
            Program.Instance.RequestExit(0);
            Stage++;
            return;
        }
        string locale = MultilingualLocales[Stage];
        if (!LocaleShotPending)
        {
            LocaleCaptionStart=CaptionDraws;
            Locale(locale);
            Tab("Display");
            var assembly = AppDomain.CurrentDomain.GetAssemblies().Single(a => a.GetName().Name == "KsaChinese");
            string expectedPlugin = Path.Combine(Constants.DocumentsFolderPath, "mods", "KsaChinese", "KsaChinese.dll");
            if (!string.Equals(Path.GetFullPath(assembly.Location), Path.GetFullPath(expectedPlugin), StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Fixture loaded a shadowing plugin: " + assembly.Location);
            var runtime = assembly.GetType("KsaChinese.Runtime")!;
            string selected = (string)runtime.GetProperty("SelectedLocale")!.GetValue(null)!;
            if (selected != locale) throw new InvalidOperationException("Wrong active locale: " + selected);
            var pack = runtime.GetProperty("CurrentPack")!.GetValue(null)!;
            var native = (Dictionary<string,string>)pack.GetType().GetProperty("Native")!.GetValue(pack)!;
            string resolution = LStrings.SettingsDisplayResolution.Localized;
            if (resolution != (locale == "en-US" ? "Resolution" : native["SettingsDisplayResolution"]))
                throw new InvalidOperationException("Native resource mismatch: " + locale);
            string gaugeFont = (string)assembly.GetType("KsaChinese.GaugeFontPatch")!.GetMethod("Name")!.Invoke(null,null)!;
            MultilingualResults.Add(new { locale, selected, resolution, gaugeFont, plugin = assembly.Location,
                actualGameVersion = typeof(Program).Assembly.GetName().Version?.ToString() });
            Log("multilingual-selected=" + locale + "; resolution=" + resolution + "; gaugeFont=" + gaugeFont);
            LocaleShotPending = true;
            NextLocaleTime = t + 3;
        }
        else
        {
            long draws=CaptionDraws-LocaleCaptionStart;
            if(draws<=0) throw new InvalidOperationException("Gauge caption renderer was not exercised: "+locale);
            Log("gauge-caption-passes="+locale+"; count="+draws);
            Shot("multilingual-" + locale);
            LocaleShotPending = false;
            Stage++;
            NextLocaleTime = t + 2;
        }
    }
}
