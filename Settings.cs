global using static JeekNoteExplorer.SettingsSingletonContainer;
using Newtonsoft.Json;

namespace JeekNoteExplorer;

public class AppSettings
{
    public static readonly string ExePath = Application.ExecutablePath;
    public static readonly string ExeDirectory = Path.GetDirectoryName(ExePath)!;
    public static readonly string SettingsFilePath = Path.ChangeExtension(ExePath, ".json");

    public static void Load()
    {
        if (!File.Exists(SettingsFilePath))
            return;

        var settings = JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(SettingsFilePath));
        if (settings != null)
            Settings = settings;
    }

    public static void Save()
    {
        File.WriteAllText(SettingsFilePath, JsonConvert.SerializeObject(Settings, Formatting.Indented));
    }

    public string NoteFolder { get; set; } = "";
}

public static class SettingsSingletonContainer
{
    public static AppSettings Settings { get; set; } = new();
}
