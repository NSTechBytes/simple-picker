using System.Windows.Forms;

namespace simple_picker
{
    public class Settings
    {
        // Color Picker Hotkey - Ctrl+Shift+C
        public Keys HotkeyKey { get; set; } = Keys.C;
        public int HotkeyModifiers { get; set; } = 6; // MOD_CONTROL (2) + MOD_SHIFT (4) = 6
        
        // Color Selector Hotkey - Ctrl+Shift+S
        public Keys ColorSelectorHotkeyKey { get; set; } = Keys.S;
        public int ColorSelectorHotkeyModifiers { get; set; } = 6; // MOD_CONTROL (2) + MOD_SHIFT (4) = 6
        
        // Popup settings
        public int PopupX { get; set; } = -1; // -1 means center
        public int PopupY { get; set; } = -1; // -1 means center
        public bool TopMost { get; set; } = true;
        public int PopupDuration { get; set; } = 5000; // milliseconds
    }
}