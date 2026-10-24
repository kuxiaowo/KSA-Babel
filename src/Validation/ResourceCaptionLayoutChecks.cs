using HarmonyLib;
using KSA;
using KsaChinese;
using System.Reflection;

internal static partial class Program
{
    static void ResourceCaptionLayoutChecks()
    {
        Check(CaptionPixelLayout.FitSize(20, 400, 100) == 5
            && CaptionPixelLayout.FitSize(20, 40, 100) == 20, "长资源文字按真实像素缩放，短文字不放大");
        foreach (float width in new[] { 1f, 80f, 150f, 340f })
        {
            float gap = MathF.Min(5, width * 0.05f);
            foreach (float value in new[] { 20f, 90f, 400f })
            {
                var allocated = CaptionPixelLayout.TotalWidths(width, value, gap);
                Check(allocated.Caption > 0 && allocated.Value > 0
                    && allocated.Caption + allocated.Value + gap <= width + 0.001f,
                    "总Δv标签与完整数值单位空间互不重叠: " + width + "/" + value);
            }
        }
        foreach (string name in new[] { nameof(SequenceList.DrawSequenceContent), nameof(SequenceList.DrawTotalDeltaVFooter) })
        {
            var method = AccessTools.Method(typeof(SequenceList), name);
            var source = PatchProcessor.GetOriginalInstructions(method).Select(i => new CodeInstruction(i)).ToArray();
            var changed = StagingCaptionLayoutPatch.Translate(source, method).ToArray();
            int expected = name == nameof(SequenceList.DrawSequenceContent) ? 4 : 2;
            Check(changed.Count(i => i.operand is MethodInfo m && m.DeclaringType == typeof(StagingCaptionLayoutPatch)) == expected,
                "真实5638分级IL仅替换受控绘字入口: " + name);
        }
        var original = Runtime.SelectedLocale;
        Runtime.ApplyLanguage("en-US");
        ReadOnlySpan<char> caption = "Liquid Hydrogen", numeric = "123.4kg";
        ResourceCaptionLayoutPatch.Capture(ref caption, ref numeric, out var state);
        Check(state is null && caption.SequenceEqual("Liquid Hydrogen") && numeric.SequenceEqual("123.4kg"),
            "资源条英文保留原绘制参数");
        Runtime.ApplyLanguage("ru-RU");
        caption = "Водород (жидкость)"; numeric = "123.4kg";
        ResourceCaptionLayoutPatch.Capture(ref caption, ref numeric, out state);
        Check(state?.Name == "Водород (жидкость)" && state.Value == "123.4kg" && caption.IsEmpty && numeric.IsEmpty,
            "资源条双行状态完整保留名称数值单位，无省略");
        Runtime.ApplyLanguage(original);
    }
}
