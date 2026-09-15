using System;
using System.Collections.Generic;
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

        private const int HOTKEY_COLORPICKER_ID = 9000;
        private const int HOTKEY_COLORSELECTOR_ID = 9001;
        private const int WM_HOTKEY = 0x0312;

        private HotkeyWindow? hotkeyWindow;
        private Dictionary<int, Action> hotkeyActions = new Dictionary<int, Action>();
        private bool disposed = false;

        public void RegisterColorPickerHotkey(int modifiers, Keys key, Action action)
        {
            RegisterHotkey(HOTKEY_COLORPICKER_ID, modifiers, key, action);
        }

        public void RegisterColorSelectorHotkey(int modifiers, Keys key, Action action)
        {
            RegisterHotkey(HOTKEY_COLORSELECTOR_ID, modifiers, key, action);
        }

        private void RegisterHotkey(int hotkeyId, int modifiers, Keys key, Action action)
        {
            if (hotkeyWindow == null)
            {
                hotkeyWindow = new HotkeyWindow(this);
            }

            // Unregister existing hotkey with same ID if it exists
            if (hotkeyActions.ContainsKey(hotkeyId))
            {
                UnregisterHotKey(hotkeyWindow.Handle, hotkeyId);
                hotkeyActions.Remove(hotkeyId);
            }

            bool success = RegisterHotKey(hotkeyWindow.Handle, hotkeyId, modifiers, (int)key);
            if (!success)
            {
                throw new InvalidOperationException($"Could not register the hot key with ID {hotkeyId}.");
            }

            hotkeyActions[hotkeyId] = action;
        }

        public void UnregisterAllHotkeys()
        {
            if (hotkeyWindow != null)
            {
                foreach (var hotkeyId in hotkeyActions.Keys)
                {
                    UnregisterHotKey(hotkeyWindow.Handle, hotkeyId);
                }
                hotkeyActions.Clear();
                hotkeyWindow.Dispose();
                hotkeyWindow = null;
            }
        }

        internal void OnHotKeyPressed(int hotkeyId)
        {
            if (hotkeyActions.TryGetValue(hotkeyId, out Action? action))
            {
                action?.Invoke();
            }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                UnregisterAllHotkeys();
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
                    int hotkeyId = m.WParam.ToInt32();
                    parent.OnHotKeyPressed(hotkeyId);
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