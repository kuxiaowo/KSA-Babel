using System.Runtime.CompilerServices;
using System.Text.Json;
using KSA;
using KsaChinese;

internal static partial class Program
{
    static void MultilingualChecks()
    {
        if (!File.Exists(Path.Combine(Root, "assets", "KsaChinese", "Locales", "ja-JP.json"))) return;
        var expected = new[] { "en-US", "zh-CN", "ja-JP", "ru-RU", "de-DE", "fr-FR", "it-IT", "es-ES", "pt-BR" };
        Check(Runtime.Packs.Keys.Order().SequenceEqual(expected.Order()), "九语言完整加载，未混入未知语言包");
        Runtime.ApplyLanguage("en-US");
        var labels = new[] { "File", "File##main", "File###main", "Resources###hud-resources" };
        var ids = labels.ToDictionary(x => x, x => Hash(Runtime.MenuText(x), 0x735A1913));
        var results = new List<object>();
        foreach (string locale in expected)
        {
            Runtime.SelectLanguage(locale);
            var pack = Runtime.CurrentPack;
            Check(LocaleSettings.Read(Runtime.SettingsPath, "zh-CN", expected, _ => { }) == locale,
                "独立档案保存并读取语言: " + locale);
            foreach (string label in labels)
                Check(Hash(Runtime.MenuText(label), 0x735A1913) == ids[label], "九语言菜单控件 ID 一致: " + locale + " " + label);
            string native = KSA.LStrings.SettingsDisplayResolution.Localized;
            Check(native == (locale == "en-US" ? "Resolution" : pack.Native["SettingsDisplayResolution"]),
                "真实原生资源提供器九语言往返: " + locale);
            Check(Runtime.GaugeText("not-a-caption") == "not-a-caption" && Runtime.PartName(new PartTemplate
                { Id = "unlisted-part", DisplayName = "My Ship 123" }) == "My Ship 123",
                "未知仪表文本与玩家命名回退: " + locale);
            Check(GaugeFontPatch.Name() == (locale == "en-US" ? "MilSpec33558" : Runtime.FontName),
                "非英语仪表选择有字形的字体: " + locale);
            Check(Runtime.DynamicText("Phase.Liquid", "Liquid {0}").Contains("{0}")
                && !Runtime.DynamicText("LanguageMenu", "Language").Contains("###"),
                "动态语言模板保留参数和独立菜单 ID: " + locale);
            const string ratio = "The oxidizer-to-fuel mass ratio fed to the combustion chamber.\nTypical value for this mixture: ";
            Check(ExpandedUi.Tooltip(ratio + "2.347") == Runtime.DynamicText("Mixture.RatioPrefix", ratio) + "2.347",
                "动态混合比前缀九语言保留实时数值: " + locale);
            var substance = (Substance)RuntimeHelpers.GetUninitializedObject(typeof(Substance));
            substance.Id = "H2"; substance.ShortName = "Hydrogen"; substance.FullName = "Hydrogen";
            var phase = (SubstancePhase)RuntimeHelpers.GetUninitializedObject(typeof(SubstancePhase));
            phase.Id = "LiquidH2"; phase.Name = "Liquid Hydrogen"; phase.Substance = substance;
            string translated = ExpandedUi.PhaseName(phase);
            Check(translated.Contains(ExpandedUi.SubstanceShort(substance)) && phase.Id == "LiquidH2"
                && phase.Name == "Liquid Hydrogen" && substance.ShortName == "Hydrogen",
                "相态显示九语言保留底层字段: " + locale);
            if (locale != "en-US")
            {
                Check(pack.Parts.Count == 650 && pack.Native.Count == 216 && pack.Display.Count == 1991
                    && pack.Content.Count == 257 && pack.History.Count == 228,
                    "现有审阅范围各区段齐全: " + locale);
                Check(Runtime.ImText(native).ToString() == native, "非 ASCII 原生文字 UTF-8 往返: " + locale);
            }
            results.Add(new { locale, resolution = native, gaugeFont = GaugeFontPatch.Name(),
                selectedLocale = Runtime.SelectedLocale, dynamicPhase = translated });
        }
        Runtime.SelectLanguage("zh-CN");
        File.WriteAllText(Path.Combine(Root, "reports", "multilingual-runtime-contracts.json"),
            JsonSerializer.Serialize(new { scope = "headless real resource provider and native ImGui hash checks; not graphical acceptance", results },
                new JsonSerializerOptions { WriteIndented = true }));
    }
}
