using System;
using System.Windows.Forms;

namespace simple_picker
{
    public partial class SettingsForm : Form
    {
        private Settings settings;

        public SettingsForm(Settings settings)
        {
            this.settings = settings;
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
    }
}