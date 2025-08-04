using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Reflection;
using System.Linq;

namespace simple_picker
{
    public partial class SettingsForm : Form
    {
        private Settings settings;
        private UpdateManager updateManager;

        public SettingsForm(Settings settings)
        {
            this.settings = settings;
            // The notification manager is not used in the settings form directly, so passing null.
            this.updateManager = new UpdateManager(settings, null); 
            InitializeComponent();
            LoadSettingsToUI();
        }

        private void LoadSettingsToUI()
        {
            // Hotkeys Tab
            hotkeyComboBox.SelectedItem = settings.HotkeyKey.ToString();
            controlModifierCheckBox.Checked = (settings.HotkeyModifiers & 2) != 0;
            altModifierCheckBox.Checked = (settings.HotkeyModifiers & 1) != 0;
            shiftModifierCheckBox.Checked = (settings.HotkeyModifiers & 4) != 0;

            colorSelectorHotkeyComboBox.SelectedItem = settings.ColorSelectorHotkeyKey.ToString();
            colorSelectorControlModifierCheckBox.Checked = (settings.ColorSelectorHotkeyModifiers & 2) != 0;
            colorSelectorAltModifierCheckBox.Checked = (settings.ColorSelectorHotkeyModifiers & 1) != 0;
            colorSelectorShiftModifierCheckBox.Checked = (settings.ColorSelectorHotkeyModifiers & 4) != 0;

            // General Tab
            topMostCheckBox.Checked = settings.TopMost;
            durationNumericUpDown.Value = settings.PopupDuration;
            showPopupOnPickCheckBox.Checked = settings.ShowPopupOnPick;
            autoCopyEnabledCheckBox.Checked = settings.AutoCopyEnabled;
            colorFormatComboBox.SelectedIndex = (int)settings.AutoCopyFormat;
            showCopyNotificationCheckBox.Checked = settings.ShowCopyNotification;
            runAtStartupCheckBox.Checked = settings.RunAtStartup; // This now reads directly from registry

            // Updates Tab
            autoUpdateCheckBox.Checked = settings.AutoCheckForUpdates;
            updateIntervalNumericUpDown.Value = settings.UpdateCheckIntervalSeconds;

            // Update UI states
            UpdateAutoCopyUIState();
            UpdateAutoUpdateUIState();
            UpdateLastCheckLabel();
        }

        private void UpdateAutoCopyUIState()
        {
            colorFormatComboBox.Enabled = autoCopyEnabledCheckBox.Checked;
            colorFormatLabel.Enabled = autoCopyEnabledCheckBox.Checked;
            showCopyNotificationCheckBox.Enabled = autoCopyEnabledCheckBox.Checked;
        }
        
        private void UpdateAutoUpdateUIState()
        {
            updateIntervalNumericUpDown.Enabled = autoUpdateCheckBox.Checked;
            updateIntervalLabel.Enabled = autoUpdateCheckBox.Checked;
        }

        private void UpdateLastCheckLabel()
        {
            if (settings.LastUpdateCheck == DateTime.MinValue)
            {
                lastUpdateCheckLabel.Text = "Last check: Never";
            }
            else
            {
                lastUpdateCheckLabel.Text = $"Last check: {settings.LastUpdateCheck:g}";
            }
        }

        private void SaveSettingsFromUI()
        {
            // Hotkeys Tab
            if (Enum.TryParse(hotkeyComboBox.SelectedItem?.ToString(), out Keys key))
            {
                settings.HotkeyKey = key;
            }
            settings.HotkeyModifiers = 0;
            if (controlModifierCheckBox.Checked) settings.HotkeyModifiers |= 2;
            if (altModifierCheckBox.Checked) settings.HotkeyModifiers |= 1;
            if (shiftModifierCheckBox.Checked) settings.HotkeyModifiers |= 4;

            if (Enum.TryParse(colorSelectorHotkeyComboBox.SelectedItem?.ToString(), out Keys colorSelectorKey))
            {
                settings.ColorSelectorHotkeyKey = colorSelectorKey;
            }
            settings.ColorSelectorHotkeyModifiers = 0;
            if (colorSelectorControlModifierCheckBox.Checked) settings.ColorSelectorHotkeyModifiers |= 2;
            if (colorSelectorAltModifierCheckBox.Checked) settings.ColorSelectorHotkeyModifiers |= 1;
            if (colorSelectorShiftModifierCheckBox.Checked) settings.ColorSelectorHotkeyModifiers |= 4;

            // General Tab
            settings.TopMost = topMostCheckBox.Checked;
            settings.PopupDuration = (int)durationNumericUpDown.Value;
            settings.ShowPopupOnPick = showPopupOnPickCheckBox.Checked;
            settings.AutoCopyEnabled = autoCopyEnabledCheckBox.Checked;
            settings.AutoCopyFormat = (ColorFormat)colorFormatComboBox.SelectedIndex;
            settings.ShowCopyNotification = showCopyNotificationCheckBox.Checked;
            
            // Handle startup setting - now managed by Settings class property
            settings.RunAtStartup = runAtStartupCheckBox.Checked;

            // Updates Tab
            settings.AutoCheckForUpdates = autoUpdateCheckBox.Checked;
            settings.UpdateCheckIntervalSeconds = (int)updateIntervalNumericUpDown.Value;
        }

        private void autoCopyEnabledCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAutoCopyUIState();
        }

        private async void checkForUpdatesButton_Click(object sender, EventArgs e)
        {
            checkForUpdatesButton.Enabled = false;
            checkForUpdatesButton.Text = "Checking...";

            try
            {
                var result = await updateManager.CheckForUpdatesAsync(showNoUpdateMessage: true);
                // This will show the update dialog if an update is available.
                updateManager.ShowUpdateDialog(result); 
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
            UpdateAutoUpdateUIState();
        }

        private void resetToDefaultsButton_Click(object sender, EventArgs e)
        {
            var (_, currentVersion, _) = settings.GetAppInfoFromRegistry();
            
            DialogResult result = MessageBox.Show(
                $"Are you sure you want to reset all settings to their default values?\n\n" +
                $"This will also reset the version in the registry to 1.0 (current: {currentVersion}) " +
                $"and disable startup functionality.",
                "Reset Settings Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                settings.ResetToDefaults();
                LoadSettingsToUI();
                
                MessageBox.Show("Settings have been reset to their default values.", 
                    "Settings Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                SaveSettingsFromUI();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}", 
                    "Settings Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void lastUpdateCheckLabel_Click(object sender, EventArgs e)
        {
            string versionInfo = updateManager.GetVersionInfo();
            MessageBox.Show(versionInfo, "Version Information", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}