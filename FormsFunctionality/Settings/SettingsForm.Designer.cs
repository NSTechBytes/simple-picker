using System;
using System.Drawing;
using System.Windows.Forms;

namespace simple_picker
{
    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;
        private GroupBox hotkeyGroupBox;
        private ComboBox hotkeyComboBox;
        private CheckBox controlModifierCheckBox;
        private CheckBox altModifierCheckBox;
        private CheckBox shiftModifierCheckBox;
        private GroupBox colorSelectorHotkeyGroupBox;
        private ComboBox colorSelectorHotkeyComboBox;
        private CheckBox colorSelectorControlModifierCheckBox;
        private CheckBox colorSelectorAltModifierCheckBox;
        private CheckBox colorSelectorShiftModifierCheckBox;
        private GroupBox popupGroupBox;
        private CheckBox topMostCheckBox;
        private Label durationLabel;
        private NumericUpDown durationNumericUpDown;
        private GroupBox updateGroupBox;
        private CheckBox autoUpdateCheckBox;
        private Label updateIntervalLabel;
        private NumericUpDown updateIntervalNumericUpDown;
        private Button checkForUpdatesButton;
        private Label lastUpdateCheckLabel;
        private GroupBox generalGroupBox;
        private CheckBox runAtStartupCheckBox;
        private Button resetToDefaultsButton;
        private Button okButton;
        private Button cancelButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            hotkeyGroupBox = new GroupBox();
            shiftModifierCheckBox = new CheckBox();
            altModifierCheckBox = new CheckBox();
            controlModifierCheckBox = new CheckBox();
            hotkeyComboBox = new ComboBox();
            colorSelectorHotkeyGroupBox = new GroupBox();
            colorSelectorShiftModifierCheckBox = new CheckBox();
            colorSelectorAltModifierCheckBox = new CheckBox();
            colorSelectorControlModifierCheckBox = new CheckBox();
            colorSelectorHotkeyComboBox = new ComboBox();
            popupGroupBox = new GroupBox();
            durationNumericUpDown = new NumericUpDown();
            durationLabel = new Label();
            topMostCheckBox = new CheckBox();
            updateGroupBox = new GroupBox();
            lastUpdateCheckLabel = new Label();
            checkForUpdatesButton = new Button();
            updateIntervalNumericUpDown = new NumericUpDown();
            updateIntervalLabel = new Label();
            autoUpdateCheckBox = new CheckBox();
            generalGroupBox = new GroupBox();
            runAtStartupCheckBox = new CheckBox();
            resetToDefaultsButton = new Button();
            okButton = new Button();
            cancelButton = new Button();
            hotkeyGroupBox.SuspendLayout();
            colorSelectorHotkeyGroupBox.SuspendLayout();
            popupGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)durationNumericUpDown).BeginInit();
            updateGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)updateIntervalNumericUpDown).BeginInit();
            generalGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // hotkeyGroupBox
            // 
            hotkeyGroupBox.Controls.Add(shiftModifierCheckBox);
            hotkeyGroupBox.Controls.Add(altModifierCheckBox);
            hotkeyGroupBox.Controls.Add(controlModifierCheckBox);
            hotkeyGroupBox.Controls.Add(hotkeyComboBox);
            hotkeyGroupBox.Location = new Point(14, 16);
            hotkeyGroupBox.Margin = new Padding(3, 4, 3, 4);
            hotkeyGroupBox.Name = "hotkeyGroupBox";
            hotkeyGroupBox.Padding = new Padding(3, 4, 3, 4);
            hotkeyGroupBox.Size = new Size(411, 133);
            hotkeyGroupBox.TabIndex = 0;
            hotkeyGroupBox.TabStop = false;
            hotkeyGroupBox.Text = "Color Picker Hotkey";
            // 
            // shiftModifierCheckBox
            // 
            shiftModifierCheckBox.AutoSize = true;
            shiftModifierCheckBox.Location = new Point(166, 80);
            shiftModifierCheckBox.Margin = new Padding(3, 4, 3, 4);
            shiftModifierCheckBox.Name = "shiftModifierCheckBox";
            shiftModifierCheckBox.Size = new Size(61, 24);
            shiftModifierCheckBox.TabIndex = 3;
            shiftModifierCheckBox.Text = "Shift";
            shiftModifierCheckBox.UseVisualStyleBackColor = true;
            // 
            // altModifierCheckBox
            // 
            altModifierCheckBox.AutoSize = true;
            altModifierCheckBox.Location = new Point(103, 80);
            altModifierCheckBox.Margin = new Padding(3, 4, 3, 4);
            altModifierCheckBox.Name = "altModifierCheckBox";
            altModifierCheckBox.Size = new Size(50, 24);
            altModifierCheckBox.TabIndex = 2;
            altModifierCheckBox.Text = "Alt";
            altModifierCheckBox.UseVisualStyleBackColor = true;
            // 
            // controlModifierCheckBox
            // 
            controlModifierCheckBox.AutoSize = true;
            controlModifierCheckBox.Checked = true;
            controlModifierCheckBox.CheckState = CheckState.Checked;
            controlModifierCheckBox.Location = new Point(17, 80);
            controlModifierCheckBox.Margin = new Padding(3, 4, 3, 4);
            controlModifierCheckBox.Name = "controlModifierCheckBox";
            controlModifierCheckBox.Size = new Size(80, 24);
            controlModifierCheckBox.TabIndex = 1;
            controlModifierCheckBox.Text = "Control";
            controlModifierCheckBox.UseVisualStyleBackColor = true;
            // 
            // hotkeyComboBox
            // 
            hotkeyComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            hotkeyComboBox.FormattingEnabled = true;
            hotkeyComboBox.Items.AddRange(new object[] { "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "D0" });
            hotkeyComboBox.Location = new Point(17, 33);
            hotkeyComboBox.Margin = new Padding(3, 4, 3, 4);
            hotkeyComboBox.Name = "hotkeyComboBox";
            hotkeyComboBox.Size = new Size(114, 28);
            hotkeyComboBox.TabIndex = 0;
            // 
            // colorSelectorHotkeyGroupBox
            // 
            colorSelectorHotkeyGroupBox.Controls.Add(colorSelectorShiftModifierCheckBox);
            colorSelectorHotkeyGroupBox.Controls.Add(colorSelectorAltModifierCheckBox);
            colorSelectorHotkeyGroupBox.Controls.Add(colorSelectorControlModifierCheckBox);
            colorSelectorHotkeyGroupBox.Controls.Add(colorSelectorHotkeyComboBox);
            colorSelectorHotkeyGroupBox.Location = new Point(14, 167);
            colorSelectorHotkeyGroupBox.Margin = new Padding(3, 4, 3, 4);
            colorSelectorHotkeyGroupBox.Name = "colorSelectorHotkeyGroupBox";
            colorSelectorHotkeyGroupBox.Padding = new Padding(3, 4, 3, 4);
            colorSelectorHotkeyGroupBox.Size = new Size(411, 133);
            colorSelectorHotkeyGroupBox.TabIndex = 1;
            colorSelectorHotkeyGroupBox.TabStop = false;
            colorSelectorHotkeyGroupBox.Text = "Color Selector Hotkey";
            // 
            // colorSelectorShiftModifierCheckBox
            // 
            colorSelectorShiftModifierCheckBox.AutoSize = true;
            colorSelectorShiftModifierCheckBox.Location = new Point(166, 80);
            colorSelectorShiftModifierCheckBox.Margin = new Padding(3, 4, 3, 4);
            colorSelectorShiftModifierCheckBox.Name = "colorSelectorShiftModifierCheckBox";
            colorSelectorShiftModifierCheckBox.Size = new Size(61, 24);
            colorSelectorShiftModifierCheckBox.TabIndex = 3;
            colorSelectorShiftModifierCheckBox.Text = "Shift";
            colorSelectorShiftModifierCheckBox.UseVisualStyleBackColor = true;
            // 
            // colorSelectorAltModifierCheckBox
            // 
            colorSelectorAltModifierCheckBox.AutoSize = true;
            colorSelectorAltModifierCheckBox.Location = new Point(103, 80);
            colorSelectorAltModifierCheckBox.Margin = new Padding(3, 4, 3, 4);
            colorSelectorAltModifierCheckBox.Name = "colorSelectorAltModifierCheckBox";
            colorSelectorAltModifierCheckBox.Size = new Size(50, 24);
            colorSelectorAltModifierCheckBox.TabIndex = 2;
            colorSelectorAltModifierCheckBox.Text = "Alt";
            colorSelectorAltModifierCheckBox.UseVisualStyleBackColor = true;
            // 
            // colorSelectorControlModifierCheckBox
            // 
            colorSelectorControlModifierCheckBox.AutoSize = true;
            colorSelectorControlModifierCheckBox.Checked = true;
            colorSelectorControlModifierCheckBox.CheckState = CheckState.Checked;
            colorSelectorControlModifierCheckBox.Location = new Point(17, 80);
            colorSelectorControlModifierCheckBox.Margin = new Padding(3, 4, 3, 4);
            colorSelectorControlModifierCheckBox.Name = "colorSelectorControlModifierCheckBox";
            colorSelectorControlModifierCheckBox.Size = new Size(80, 24);
            colorSelectorControlModifierCheckBox.TabIndex = 1;
            colorSelectorControlModifierCheckBox.Text = "Control";
            colorSelectorControlModifierCheckBox.UseVisualStyleBackColor = true;
            // 
            // colorSelectorHotkeyComboBox
            // 
            colorSelectorHotkeyComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            colorSelectorHotkeyComboBox.FormattingEnabled = true;
            colorSelectorHotkeyComboBox.Items.AddRange(new object[] { "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "D0" });
            colorSelectorHotkeyComboBox.Location = new Point(17, 33);
            colorSelectorHotkeyComboBox.Margin = new Padding(3, 4, 3, 4);
            colorSelectorHotkeyComboBox.Name = "colorSelectorHotkeyComboBox";
            colorSelectorHotkeyComboBox.Size = new Size(114, 28);
            colorSelectorHotkeyComboBox.TabIndex = 0;
            // 
            // popupGroupBox
            // 
            popupGroupBox.Controls.Add(durationNumericUpDown);
            popupGroupBox.Controls.Add(durationLabel);
            popupGroupBox.Controls.Add(topMostCheckBox);
            popupGroupBox.Location = new Point(14, 320);
            popupGroupBox.Margin = new Padding(3, 4, 3, 4);
            popupGroupBox.Name = "popupGroupBox";
            popupGroupBox.Padding = new Padding(3, 4, 3, 4);
            popupGroupBox.Size = new Size(411, 107);
            popupGroupBox.TabIndex = 2;
            popupGroupBox.TabStop = false;
            popupGroupBox.Text = "Popup Settings";
            // 
            // durationNumericUpDown
            // 
            durationNumericUpDown.Location = new Point(177, 64);
            durationNumericUpDown.Margin = new Padding(3, 4, 3, 4);
            durationNumericUpDown.Maximum = new decimal(new int[] { 30000, 0, 0, 0 });
            durationNumericUpDown.Name = "durationNumericUpDown";
            durationNumericUpDown.Size = new Size(91, 27);
            durationNumericUpDown.TabIndex = 2;
            durationNumericUpDown.Value = new decimal(new int[] { 5000, 0, 0, 0 });
            // 
            // durationLabel
            // 
            durationLabel.AutoSize = true;
            durationLabel.Location = new Point(17, 67);
            durationLabel.Name = "durationLabel";
            durationLabel.Size = new Size(152, 20);
            durationLabel.TabIndex = 1;
            durationLabel.Text = "Auto-close after (ms):";
            // 
            // topMostCheckBox
            // 
            topMostCheckBox.AutoSize = true;
            topMostCheckBox.Checked = true;
            topMostCheckBox.CheckState = CheckState.Checked;
            topMostCheckBox.Location = new Point(17, 33);
            topMostCheckBox.Margin = new Padding(3, 4, 3, 4);
            topMostCheckBox.Name = "topMostCheckBox";
            topMostCheckBox.Size = new Size(125, 24);
            topMostCheckBox.TabIndex = 0;
            topMostCheckBox.Text = "Always on top";
            topMostCheckBox.UseVisualStyleBackColor = true;
            // 
            // updateGroupBox
            // 
            updateGroupBox.Controls.Add(lastUpdateCheckLabel);
            updateGroupBox.Controls.Add(checkForUpdatesButton);
            updateGroupBox.Controls.Add(updateIntervalNumericUpDown);
            updateGroupBox.Controls.Add(updateIntervalLabel);
            updateGroupBox.Controls.Add(autoUpdateCheckBox);
            updateGroupBox.Location = new Point(14, 447);
            updateGroupBox.Margin = new Padding(3, 4, 3, 4);
            updateGroupBox.Name = "updateGroupBox";
            updateGroupBox.Padding = new Padding(3, 4, 3, 4);
            updateGroupBox.Size = new Size(411, 160);
            updateGroupBox.TabIndex = 3;
            updateGroupBox.TabStop = false;
            updateGroupBox.Text = "Update Settings";
            // 
            // lastUpdateCheckLabel
            // 
            lastUpdateCheckLabel.AutoSize = true;
            lastUpdateCheckLabel.ForeColor = SystemColors.GrayText;
            lastUpdateCheckLabel.Location = new Point(177, 106);
            lastUpdateCheckLabel.Name = "lastUpdateCheckLabel";
            lastUpdateCheckLabel.Size = new Size(122, 20);
            lastUpdateCheckLabel.TabIndex = 4;
            lastUpdateCheckLabel.Text = "Last check: Never";
            lastUpdateCheckLabel.Click += lastUpdateCheckLabel_Click;
            // 
            // checkForUpdatesButton
            // 
            checkForUpdatesButton.Location = new Point(17, 100);
            checkForUpdatesButton.Margin = new Padding(3, 4, 3, 4);
            checkForUpdatesButton.Name = "checkForUpdatesButton";
            checkForUpdatesButton.Size = new Size(142, 33);
            checkForUpdatesButton.TabIndex = 3;
            checkForUpdatesButton.Text = "Check for Updates";
            checkForUpdatesButton.UseVisualStyleBackColor = true;
            checkForUpdatesButton.Click += checkForUpdatesButton_Click;
            // 
            // updateIntervalNumericUpDown
            // 
            updateIntervalNumericUpDown.Location = new Point(199, 65);
            updateIntervalNumericUpDown.Margin = new Padding(3, 4, 3, 4);
            updateIntervalNumericUpDown.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
            updateIntervalNumericUpDown.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            updateIntervalNumericUpDown.Name = "updateIntervalNumericUpDown";
            updateIntervalNumericUpDown.Size = new Size(69, 27);
            updateIntervalNumericUpDown.TabIndex = 2;
            updateIntervalNumericUpDown.Value = new decimal(new int[] { 30, 0, 0, 0 });
            // 
            // updateIntervalLabel
            // 
            updateIntervalLabel.AutoSize = true;
            updateIntervalLabel.Location = new Point(17, 67);
            updateIntervalLabel.Name = "updateIntervalLabel";
            updateIntervalLabel.Size = new Size(171, 20);
            updateIntervalLabel.TabIndex = 1;
            updateIntervalLabel.Text = "Check interval (seconds):";
            // 
            // autoUpdateCheckBox
            // 
            autoUpdateCheckBox.AutoSize = true;
            autoUpdateCheckBox.Checked = true;
            autoUpdateCheckBox.CheckState = CheckState.Checked;
            autoUpdateCheckBox.Location = new Point(17, 33);
            autoUpdateCheckBox.Margin = new Padding(3, 4, 3, 4);
            autoUpdateCheckBox.Name = "autoUpdateCheckBox";
            autoUpdateCheckBox.Size = new Size(244, 24);
            autoUpdateCheckBox.TabIndex = 0;
            autoUpdateCheckBox.Text = "Automatically check for updates";
            autoUpdateCheckBox.UseVisualStyleBackColor = true;
            autoUpdateCheckBox.CheckedChanged += autoUpdateCheckBox_CheckedChanged;
            // 
            // generalGroupBox
            // 
            generalGroupBox.Controls.Add(runAtStartupCheckBox);
            generalGroupBox.Location = new Point(14, 627);
            generalGroupBox.Margin = new Padding(3, 4, 3, 4);
            generalGroupBox.Name = "generalGroupBox";
            generalGroupBox.Padding = new Padding(3, 4, 3, 4);
            generalGroupBox.Size = new Size(411, 80);
            generalGroupBox.TabIndex = 4;
            generalGroupBox.TabStop = false;
            generalGroupBox.Text = "General Settings";
            // 
            // runAtStartupCheckBox
            // 
            runAtStartupCheckBox.AutoSize = true;
            runAtStartupCheckBox.Location = new Point(17, 33);
            runAtStartupCheckBox.Margin = new Padding(3, 4, 3, 4);
            runAtStartupCheckBox.Name = "runAtStartupCheckBox";
            runAtStartupCheckBox.Size = new Size(212, 24);
            runAtStartupCheckBox.TabIndex = 0;
            runAtStartupCheckBox.Text = "Run SimplePicker at startup";
            runAtStartupCheckBox.UseVisualStyleBackColor = true;
            // 
            // resetToDefaultsButton
            // 
            resetToDefaultsButton.Location = new Point(14, 727);
            resetToDefaultsButton.Margin = new Padding(3, 4, 3, 4);
            resetToDefaultsButton.Name = "resetToDefaultsButton";
            resetToDefaultsButton.Size = new Size(142, 33);
            resetToDefaultsButton.TabIndex = 5;
            resetToDefaultsButton.Text = "Reset to Defaults";
            resetToDefaultsButton.UseVisualStyleBackColor = true;
            resetToDefaultsButton.Click += resetToDefaultsButton_Click;
            // 
            // okButton
            // 
            okButton.Location = new Point(247, 727);
            okButton.Margin = new Padding(3, 4, 3, 4);
            okButton.Name = "okButton";
            okButton.Size = new Size(86, 33);
            okButton.TabIndex = 6;
            okButton.Text = "OK";
            okButton.UseVisualStyleBackColor = true;
            okButton.Click += okButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.Location = new Point(339, 727);
            cancelButton.Margin = new Padding(3, 4, 3, 4);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(86, 33);
            cancelButton.TabIndex = 7;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(439, 776);
            Controls.Add(cancelButton);
            Controls.Add(okButton);
            Controls.Add(resetToDefaultsButton);
            Controls.Add(generalGroupBox);
            Controls.Add(updateGroupBox);
            Controls.Add(popupGroupBox);
            Controls.Add(colorSelectorHotkeyGroupBox);
            Controls.Add(hotkeyGroupBox);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "SimplePicker Settings";
            hotkeyGroupBox.ResumeLayout(false);
            hotkeyGroupBox.PerformLayout();
            colorSelectorHotkeyGroupBox.ResumeLayout(false);
            colorSelectorHotkeyGroupBox.PerformLayout();
            popupGroupBox.ResumeLayout(false);
            popupGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)durationNumericUpDown).EndInit();
            updateGroupBox.ResumeLayout(false);
            updateGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)updateIntervalNumericUpDown).EndInit();
            generalGroupBox.ResumeLayout(false);
            generalGroupBox.PerformLayout();
            ResumeLayout(false);
        }
    }
}