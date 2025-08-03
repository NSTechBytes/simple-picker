using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace simple_picker
{
    public class GlobalHotkey : IDisposable
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const int WM_HOTKEY = 0x0312;
        private const int HOTKEY_ID = 9000;

        private HotkeyWindow hotkeyWindow;
        private Action hotkeyAction;

        public void RegisterHotkey(int modifiers, Keys key, Action action)
        {
            hotkeyAction = action;
            hotkeyWindow = new HotkeyWindow(this);
            RegisterHotKey(hotkeyWindow.Handle, HOTKEY_ID, modifiers, (int)key);
        }

        internal void OnHotkey()
        {
            hotkeyAction?.Invoke();
        }

        public void Dispose()
        {
            if (hotkeyWindow != null)
            {
                UnregisterHotKey(hotkeyWindow.Handle, HOTKEY_ID);
                hotkeyWindow.Dispose();
            }
        }

        private class HotkeyWindow : Form
        {
            private GlobalHotkey parent;

            public HotkeyWindow(GlobalHotkey parent)
            {
                this.parent = parent;
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
                this.Visible = false;
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == WM_HOTKEY)
                {
                    parent.OnHotkey();
                }
                base.WndProc(ref m);
            }
        }
    }
}