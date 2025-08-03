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

        private const int HOTKEY_ID = 9000;
        private const int WM_HOTKEY = 0x0312;

        private HotkeyWindow? hotkeyWindow;
        private Action? hotkeyAction;
        private bool disposed = false;

        public void RegisterHotkey(int modifiers, Keys key, Action action)
        {
            if (hotkeyWindow != null)
            {
                UnregisterHotkey();
            }

            hotkeyAction = action;
            hotkeyWindow = new HotkeyWindow(this);
            
            bool success = RegisterHotKey(hotkeyWindow.Handle, HOTKEY_ID, modifiers, (int)key);
            if (!success)
            {
                throw new InvalidOperationException("Could not register the hot key.");
            }
        }

        public void UnregisterHotkey()
        {
            if (hotkeyWindow != null)
            {
                UnregisterHotKey(hotkeyWindow.Handle, HOTKEY_ID);
                hotkeyWindow.Dispose();
                hotkeyWindow = null;
            }
            hotkeyAction = null;
        }

        internal void OnHotKeyPressed()
        {
            hotkeyAction?.Invoke();
        }

        public void Dispose()
        {
            if (!disposed)
            {
                UnregisterHotkey();
                disposed = true;
            }
        }

        private class HotkeyWindow : NativeWindow, IDisposable
        {
            private GlobalHotkey parent;

            public HotkeyWindow(GlobalHotkey parent)
            {
                this.parent = parent;
                CreateHandle(new CreateParams());
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == WM_HOTKEY)
                {
                    parent.OnHotKeyPressed();
                }
                base.WndProc(ref m);
            }

            public void Dispose()
            {
                if (Handle != IntPtr.Zero)
                {
                    DestroyHandle();
                }
            }
        }
    }
}