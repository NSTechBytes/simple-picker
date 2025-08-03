using System.Windows.Forms;
using Microsoft.Win32;

namespace simple_picker
{
    public class Settings
    {
        // Registry constants
        private const string REGISTRY_KEY_PATH = @"Software\SimplePicker";
        private const string VERSION_VALUE_NAME = "Version";
        private const string APP_NAME_VALUE_NAME = "AppName";
        private const string PUBLISHER_VALUE_NAME = "Publisher";
        
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
        
        // Update settings - Changed from days to seconds
        public bool AutoCheckForUpdates { get; set; } = true;
        public int UpdateCheckIntervalSeconds { get; set; } = 30; // Check every 30 seconds
        public DateTime LastUpdateCheck { get; set; } = DateTime.MinValue;
        public string UpdateUrl { get; set; } = "https://raw.githubusercontent.com/NSTechBytes/simple-picker/refs/heads/main/.github/version.ini";
        
        // Session tracking for update dialog
        public bool UpdateDialogShownThisSession { get; set; } = false;
        
        // Startup settings
        public bool RunAtStartup { get; set; } = false;

        // Property to get current version from registry
        public string CurrentVersion
        {
            get => GetVersionFromRegistry();
        }

        /// <summary>
        /// Gets the current version from Windows Registry
        /// </summary>
        /// <returns>Version string from registry or "1.0" as fallback</returns>
        private string GetVersionFromRegistry()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(REGISTRY_KEY_PATH))
                {
                    if (key != null)
                    {
                        string version = key.GetValue(VERSION_VALUE_NAME)?.ToString();
                        if (!string.IsNullOrEmpty(version))
                        {
                            return version;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error if needed, but don't throw
                System.Diagnostics.Debug.WriteLine($"Error reading version from registry: {ex.Message}");
            }
            
            // Fallback to default version if registry read fails
            return "1.0";
        }

        /// <summary>
        /// Sets the current version in Windows Registry
        /// </summary>
        /// <param name="version">Version string to set</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool SetVersionInRegistry(string version)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(REGISTRY_KEY_PATH))
                {
                    if (key != null)
                    {
                        key.SetValue(VERSION_VALUE_NAME, version);
                        key.SetValue(APP_NAME_VALUE_NAME, "SimplePicker");
                        key.SetValue(PUBLISHER_VALUE_NAME, "nstechbytes");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error writing version to registry: {ex.Message}");
            }
            
            return false;
        }

        /// <summary>
        /// Gets application information from registry
        /// </summary>
        /// <returns>Tuple containing AppName, Version, and Publisher</returns>
        public (string AppName, string Version, string Publisher) GetAppInfoFromRegistry()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(REGISTRY_KEY_PATH))
                {
                    if (key != null)
                    {
                        string appName = key.GetValue(APP_NAME_VALUE_NAME)?.ToString() ?? "SimplePicker";
                        string version = key.GetValue(VERSION_VALUE_NAME)?.ToString() ?? "1.0";
                        string publisher = key.GetValue(PUBLISHER_VALUE_NAME)?.ToString() ?? "nstechbytes";
                        
                        return (appName, version, publisher);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reading app info from registry: {ex.Message}");
            }
            
            // Return defaults if registry read fails
            return ("SimplePicker", "1.0", "nstechbytes");
        }

        /// <summary>
        /// Initializes registry with default values if they don't exist
        /// </summary>
        public void InitializeRegistryIfNeeded()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(REGISTRY_KEY_PATH))
                {
                    if (key == null)
                    {
                        // Registry key doesn't exist, create it with default values
                        SetVersionInRegistry("1.0");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error initializing registry: {ex.Message}");
            }
        }

        // Method to reset settings to defaults
        public void ResetToDefaults()
        {
            HotkeyKey = Keys.C;
            HotkeyModifiers = 6; // MOD_CONTROL (2) + MOD_SHIFT (4) = 6
            ColorSelectorHotkeyKey = Keys.S;
            ColorSelectorHotkeyModifiers = 6; // MOD_CONTROL (2) + MOD_SHIFT (4) = 6
            PopupX = -1;
            PopupY = -1;
            TopMost = true;
            PopupDuration = 5000;
            AutoCheckForUpdates = true;
            UpdateCheckIntervalSeconds = 30;
            LastUpdateCheck = DateTime.MinValue;
            UpdateUrl = "https://raw.githubusercontent.com/NSTechBytes/simple-picker/refs/heads/main/.github/version.ini";
            UpdateDialogShownThisSession = false;
            RunAtStartup = false;
            
            // Reset registry version to default
            SetVersionInRegistry("1.0");
        }
    }
}