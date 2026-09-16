using System.Windows.Forms;
using Microsoft.Win32;
using System;
using System.Reflection;

namespace simple_picker
{
    public class Settings
    {
        // Registry constants (startup only - version comes from assembly metadata)
        private const string STARTUP_REGISTRY_PATH = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        private const string STARTUP_APP_NAME = "SimplePicker";

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
        public bool ShowPopupOnPick { get; set; } = true;

        // Auto-copy settings
        public bool AutoCopyEnabled { get; set; } = true;
        public ColorFormat AutoCopyFormat { get; set; } = ColorFormat.Hex;
        public bool ShowCopyNotification { get; set; } = false;

        // Update settings
        public bool AutoCheckForUpdates { get; set; } = true;
        public int UpdateCheckIntervalSeconds { get; set; } = 30;
        public DateTime LastUpdateCheck { get; set; } = DateTime.MinValue;
        public string UpdateUrl { get; set; } = "https://raw.githubusercontent.com/NSTechBytes/simple-picker/refs/heads/main/.github/version.ini";

        // Session tracking for update dialog
        public bool UpdateDialogShownThisSession { get; set; } = false;

        // Startup settings - Property that checks registry directly
        public bool RunAtStartup
        {
            get => GetRunAtStartupFromRegistry();
            set => SetRunAtStartupInRegistry(value);
        }

        /// <summary>
        /// Gets the current application version from assembly metadata.
        /// This reads the version embedded at build time via the .csproj Version property.
        /// </summary>
        public string CurrentVersion
        {
            get
            {
                try
                {
                    var version = Assembly.GetEntryAssembly()?.GetName().Version;
                    if (version != null)
                        return $"{version.Major}.{version.Minor}.{version.Build}";

                    version = Assembly.GetExecutingAssembly().GetName().Version;
                    if (version != null)
                        return $"{version.Major}.{version.Minor}.{version.Build}";
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error reading version from assembly: {ex.Message}");
                }

                return "1.0";
            }
        }

        /// <summary>
        /// Gets application display name from assembly metadata.
        /// </summary>
        public string AppName
        {
            get
            {
                try
                {
                    var name = Assembly.GetEntryAssembly()?.GetName().Name
                            ?? Assembly.GetExecutingAssembly().GetName().Name;
                    return name ?? "SimplePicker";
                }
                catch
                {
                    return "SimplePicker";
                }
            }
        }

        /// <summary>
        /// Gets the run at startup setting directly from Windows Registry.
        /// </summary>
        private bool GetRunAtStartupFromRegistry()
        {
            try
            {
                using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(STARTUP_REGISTRY_PATH))
                {
                    if (key != null)
                    {
                        string? startupValue = key.GetValue(STARTUP_APP_NAME)?.ToString();
                        return !string.IsNullOrEmpty(startupValue);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reading startup setting from registry: {ex.Message}");
            }

            return false;
        }

        /// <summary>
        /// Sets the run at startup setting directly in Windows Registry.
        /// </summary>
        private bool SetRunAtStartupInRegistry(bool enable)
        {
            try
            {
                using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(STARTUP_REGISTRY_PATH, true))
                {
                    if (key != null)
                    {
                        if (enable)
                        {
                            string exePath = Assembly.GetExecutingAssembly().Location;
                            exePath = exePath.Replace(".dll", ".exe");
                            key.SetValue(STARTUP_APP_NAME, $"\"{exePath}\"");
                        }
                        else
                        {
                            key.DeleteValue(STARTUP_APP_NAME, false);
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error setting startup registry: {ex.Message}");
            }

            return false;
        }

        // Method to reset settings to defaults
        public void ResetToDefaults()
        {
            HotkeyKey = Keys.C;
            HotkeyModifiers = 6;
            ColorSelectorHotkeyKey = Keys.S;
            ColorSelectorHotkeyModifiers = 6;
            PopupX = -1;
            PopupY = -1;
            TopMost = true;
            PopupDuration = 5000;
            ShowPopupOnPick = true;
            AutoCopyEnabled = true;
            AutoCopyFormat = ColorFormat.Hex;
            ShowCopyNotification = false;
            AutoCheckForUpdates = true;
            UpdateCheckIntervalSeconds = 30;
            LastUpdateCheck = DateTime.MinValue;
            UpdateUrl = "https://raw.githubusercontent.com/NSTechBytes/simple-picker/refs/heads/main/.github/version.ini";
            UpdateDialogShownThisSession = false;

            RunAtStartup = false;
        }
    }
}
