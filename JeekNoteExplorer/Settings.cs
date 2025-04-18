global using static JeekNoteExplorer.SettingsSingletonContainer;
using JeekTools;
using Newtonsoft.Json;

namespace JeekNoteExplorer;

public class AppSettings
{
    public static readonly string ExePath = Application.ExecutablePath;
    public static readonly string AppName = Path.GetFileNameWithoutExtension(ExePath);
    public static readonly string ExeDirectory = Path.GetDirectoryName(ExePath)!;

    public static readonly string SettingsFilePath =
        Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE")!, @"AppData\Local", AppName, "Settings.json");

    public static readonly string NewFilesDirectory = Path.Combine(ExeDirectory, "NewFiles");

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
        Directory.CreateDirectory(Path.GetDirectoryName(SettingsFilePath)!);
        File.WriteAllText(SettingsFilePath, JsonConvert.SerializeObject(Settings, Formatting.Indented));
    }

    private string _noteFolder = "";

    public string NoteFolder
    {
        get => _noteFolder;
        set
        {
            if (_noteFolder == value)
                return;

            _noteFolder = value;
            RootFolder.Root.FullPath = value;
            RootFolder.Refresh();
        }
    }

    private const string RunRegistryKey = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

    public bool StartWithSystem
    {
        get => RegistryHelper.GetValue(RunRegistryKey, AppName, "") == ExePath;
        set
        {
            if (value)
                RegistryHelper.SetValue(RunRegistryKey, AppName, ExePath);
            else
                RegistryHelper.DeleteValue(RunRegistryKey, AppName);
        }
    }
}

public static class SettingsSingletonContainer
{
    public static AppSettings Settings { get; set; } = new();
}
