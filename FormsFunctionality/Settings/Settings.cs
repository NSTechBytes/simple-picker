using System.Windows.Forms;

namespace simple_picker
{
    public class Settings
    {
        public Keys HotkeyKey { get; set; } = Keys.F1;
        public int HotkeyModifiers { get; set; } = 2; // MOD_CONTROL
        public int PopupX { get; set; } = -1; // -1 means center
        public int PopupY { get; set; } = -1; // -1 means center
        public bool TopMost { get; set; } = true;
        public int PopupDuration { get; set; } = 5000; // milliseconds
    }
}