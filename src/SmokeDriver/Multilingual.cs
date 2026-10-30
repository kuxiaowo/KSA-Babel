using System.Reflection;
using System.Text.Json;
using Brutal.ImGuiApi;
using Brutal.Numerics;
using KSA;
using HarmonyLib;
using Constants = KSA.Constants;

namespace KsaChineseSmoke;

public sealed partial class Driver
{
    readonly string[] MultilingualLocales = ["en-US", "ja-JP", "ru-RU", "de-DE", "fr-FR", "it-IT", "es-ES", "pt-BR", "zh-CN"];
    readonly List<object> MultilingualResults = new();
    double NextLocaleTime = 2;
    bool LocaleShotPending;
    int LocaleVisualPhase;
    static long CaptionDraws;
    long LocaleCaptionStart;
    public static void CaptionDrawObserved() => CaptionDraws++;
    static readonly Dictionary<string,int> BodyMetricsFrames = new();
    public static void LocaleBodyMetrics(ReadOnlySpan<char> id, float2 pos, float2 size)
    {
        if (!id.StartsWith("Settings")) return;
        var runtime=AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a=>a.GetName().Name=="KsaBabel")?.GetType("KsaChinese.Runtime");
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
                scope = "Real loaded 5707 game, all nine locale switches, HDR settings and space weather screenshots; not exhaustive linguistic or HDR-output acceptance",
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
            var assembly = AppDomain.CurrentDomain.GetAssemblies().Single(a => a.GetName().Name == "KsaBabel");
            string expectedPlugin = Path.Combine(Constants.DocumentsFolderPath, "mods", "KsaBabel", "KsaBabel.dll");
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
            var display = (Dictionary<string,string>)pack.GetType().GetProperty("Display")!.GetValue(pack)!;
            string weather = (string)assembly.GetType("KsaChinese.Update5707Text")!.GetMethod("Text")!.Invoke(null,["Solar wind: "])!;
            if(weather != (locale=="en-US" ? "Solar wind: " : display["Solar wind: "]))
                throw new InvalidOperationException("5707 weather translation mismatch: "+locale);
            MultilingualResults.Add(new { locale, selected, resolution, gaugeFont, plugin = assembly.Location,
                weather,
                actualGameVersion = typeof(Program).Assembly.GetName().Version?.ToString() });
            Log("multilingual-selected=" + locale + "; resolution=" + resolution + "; gaugeFont=" + gaugeFont);
            LocaleShotPending = true;
            NextLocaleTime = t + 3;
        }
        else if (LocaleVisualPhase==0)
        {
            long draws=CaptionDraws-LocaleCaptionStart;
            if(draws<=0) throw new InvalidOperationException("Gauge caption renderer was not exercised: "+locale);
            Log("gauge-caption-passes="+locale+"; count="+draws);
            Shot("multilingual-" + locale);
            LocaleVisualPhase=1;
            NextLocaleTime=t+1;
        }
        else if(LocaleVisualPhase==1)
        {
            GameSettings.Show=false;
            SpaceWeatherWindow.Toggle();
            var window=AccessTools.Field(typeof(SpaceWeatherWindow),"_instance").GetValue(null)!;
            AccessTools.Field(window.GetType(),"_pickedStarId").SetValue(window,"Sol");
            AccessTools.Field(window.GetType(),"_pickedBodyId").SetValue(window,"Earth");
            LocaleVisualPhase=2;
            NextLocaleTime=t+3;
        }
        else if(LocaleVisualPhase==2)
        {
            Shot("5707-weather-"+locale);
            LocaleVisualPhase=3;
            NextLocaleTime=t+1;
        }
        else
        {
            SpaceWeatherWindow.Toggle();
            LocaleVisualPhase=0;
            LocaleShotPending = false;
            Stage++;
            NextLocaleTime = t + 2;
        }
    }
}
