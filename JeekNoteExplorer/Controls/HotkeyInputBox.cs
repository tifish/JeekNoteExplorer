// 0.2 2025 Winston Feng
// HotkeyInputBox 0.1 (c) 2011 Richard Z.H. Wang
// MIT licensed.

using System.ComponentModel;

namespace JeekNoteExplorer.Controls
{
    class HotkeyInputBox : TextBox
    {
        public HotkeyInputBox() { }

        #region Properties to hide from the designer
        [Browsable(false)]
        public new string[] Lines { get { return [Text]; } private set { base.Lines = value; } }
        [Browsable(false)]
        public override bool Multiline { get { return false; } }
        [Browsable(false)]
        public new char PasswordChar { get; set; }
        [Browsable(false)]
        public new ScrollBars ScrollBars { get; set; }
        [Browsable(false)]
        public override bool ShortcutsEnabled { get { return false; } }
        // [Browsable(false)]
        // public override string Text { get { return base.Text; } set { base.Text = value ?? ""; } }
        [Browsable(false)]
        public new bool WordWrap { get; set; }
        #endregion

        #region Focus detection - use this to stop hotkeys being triggered in your code
        private static Control? FindFocusedControl(Control? control)
        {
            var container = control as ContainerControl;
            while (container != null)
            {
                control = container.ActiveControl;
                container = control as ContainerControl;
            }
            return control;
        }
        public bool IsFocused { get { return FindFocusedControl(Form.ActiveForm) == this; } }
        public static bool TypeIsFocused { get { return FindFocusedControl(Form.ActiveForm) is HotkeyInputBox; } }
        #endregion

        public Hotkey Hotkey
        {
            get;
            set
            {
                field = value;
                RefreshText();
            }
        } = new();

        public Keys KeyCode { get { return Hotkey.KeyCode; } set { Hotkey.KeyCode = value; } }
        public bool Windows { get { return Hotkey.Windows; } set { Hotkey.Windows = value; } }
        public bool Control { get { return Hotkey.Control; } set { Hotkey.Control = value; } }
        public bool Alt { get { return Hotkey.Alt; } set { Hotkey.Alt = value; } }
        public bool Shift { get { return Hotkey.Shift; } set { Hotkey.Shift = value; } }

        public void Reset()
        {
            KeyCode = Keys.None;
            Windows = false;
            Control = false;
            Alt = false;
            Shift = false;
        }

        private void RefreshText()
        {
            Text = Hotkey.ToString();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            RefreshText();
            base.OnPaint(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.None)
                SelectAll();
            base.OnMouseMove(e);
        }

        public event EventHandler? HotkeyChanged;

        protected virtual void OnHotkeyChanged(EventArgs e)
        {
            HotkeyChanged?.Invoke(this, e);
        }

        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x101;
        const int WM_CHAR = 0x102;
        const int WM_SYSCHAR = 0x106;
        const int WM_SYSKEYDOWN = 0x104;
        const int WM_SYSKEYUP = 0x105;
        const int WM_IME_CHAR = 0x286;

        private int _keysPressed = 0;
        protected override bool ProcessKeyMessage(ref Message m)
        {
            if (m.Msg == WM_KEYUP || m.Msg == WM_SYSKEYUP)
            {
                _keysPressed--;

                if (_keysPressed == 0)
                    OnHotkeyChanged(new EventArgs());
            }

            if (m.Msg != WM_CHAR && m.Msg != WM_SYSCHAR && m.Msg != WM_IME_CHAR)
            {
                KeyEventArgs e = new(((Keys)(int)(long)m.WParam) | ModifierKeys);

                if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                    Reset();
                else
                {
                    // Print Screen doesn't seem to be part of WM_KEYDOWN/WM_SYSKEYDOWN...
                    if (m.Msg == WM_KEYDOWN || m.Msg == WM_SYSKEYDOWN || e.KeyCode == Keys.PrintScreen)
                    {
                        // Start over if we had no keys pressed, or have a selection (since it's always select all)
                        if (_keysPressed < 1 || SelectionLength > 0)
                            Reset();

                        //if (e.KeyCode )
                        //    this.Windows = true;

                        Control = e.Control;
                        Shift = e.Shift;
                        Alt = e.Alt;

                        if (e.KeyCode != Keys.ShiftKey
                            && e.KeyCode != Keys.ControlKey
                            && e.KeyCode != Keys.Menu)
                            KeyCode = e.KeyCode;

                        _keysPressed++;
                    }
                }

                // Pretty readable output
                RefreshText();

                // Select the end of our textbox
                Select(TextLength, 0);
            }

            return true;
        }
    }
}

public class Hotkey
{
    public Keys KeyCode { get; set; }
    public bool Windows { get; set; }
    public bool Control { get; set; }
    public bool Alt { get; set; }
    public bool Shift { get; set; }

    public Hotkey Clone()
    {
        return new Hotkey
        {
            KeyCode = KeyCode,
            Windows = Windows,
            Control = Control,
            Alt = Alt,
            Shift = Shift
        };
    }

    public override string ToString()
    {
        return $"{(Windows ? "Win+" : "")}{(Control ? "Ctrl+" : "")}{(Alt ? "Alt+" : "")}{(Shift ? "Shift+" : "")}{KeyCodeToString(KeyCode)}";
    }

    public static string KeyCodeToString(Keys keyCode)
    {
        return keyCode switch
        {
            Keys.None => "",
            Keys.Back => "Backspace",
            Keys.Oemtilde => "`",
            Keys.D0 => "0",
            Keys.D1 => "1",
            Keys.D2 => "2",
            Keys.D3 => "3",
            Keys.D4 => "4",
            Keys.D5 => "5",
            Keys.D6 => "6",
            Keys.D7 => "7",
            Keys.D8 => "8",
            Keys.D9 => "9",
            Keys.OemMinus => "-",
            Keys.Oemplus => "=",
            Keys.OemOpenBrackets => "[",
            Keys.OemCloseBrackets => "]",
            Keys.OemSemicolon => ";",
            Keys.OemQuotes => "'",
            Keys.OemPipe => "\\",
            Keys.Oemcomma => ",",
            Keys.OemPeriod => ".",
            Keys.Oem2 => "/",

            _ => keyCode.ToString(),
        };
    }

    public static Keys ParseKeyCode(string keyCodeString)
    {
        return keyCodeString switch
        {
            "Backspace" => Keys.Back,
            "`" => Keys.Oemtilde,
            "0" => Keys.D0,
            "1" => Keys.D1,
            "2" => Keys.D2,
            "3" => Keys.D3,
            "4" => Keys.D4,
            "5" => Keys.D5,
            "6" => Keys.D6,
            "7" => Keys.D7,
            "8" => Keys.D8,
            "9" => Keys.D9,
            "-" => Keys.OemMinus,
            "=" => Keys.Oemplus,
            "[" => Keys.OemOpenBrackets,
            "]" => Keys.OemCloseBrackets,
            ";" => Keys.OemSemicolon,
            "'" => Keys.OemQuotes,
            "\\" => Keys.OemPipe,
            "," => Keys.Oemcomma,
            "." => Keys.OemPeriod,
            "/" => Keys.Oem2,

            _ => Enum.TryParse(typeof(Keys), keyCodeString, out var key) ? (Keys)key : Keys.None,
        };
    }

    public static Hotkey Parse(string hotkeyString)
    {
        var hotkeyParts = hotkeyString.Split('+');
        var hotkey = new Hotkey();

        foreach (var part in hotkeyParts)
        {
            switch (part)
            {
                case "Win":
                    hotkey.Windows = true;
                    break;
                case "Ctrl":
                    hotkey.Control = true;
                    break;
                case "Alt":
                    hotkey.Alt = true;
                    break;
                case "Shift":
                    hotkey.Shift = true;
                    break;
                default:
                    hotkey.KeyCode = ParseKeyCode(part);
                    break;
            }
        }

        return hotkey;
    }

    public Keys ToKeys()
    {
        var keys = Keys.None;
        if (Windows) keys |= Keys.LWin;
        if (Control) keys |= Keys.Control;
        if (Alt) keys |= Keys.Alt;
        if (Shift) keys |= Keys.Shift;
        keys |= KeyCode;
        return keys;
    }
}
