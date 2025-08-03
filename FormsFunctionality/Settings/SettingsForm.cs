using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Reflection;

namespace simple_picker
{
    public partial class SettingsForm : Form
    {
        private Settings settings;
        private UpdateManager updateManager;

        public SettingsForm(Settings settings)
        {
            this.settings = settings;
            this.updateManager = new UpdateManager(settings, null);
            InitializeComponent();
            LoadSettingsToUI();
        }

        private void LoadSettingsToUI()
        {
            // Color Picker Hotkey
            hotkeyComboBox.SelectedItem = settings.HotkeyKey.ToString();
            controlModifierCheckBox.Checked = (settings.HotkeyModifiers & 2) != 0;
            altModifierCheckBox.Checked = (settings.HotkeyModifiers & 1) != 0;
            shiftModifierCheckBox.Checked = (settings.HotkeyModifiers & 4) != 0;

            // Color Selector Hotkey
            colorSelectorHotkeyComboBox.SelectedItem = settings.ColorSelectorHotkeyKey.ToString();
            colorSelectorControlModifierCheckBox.Checked = (settings.ColorSelectorHotkeyModifiers & 2) != 0;
            colorSelectorAltModifierCheckBox.Checked = (settings.ColorSelectorHotkeyModifiers & 1) != 0;
            colorSelectorShiftModifierCheckBox.Checked = (settings.ColorSelectorHotkeyModifiers & 4) != 0;

            // Popup Settings
            topMostCheckBox.Checked = settings.TopMost;
            durationNumericUpDown.Value = settings.PopupDuration;

            // Update Settings - Changed to use seconds
            autoUpdateCheckBox.Checked = settings.AutoCheckForUpdates;
            updateIntervalNumericUpDown.Value = settings.UpdateCheckIntervalSeconds;

            // Startup Settings
            runAtStartupCheckBox.Checked = settings.RunAtStartup;

            // Update last check display
            UpdateLastCheckLabel();
        }

        private void UpdateLastCheckLabel()
        {
            if (settings.LastUpdateCheck == DateTime.MinValue)
            {
                lastUpdateCheckLabel.Text = "Last check: Never";
            }
            else
            {
                lastUpdateCheckLabel.Text = $"Last check: {settings.LastUpdateCheck:MM/dd/yyyy HH:mm:ss}";
            }
        }

        private void SaveSettingsFromUI()
        {
            // Color Picker Hotkey
            if (Enum.TryParse(hotkeyComboBox.SelectedItem?.ToString(), out Keys key))
            {
                settings.HotkeyKey = key;
            }

            settings.HotkeyModifiers = 0;
            if (controlModifierCheckBox.Checked) settings.HotkeyModifiers |= 2;
            if (altModifierCheckBox.Checked) settings.HotkeyModifiers |= 1;
            if (shiftModifierCheckBox.Checked) settings.HotkeyModifiers |= 4;

            // Color Selector Hotkey
            if (Enum.TryParse(colorSelectorHotkeyComboBox.SelectedItem?.ToString(), out Keys colorSelectorKey))
            {
                settings.ColorSelectorHotkeyKey = colorSelectorKey;
            }

            settings.ColorSelectorHotkeyModifiers = 0;
            if (colorSelectorControlModifierCheckBox.Checked) settings.ColorSelectorHotkeyModifiers |= 2;
            if (colorSelectorAltModifierCheckBox.Checked) settings.ColorSelectorHotkeyModifiers |= 1;
            if (colorSelectorShiftModifierCheckBox.Checked) settings.ColorSelectorHotkeyModifiers |= 4;

            // Popup Settings
            settings.TopMost = topMostCheckBox.Checked;
            settings.PopupDuration = (int)durationNumericUpDown.Value;

            // Update Settings - Changed to use seconds
            settings.AutoCheckForUpdates = autoUpdateCheckBox.Checked;
            settings.UpdateCheckIntervalSeconds = (int)updateIntervalNumericUpDown.Value;

            // Startup Settings
            bool oldRunAtStartup = settings.RunAtStartup;
            settings.RunAtStartup = runAtStartupCheckBox.Checked;
            
            // Apply startup setting if it changed
            if (oldRunAtStartup != settings.RunAtStartup)
            {
                SetStartupRegistry(settings.RunAtStartup);
            }
        }

        private void SetStartupRegistry(bool enable)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
                {
                    if (key != null)
                    {
                        string appName = "SimplePicker";
                        if (enable)
                        {
                            string exePath = Assembly.GetExecutingAssembly().Location;
                            if (exePath.EndsWith(".dll"))
                            {
                                // For .NET applications, we need the exe path
                                exePath = exePath.Replace(".dll", ".exe");
                            }
                            key.SetValue(appName, $"\"{exePath}\"");
                        }
                        else
                        {
                            key.DeleteValue(appName, false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update startup settings: {ex.Message}", 
                    "Registry Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void checkForUpdatesButton_Click(object sender, EventArgs e)
        {
            checkForUpdatesButton.Enabled = false;
            checkForUpdatesButton.Text = "Checking...";

            try
            {
                var result = await updateManager.CheckForUpdatesAsync(showNoUpdateMessage: true);
                updateManager.ShowUpdateDialog(result);

                // Update the last check time display
                UpdateLastCheckLabel();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during update check: {ex.Message}", 
                    "Update Check Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                checkForUpdatesButton.Enabled = true;
                checkForUpdatesButton.Text = "Check for Updates";
            }
        }

        private void autoUpdateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            updateIntervalNumericUpDown.Enabled = autoUpdateCheckBox.Checked;
            updateIntervalLabel.Enabled = autoUpdateCheckBox.Checked;
        }

        private void resetToDefaultsButton_Click(object sender, EventArgs e)
        {
            var (appName, currentVersion, _) = settings.GetAppInfoFromRegistry();
            
            DialogResult result = MessageBox.Show(
                $"Are you sure you want to reset all settings to their default values?\n\n" +
                $"This will also reset the version in registry to 1.0 (current: {currentVersion})",
                "Reset Settings",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                settings.ResetToDefaults();
                LoadSettingsToUI();
                
                MessageBox.Show("Settings have been reset to defaults.\nRegistry version has been reset to 1.0.", 
                    "Settings Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            SaveSettingsFromUI();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void lastUpdateCheckLabel_Click(object sender, EventArgs e)
        {
            // Show detailed version information when clicked
            string versionInfo = updateManager.GetVersionInfo();
            MessageBox.Show(versionInfo, "Version Information", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Method to manually update registry version (for testing/debugging)
        private void UpdateRegistryVersion(string newVersion)
        {
            if (settings.SetVersionInRegistry(newVersion))
            {
                MessageBox.Show($"Registry version updated to: {newVersion}", 
                    "Version Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Failed to update registry version.", 
                    "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}