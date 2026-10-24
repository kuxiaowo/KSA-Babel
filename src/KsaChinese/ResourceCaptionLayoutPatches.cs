using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Brutal.ImGuiApi;
using Brutal.Numerics;
using HarmonyLib;
using KSA;

namespace KsaChinese;

// These helpers change only draw-list geometry. The complete caption, numeric
// value and unit are rendered; no resource fields or performance data change.
public static class CaptionPixelLayout
{
    public static float FitSize(float size, float measuredWidth, float availableWidth)
        => MathF.Max(0.1f, size * MathF.Min(1f, MathF.Max(0.1f, availableWidth) / MathF.Max(0.1f, measuredWidth)));

    public static (float Caption, float Value) TotalWidths(float width, float valueWidth, float gap)
    {
        float usable = MathF.Max(0.2f, width - gap);
        // Values normally keep their original size. Very large values share
        // the row rather than consuming the complete translated caption.
        float value = MathF.Min(MathF.Max(0.1f, valueWidth), usable * 0.60f);
        return (MathF.Max(0.1f, usable - value), value);
    }

    public static void Text(ImDrawListPtr draw, string text, ImColor8 color, float left,
        float top, float width, float height, bool right = false, float maxSize = 0, ImFontPtr? fontOverride = null)
    {
        if (string.IsNullOrEmpty(text) || width <= 0 || height <= 0) return;
        var font = fontOverride ?? ImGui.GetFont();
        float size = maxSize > 0 ? maxSize : ImGui.GetFontSize();
        size = MathF.Min(size, height);
        // ImString's frame arena avoids storing continually changing numeric
        // readouts in Runtime's permanent translation UTF-8 cache.
        ImString caption = text;
        var measured = font.CalcTextSizeA(size, float.MaxValue, 0, caption);
        size = FitSize(size, measured.X, width);
        measured = font.CalcTextSizeA(size, float.MaxValue, 0, caption);
        var pos = new float2(right ? left + width - measured.X : left, top + (height - measured.Y) * 0.5f);
        draw.AddText(font, size, pos, color, caption);
    }
}

// Retain the game's bar fill and fraction marker. Only suppress its single
// concatenated text line, then render the full name and full value on two lines.
[HarmonyPatch(typeof(ResourcesPanel), "DrawBar")]
public static class ResourceCaptionLayoutPatch
{
    public sealed record Caption(string Name, string Value);

    [HarmonyPrefix, HarmonyPriority(Priority.Last)]
    public static void Capture(ref ReadOnlySpan<char> name, ref ReadOnlySpan<char> value, out Caption? __state)
    {
        __state = null;
        if (Runtime.SelectedLocale == "en-US" || (name.IsEmpty && value.IsEmpty)) return;
        __state = new Caption(name.ToString(), value.ToString());
        name = default;
        value = default;
    }

    [HarmonyPostfix]
    public static void Draw(float2 origin, float2 size, Caption? __state)
    {
        if (__state is null || size.X <= 0 || size.Y <= 0) return;
        var draw = ImGui.GetWindowDrawList();
        float padding = MathF.Min(4f * ConsoleStyle.EffectiveInterfaceScale, size.X * 0.10f);
        float left = origin.X + padding, width = size.X - padding * 2f;
        uint background = ImGui.ColorConvertFloat4ToU32(new float4(0, 0, 0, 0.60f));
        draw.AddRectFilled(new float2(left, origin.Y), new float2(left + width, origin.Y + size.Y), background);
        float half = size.Y * 0.5f;
        CaptionPixelLayout.Text(draw, __state.Name, Color.White.AsByte4, left, origin.Y, width, half);
        CaptionPixelLayout.Text(draw, __state.Value, Color.White.AsByte4, left, origin.Y + half, width, half, right: true);
    }
}

// Only the four caption/value AddText calls identified by their adjacent
// inspected literals are replaced. Other sequence text and interaction IDs
// continue through the game's renderer.
[HarmonyPatch]
public static class StagingCaptionLayoutPatch
{
    public sealed class Bounds
    {
        public float Left, Width;
        public string? TotalCaption;
        public ImColor8 TotalColor;
        public ImFontPtr TotalFont;
        public float TotalY, TotalSize;
    }
    [ThreadStatic] private static Bounds? _bounds;

    public static IEnumerable<MethodBase> TargetMethods()
    {
        yield return AccessTools.Method(typeof(SequenceList), nameof(SequenceList.DrawSequenceContent));
        yield return AccessTools.Method(typeof(SequenceList), nameof(SequenceList.DrawTotalDeltaVFooter));
    }

    [HarmonyPrefix]
    public static void Capture(MethodBase __originalMethod, object[] __args, out Bounds? __state)
    {
        __state = _bounds;
        if (Runtime.SelectedLocale == "en-US") { _bounds = null; return; }
        float padding = ImGui.GetStyle().FramePadding.X;
        if (__originalMethod.Name == nameof(SequenceList.DrawTotalDeltaVFooter))
        {
            var min = (float2)__args[0]; var size = (float2)__args[1];
            _bounds = new Bounds { Left = min.X + padding, Width = MathF.Max(0.2f, size.X - padding * 2f) };
        }
        else _bounds = new Bounds { Left = ImGui.GetCursorScreenPos().X + padding,
            Width = MathF.Max(0.2f, ImGui.GetContentRegionAvail().X - padding * 2f) };
    }

    [HarmonyFinalizer]
    public static Exception? Restore(Exception? __exception, Bounds? __state)
    { _bounds = __state; return __exception; }

    public static void Isp(ImDrawListPtr draw, in float2 pos, ImColor8 color, ImString text)
        => Metric(draw, pos, color, text, false);
    public static void Twr(ImDrawListPtr draw, in float2 pos, ImColor8 color, ImString text)
        => Metric(draw, pos, color, text, true);
    private static void Metric(ImDrawListPtr draw, in float2 pos, ImColor8 color, ImString text, bool right)
    {
        if (_bounds is not { } b) { draw.AddText(in pos, color, text); return; }
        float gap = MathF.Min(4f * ConsoleStyle.EffectiveInterfaceScale, b.Width * 0.05f);
        float column = MathF.Max(0.1f, (b.Width - gap) * 0.5f);
        CaptionPixelLayout.Text(draw, Encoding.UTF8.GetString(text.AsSpan()), color, b.Left + (right ? column + gap : 0),
            pos.Y, column, ImGui.GetTextLineHeight(), right);
    }

    public static void TotalCaption(ImDrawListPtr draw, in float2 pos, ImColor8 color, ImString text)
    {
        if (_bounds is not { } b) { draw.AddText(in pos, color, text); return; }
        b.TotalCaption = Encoding.UTF8.GetString(text.AsSpan()); b.TotalColor = color; b.TotalY = pos.Y; b.TotalSize = ImGui.GetFontSize();
        b.TotalFont = ImGui.GetFont();
    }
    public static void TotalValue(ImDrawListPtr draw, in float2 pos, ImColor8 color, ImString text)
    {
        if (_bounds is not { } b || b.TotalCaption is null) { draw.AddText(in pos, color, text); return; }
        float gap = MathF.Min(5f * ConsoleStyle.EffectiveInterfaceScale, b.Width * 0.05f);
        var font = ImGui.GetFont(); float size = ImGui.GetFontSize();
        var widths = CaptionPixelLayout.TotalWidths(b.Width, font.CalcTextSizeA(size, float.MaxValue, 0, text).X, gap);
        CaptionPixelLayout.Text(draw, b.TotalCaption, b.TotalColor, b.Left, b.TotalY,
            widths.Caption, b.TotalSize, maxSize: b.TotalSize, fontOverride: b.TotalFont);
        CaptionPixelLayout.Text(draw, Encoding.UTF8.GetString(text.AsSpan()), color, b.Left + widths.Caption + gap, pos.Y,
            widths.Value, size, right: true, maxSize: size);
        b.TotalCaption = null;
    }

    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> Translate(IEnumerable<CodeInstruction> instructions, MethodBase __originalMethod)
    {
        string? pending = null; int count = 0;
        foreach (var instruction in instructions)
        {
            if (instruction.opcode == OpCodes.Ldstr && instruction.operand is string literal)
                pending = literal switch { "Isp " => nameof(Isp), "TWR " => nameof(Twr), "TOTAL dV" => nameof(TotalCaption), _ => pending };
            if (pending is not null && instruction.operand is MethodInfo method && method.Name == "AddText"
                && method.DeclaringType == typeof(ImDrawListExtensions))
            {
                var parameters = method.GetParameters().Select(p => p.ParameterType).ToArray();
                var expected = new[] { typeof(ImDrawListPtr), typeof(float2).MakeByRefType(), typeof(ImColor8), typeof(ImString) };
                if (!parameters.SequenceEqual(expected)) throw new InvalidOperationException("5638 staging AddText signature changed.");
                string replacement = pending;
                pending = pending == nameof(TotalCaption) ? nameof(TotalValue) : null;
                instruction.opcode = OpCodes.Call;
                instruction.operand = AccessTools.Method(typeof(StagingCaptionLayoutPatch), replacement);
                count++;
            }
            yield return instruction;
        }
        int expectedCount = __originalMethod.Name == nameof(SequenceList.DrawSequenceContent) ? 4 : 2;
        if (count != expectedCount || pending is not null)
            throw new InvalidOperationException($"5638 staging layout changed: {__originalMethod.Name} {count}/{expectedCount}.");
    }
}
