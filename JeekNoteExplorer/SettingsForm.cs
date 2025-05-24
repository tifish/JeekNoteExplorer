using BlueMystic;

namespace JeekNoteExplorer;

public partial class SettingsForm : Form
{
    private readonly DarkModeCS _darkMode;

    public SettingsForm()
    {
        InitializeComponent();
        _darkMode = new DarkModeCS(this);
    }

    private void closeButton_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        Settings.NoteFolder = noteFolderTextBox.Text;
        Settings.StartWithSystem = startWithSystemCheckBox.Checked;
        Settings.WakeUpKey = wakeUpHotkeyInputBox.Hotkey.ToString();

        AppSettings.Save();

        MainForm.Instance!.RegisterWakeUpHotkey(wakeUpHotkeyInputBox.Hotkey);
    }

    private void SettingsForm_Load(object sender, EventArgs e)
    {
        noteFolderTextBox.Text = Settings.NoteFolder;
        startWithSystemCheckBox.Checked = Settings.StartWithSystem;
        wakeUpHotkeyInputBox.Hotkey = Hotkey.Parse(Settings.WakeUpKey);
    }
}
