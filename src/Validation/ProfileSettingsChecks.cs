using KsaChinese;

internal static partial class Program
{
    static void ProfileSettingsChecks()
    {
        var fixture = Path.Combine(Fixture, "locale-" + Guid.NewGuid().ToString("N"));
        var first = Path.Combine(fixture, "first", "settings.json");
        var second = Path.Combine(fixture, "second", "settings.json");
        string[] available = ["zh-CN", "en-US"];
        var diagnostics = new List<string>();
        Check(Runtime.SettingsPath == Path.Combine(Fixture, "KsaBabel", "settings.json"), "语言设置使用当前档案路径");
        Check(LocaleSettings.Read(first, "zh-CN", available, diagnostics.Add) == "zh-CN", "无个人语言设置使用包默认值");
        LocaleSettings.Write(first, "en-US", diagnostics.Add);
        Check(LocaleSettings.Read(first, "zh-CN", available, diagnostics.Add) == "en-US", "重读语言选择可恢复英文");
        Check(LocaleSettings.Read(second, "zh-CN", available, diagnostics.Add) == "zh-CN", "另一个实例保持独立默认值");
        LocaleSettings.Write(first, "zh-CN", diagnostics.Add);
        Check(LocaleSettings.Read(first, "en-US", available, diagnostics.Add) == "zh-CN", "替换旧语言选择并恢复中文");
        File.WriteAllText(first, "{broken");
        Check(LocaleSettings.Read(first, "zh-CN", available, diagnostics.Add) == "zh-CN", "损坏设置回退且不禁用汉化");
        File.WriteAllText(first, "{\"locale\":\"unknown\"}");
        Check(LocaleSettings.Read(first, "zh-CN", available, diagnostics.Add) == "zh-CN", "未知个人语言回退");
        File.WriteAllText(first, "{\"locale\":42}");
        Check(LocaleSettings.Read(first, "zh-CN", available, diagnostics.Add) == "zh-CN", "错误设置类型回退");
        var fileAsFolder = Path.Combine(fixture, "blocked");
        File.WriteAllText(fileAsFolder, "fixture");
        LocaleSettings.Write(Path.Combine(fileAsFolder, "settings.json"), "en-US", diagnostics.Add);
        Check(diagnostics.Count == 4, "不可写设置不抛异常并记录原因");
        Check(!Directory.EnumerateFiles(fixture, "*.tmp", SearchOption.AllDirectories).Any(), "原子写入不遗留临时文件");

        var legacy = Path.Combine(fixture, "legacy", "settings.json");
        var migrated = Path.Combine(fixture, "babel", "settings.json");
        Directory.CreateDirectory(Path.GetDirectoryName(legacy)!);
        File.WriteAllText(legacy, "{\"locale\":\"en-US\",\"unrelated\":\"do not import\"}");
        var original = File.ReadAllBytes(legacy);
        Check(LocaleSettings.ReadWithMigration(migrated, legacy, "zh-CN", available, diagnostics.Add) == "en-US",
            "首次启动继承同档案旧模组的有效语言");
        Check(File.ReadAllBytes(legacy).SequenceEqual(original), "迁移不改旧设置字节");
        Check(!File.ReadAllText(migrated).Contains("unrelated"), "仅迁移语言而不复制其他旧配置");
        LocaleSettings.Write(migrated, "zh-CN", diagnostics.Add);
        Check(LocaleSettings.ReadWithMigration(migrated, legacy, "en-US", available, diagnostics.Add) == "zh-CN",
            "已有Babel设置优先于旧设置");
        File.WriteAllText(migrated, "{broken");
        Check(LocaleSettings.ReadWithMigration(migrated, legacy, "zh-CN", available, diagnostics.Add) == "zh-CN"
            && File.ReadAllText(migrated) == "{broken", "已有但损坏的新设置保留并回退，不重新覆盖");
        File.Delete(migrated);
        File.WriteAllText(legacy, "{\"locale\":\"unknown\"}");
        Check(LocaleSettings.ReadWithMigration(migrated, legacy, "zh-CN", available, diagnostics.Add) == "zh-CN"
            && !File.Exists(migrated), "无效旧语言不创建新设置");
        File.WriteAllText(legacy, "{broken");
        Check(LocaleSettings.ReadWithMigration(migrated, legacy, "zh-CN", available, diagnostics.Add) == "zh-CN"
            && !File.Exists(migrated), "损坏旧设置安全回退且不写入");
        File.Delete(legacy);
        Check(LocaleSettings.ReadWithMigration(migrated, legacy, "zh-CN", available, diagnostics.Add) == "zh-CN"
            && !File.Exists(migrated), "全新档案无迁移副作用");
        File.WriteAllText(legacy, "{\"locale\":\"en-US\"}");
        Check(LocaleSettings.ReadWithMigration(Path.Combine(fileAsFolder, "settings.json"), legacy,
            "zh-CN", available, diagnostics.Add) == "en-US", "迁移写入失败仍可本次使用旧语言");
        Check(!Directory.EnumerateFiles(fixture, "*.tmp", SearchOption.AllDirectories).Any(), "迁移不遗留临时文件");
        Check(LanguagePlugin.HasLegacyConflict([new KSA.ModEntry { Id = "KsaChinese", Enabled = true }]), "启用旧ID触发冲突保护");
        Check(LanguagePlugin.HasLegacyConflict([new KSA.ModEntry { Id = "ksachinese", Enabled = true }]), "旧ID冲突判断不区分大小写");
        Check(!LanguagePlugin.HasLegacyConflict([new KSA.ModEntry { Id = "KsaChinese", Enabled = false }]), "禁用旧ID允许Babel启用");
        Check(!LanguagePlugin.HasLegacyConflict([new KSA.ModEntry { Id = "KsaBabel", Enabled = true }]), "独立新ID不误报冲突");
        var previousManifest = KSA.ModLibrary.Manifest;
        var previousProvider = Brutal.Localization.LStrings.LocalizedStringProvider;
        try
        {
            KSA.ModLibrary.Manifest = new KSA.ModManifest();
            KSA.ModLibrary.Manifest.Mods.Add(new KSA.ModEntry { Id = "KsaChinese", Enabled = true });
            new LanguagePlugin().BeforeMain();
            Check(!Runtime.IsActive && Runtime.DisabledReason?.Contains("KsaChinese") == true,
                "实际BeforeMain入口在旧ID启用时拒绝激活");
            Check(Runtime.Packs.Count == 0 && ReferenceEquals(previousProvider, Brutal.Localization.LStrings.LocalizedStringProvider),
                "冲突入口在读取语言包和修改原生提供器之前返回");
        }
        finally { KSA.ModLibrary.Manifest = previousManifest; }
    }
}
