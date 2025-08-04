using System;
using System.Drawing;
using System.Windows.Forms;

namespace simple_picker
{
    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;

        // Main layout controls
        private TabControl tabControl;
        private TabPage hotkeysTabPage;
        private TabPage generalTabPage;
        private TabPage updatesTabPage;
        private Button okButton;
        private Button cancelButton;
        private Button resetToDefaultsButton;

        // Hotkeys Tab controls
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

        // General Tab controls
        private GroupBox popupGroupBox;
        private CheckBox topMostCheckBox;
        private Label durationLabel;
        private NumericUpDown durationNumericUpDown;
        private CheckBox showPopupOnPickCheckBox;
        private GroupBox autoCopyGroupBox;
        private CheckBox autoCopyEnabledCheckBox;
        private Label colorFormatLabel;
        private ComboBox colorFormatComboBox;
        private CheckBox showCopyNotificationCheckBox;
        private GroupBox generalGroupBox;
        private CheckBox runAtStartupCheckBox;

        // Updates Tab controls
        private GroupBox updateGroupBox;
        private CheckBox autoUpdateCheckBox;
        private Label updateIntervalLabel;
        private NumericUpDown updateIntervalNumericUpDown;
        private Button checkForUpdatesButton;
        private Label lastUpdateCheckLabel;

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
            this.components = new System.ComponentModel.Container();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.hotkeysTabPage = new System.Windows.Forms.TabPage();
            this.generalTabPage = new System.Windows.Forms.TabPage();
            this.updatesTabPage = new System.Windows.Forms.TabPage();
            
            // Initialize group boxes and controls for each tab
            this.hotkeyGroupBox = new System.Windows.Forms.GroupBox();
            this.hotkeyComboBox = new System.Windows.Forms.ComboBox();
            this.controlModifierCheckBox = new System.Windows.Forms.CheckBox();
            this.altModifierCheckBox = new System.Windows.Forms.CheckBox();
            this.shiftModifierCheckBox = new System.Windows.Forms.CheckBox();
            
            this.colorSelectorHotkeyGroupBox = new System.Windows.Forms.GroupBox();
            this.colorSelectorHotkeyComboBox = new System.Windows.Forms.ComboBox();
            this.colorSelectorControlModifierCheckBox = new System.Windows.Forms.CheckBox();
            this.colorSelectorAltModifierCheckBox = new System.Windows.Forms.CheckBox();
            this.colorSelectorShiftModifierCheckBox = new System.Windows.Forms.CheckBox();

            this.popupGroupBox = new System.Windows.Forms.GroupBox();
            this.topMostCheckBox = new System.Windows.Forms.CheckBox();
            this.durationLabel = new System.Windows.Forms.Label();
            this.durationNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.showPopupOnPickCheckBox = new System.Windows.Forms.CheckBox();

            this.autoCopyGroupBox = new System.Windows.Forms.GroupBox();
            this.autoCopyEnabledCheckBox = new System.Windows.Forms.CheckBox();
            this.colorFormatLabel = new System.Windows.Forms.Label();
            this.colorFormatComboBox = new System.Windows.Forms.ComboBox();
            this.showCopyNotificationCheckBox = new System.Windows.Forms.CheckBox();

            this.generalGroupBox = new System.Windows.Forms.GroupBox();
            this.runAtStartupCheckBox = new System.Windows.Forms.CheckBox();

            this.updateGroupBox = new System.Windows.Forms.GroupBox();
            this.autoUpdateCheckBox = new System.Windows.Forms.CheckBox();
            this.updateIntervalLabel = new System.Windows.Forms.Label();
            this.updateIntervalNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.checkForUpdatesButton = new System.Windows.Forms.Button();
            this.lastUpdateCheckLabel = new System.Windows.Forms.Label();

            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.resetToDefaultsButton = new System.Windows.Forms.Button();

            // Suspend layout for performance
            this.tabControl.SuspendLayout();
            this.hotkeysTabPage.SuspendLayout();
            this.generalTabPage.SuspendLayout();
            this.updatesTabPage.SuspendLayout();
            this.hotkeyGroupBox.SuspendLayout();
            this.colorSelectorHotkeyGroupBox.SuspendLayout();
            this.popupGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.durationNumericUpDown)).BeginInit();
            this.autoCopyGroupBox.SuspendLayout();
            this.generalGroupBox.SuspendLayout();
            this.updateGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updateIntervalNumericUpDown)).BeginInit();
            this.SuspendLayout();

            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.hotkeysTabPage);
            this.tabControl.Controls.Add(this.generalTabPage);
            this.tabControl.Controls.Add(this.updatesTabPage);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(415, 410);
            this.tabControl.TabIndex = 0;

            // 
            // hotkeysTabPage
            // 
            this.hotkeysTabPage.Controls.Add(this.hotkeyGroupBox);
            this.hotkeysTabPage.Controls.Add(this.colorSelectorHotkeyGroupBox);
            this.hotkeysTabPage.Location = new System.Drawing.Point(4, 29);
            this.hotkeysTabPage.Name = "hotkeysTabPage";
            this.hotkeysTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.hotkeysTabPage.Size = new System.Drawing.Size(407, 377);
            this.hotkeysTabPage.TabIndex = 0;
            this.hotkeysTabPage.Text = "Hotkeys";
            this.hotkeysTabPage.UseVisualStyleBackColor = true;

            // 
            // generalTabPage
            // 
            this.generalTabPage.Controls.Add(this.popupGroupBox);
            this.generalTabPage.Controls.Add(this.autoCopyGroupBox);
            this.generalTabPage.Controls.Add(this.generalGroupBox);
            this.generalTabPage.Location = new System.Drawing.Point(4, 29);
            this.generalTabPage.Name = "generalTabPage";
            this.generalTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.generalTabPage.Size = new System.Drawing.Size(407, 377);
            this.generalTabPage.TabIndex = 1;
            this.generalTabPage.Text = "General";
            this.generalTabPage.UseVisualStyleBackColor = true;

            // 
            // updatesTabPage
            // 
            this.updatesTabPage.Controls.Add(this.updateGroupBox);
            this.updatesTabPage.Location = new System.Drawing.Point(4, 29);
            this.updatesTabPage.Name = "updatesTabPage";
            this.updatesTabPage.Size = new System.Drawing.Size(407, 377);
            this.updatesTabPage.TabIndex = 2;
            this.updatesTabPage.Text = "Updates";
            this.updatesTabPage.UseVisualStyleBackColor = true;

            //
            // hotkeyGroupBox
            //
            this.hotkeyGroupBox.Controls.Add(this.shiftModifierCheckBox);
            this.hotkeyGroupBox.Controls.Add(this.altModifierCheckBox);
            this.hotkeyGroupBox.Controls.Add(this.controlModifierCheckBox);
            this.hotkeyGroupBox.Controls.Add(this.hotkeyComboBox);
            this.hotkeyGroupBox.Location = new System.Drawing.Point(6, 6);
            this.hotkeyGroupBox.Name = "hotkeyGroupBox";
            this.hotkeyGroupBox.Size = new System.Drawing.Size(395, 125);
            this.hotkeyGroupBox.TabIndex = 0;
            this.hotkeyGroupBox.TabStop = false;
            this.hotkeyGroupBox.Text = "Color Picker Hotkey";
            
            this.hotkeyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.hotkeyComboBox.FormattingEnabled = true;
            this.hotkeyComboBox.Items.AddRange(new object[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12" });
            this.hotkeyComboBox.Location = new System.Drawing.Point(15, 30);
            this.hotkeyComboBox.Name = "hotkeyComboBox";
            this.hotkeyComboBox.Size = new System.Drawing.Size(121, 28);
            this.hotkeyComboBox.TabIndex = 0;
            
            this.controlModifierCheckBox.AutoSize = true;
            this.controlModifierCheckBox.Location = new System.Drawing.Point(15, 75);
            this.controlModifierCheckBox.Name = "controlModifierCheckBox";
            this.controlModifierCheckBox.Size = new System.Drawing.Size(78, 24);
            this.controlModifierCheckBox.TabIndex = 1;
            this.controlModifierCheckBox.Text = "Control";
            this.controlModifierCheckBox.UseVisualStyleBackColor = true;

            this.altModifierCheckBox.AutoSize = true;
            this.altModifierCheckBox.Location = new System.Drawing.Point(99, 75);
            this.altModifierCheckBox.Name = "altModifierCheckBox";
            this.altModifierCheckBox.Size = new System.Drawing.Size(50, 24);
            this.altModifierCheckBox.TabIndex = 2;
            this.altModifierCheckBox.Text = "Alt";
            this.altModifierCheckBox.UseVisualStyleBackColor = true;

            this.shiftModifierCheckBox.AutoSize = true;
            this.shiftModifierCheckBox.Location = new System.Drawing.Point(155, 75);
            this.shiftModifierCheckBox.Name = "shiftModifierCheckBox";
            this.shiftModifierCheckBox.Size = new System.Drawing.Size(61, 24);
            this.shiftModifierCheckBox.TabIndex = 3;
            this.shiftModifierCheckBox.Text = "Shift";
            this.shiftModifierCheckBox.UseVisualStyleBackColor = true;

            //
            // colorSelectorHotkeyGroupBox
            //
            this.colorSelectorHotkeyGroupBox.Controls.Add(this.colorSelectorShiftModifierCheckBox);
            this.colorSelectorHotkeyGroupBox.Controls.Add(this.colorSelectorAltModifierCheckBox);
            this.colorSelectorHotkeyGroupBox.Controls.Add(this.colorSelectorControlModifierCheckBox);
            this.colorSelectorHotkeyGroupBox.Controls.Add(this.colorSelectorHotkeyComboBox);
            this.colorSelectorHotkeyGroupBox.Location = new System.Drawing.Point(6, 137);
            this.colorSelectorHotkeyGroupBox.Name = "colorSelectorHotkeyGroupBox";
            this.colorSelectorHotkeyGroupBox.Size = new System.Drawing.Size(395, 125);
            this.colorSelectorHotkeyGroupBox.TabIndex = 1;
            this.colorSelectorHotkeyGroupBox.TabStop = false;
            this.colorSelectorHotkeyGroupBox.Text = "Color Selector Hotkey";

            this.colorSelectorHotkeyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.colorSelectorHotkeyComboBox.FormattingEnabled = true;
            this.colorSelectorHotkeyComboBox.Items.AddRange(new object[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12" });
            this.colorSelectorHotkeyComboBox.Location = new System.Drawing.Point(15, 30);
            this.colorSelectorHotkeyComboBox.Name = "colorSelectorHotkeyComboBox";
            this.colorSelectorHotkeyComboBox.Size = new System.Drawing.Size(121, 28);
            this.colorSelectorHotkeyComboBox.TabIndex = 0;

            this.colorSelectorControlModifierCheckBox.AutoSize = true;
            this.colorSelectorControlModifierCheckBox.Location = new System.Drawing.Point(15, 75);
            this.colorSelectorControlModifierCheckBox.Name = "colorSelectorControlModifierCheckBox";
            this.colorSelectorControlModifierCheckBox.Size = new System.Drawing.Size(78, 24);
            this.colorSelectorControlModifierCheckBox.TabIndex = 1;
            this.colorSelectorControlModifierCheckBox.Text = "Control";
            this.colorSelectorControlModifierCheckBox.UseVisualStyleBackColor = true;

            this.colorSelectorAltModifierCheckBox.AutoSize = true;
            this.colorSelectorAltModifierCheckBox.Location = new System.Drawing.Point(99, 75);
            this.colorSelectorAltModifierCheckBox.Name = "colorSelectorAltModifierCheckBox";
            this.colorSelectorAltModifierCheckBox.Size = new System.Drawing.Size(50, 24);
            this.colorSelectorAltModifierCheckBox.TabIndex = 2;
            this.colorSelectorAltModifierCheckBox.Text = "Alt";
            this.colorSelectorAltModifierCheckBox.UseVisualStyleBackColor = true;

            this.colorSelectorShiftModifierCheckBox.AutoSize = true;
            this.colorSelectorShiftModifierCheckBox.Location = new System.Drawing.Point(155, 75);
            this.colorSelectorShiftModifierCheckBox.Name = "colorSelectorShiftModifierCheckBox";
            this.colorSelectorShiftModifierCheckBox.Size = new System.Drawing.Size(61, 24);
            this.colorSelectorShiftModifierCheckBox.TabIndex = 3;
            this.colorSelectorShiftModifierCheckBox.Text = "Shift";
            this.colorSelectorShiftModifierCheckBox.UseVisualStyleBackColor = true;

            // 
            // popupGroupBox
            // 
            this.popupGroupBox.Controls.Add(this.showPopupOnPickCheckBox);
            this.popupGroupBox.Controls.Add(this.durationNumericUpDown);
            this.popupGroupBox.Controls.Add(this.durationLabel);
            this.popupGroupBox.Controls.Add(this.topMostCheckBox);
            this.popupGroupBox.Location = new System.Drawing.Point(6, 6);
            this.popupGroupBox.Name = "popupGroupBox";
            this.popupGroupBox.Size = new System.Drawing.Size(395, 130);
            this.popupGroupBox.TabIndex = 0;
            this.popupGroupBox.TabStop = false;
            this.popupGroupBox.Text = "Popup Settings";

            this.topMostCheckBox.AutoSize = true;
            this.topMostCheckBox.Location = new System.Drawing.Point(15, 30);
            this.topMostCheckBox.Name = "topMostCheckBox";
            this.topMostCheckBox.Size = new System.Drawing.Size(123, 24);
            this.topMostCheckBox.TabIndex = 0;
            this.topMostCheckBox.Text = "Always on top";
            this.topMostCheckBox.UseVisualStyleBackColor = true;

            this.showPopupOnPickCheckBox.AutoSize = true;
            this.showPopupOnPickCheckBox.Location = new System.Drawing.Point(15, 60);
            this.showPopupOnPickCheckBox.Name = "showPopupOnPickCheckBox";
            this.showPopupOnPickCheckBox.Size = new System.Drawing.Size(210, 24);
            this.showPopupOnPickCheckBox.TabIndex = 1;
            this.showPopupOnPickCheckBox.Text = "Show popup after picking";
            this.showPopupOnPickCheckBox.UseVisualStyleBackColor = true;

            this.durationLabel.AutoSize = true;
            this.durationLabel.Location = new System.Drawing.Point(15, 93);
            this.durationLabel.Name = "durationLabel";
            this.durationLabel.Size = new System.Drawing.Size(150, 20);
            this.durationLabel.TabIndex = 2;
            this.durationLabel.Text = "Auto-close after (ms):";

            this.durationNumericUpDown.Location = new System.Drawing.Point(171, 91);
            this.durationNumericUpDown.Maximum = new decimal(new int[] { 30000, 0, 0, 0 });
            this.durationNumericUpDown.Name = "durationNumericUpDown";
            this.durationNumericUpDown.Size = new System.Drawing.Size(80, 27);
            this.durationNumericUpDown.TabIndex = 3;
            this.durationNumericUpDown.Value = new decimal(new int[] { 5000, 0, 0, 0 });
            
            // 
            // autoCopyGroupBox
            // 
            this.autoCopyGroupBox.Controls.Add(this.showCopyNotificationCheckBox);
            this.autoCopyGroupBox.Controls.Add(this.colorFormatComboBox);
            this.autoCopyGroupBox.Controls.Add(this.colorFormatLabel);
            this.autoCopyGroupBox.Controls.Add(this.autoCopyEnabledCheckBox);
            this.autoCopyGroupBox.Location = new System.Drawing.Point(6, 142);
            this.autoCopyGroupBox.Name = "autoCopyGroupBox";
            this.autoCopyGroupBox.Size = new System.Drawing.Size(395, 130);
            this.autoCopyGroupBox.TabIndex = 1;
            this.autoCopyGroupBox.TabStop = false;
            this.autoCopyGroupBox.Text = "Auto-Copy Settings";

            this.autoCopyEnabledCheckBox.AutoSize = true;
            this.autoCopyEnabledCheckBox.Location = new System.Drawing.Point(15, 30);
            this.autoCopyEnabledCheckBox.Name = "autoCopyEnabledCheckBox";
            this.autoCopyEnabledCheckBox.Size = new System.Drawing.Size(248, 24);
            this.autoCopyEnabledCheckBox.TabIndex = 0;
            this.autoCopyEnabledCheckBox.Text = "Automatically copy to clipboard";
            this.autoCopyEnabledCheckBox.UseVisualStyleBackColor = true;
            this.autoCopyEnabledCheckBox.CheckedChanged += new System.EventHandler(this.autoCopyEnabledCheckBox_CheckedChanged);
            
            this.colorFormatLabel.AutoSize = true;
            this.colorFormatLabel.Location = new System.Drawing.Point(15, 63);
            this.colorFormatLabel.Name = "colorFormatLabel";
            this.colorFormatLabel.Size = new System.Drawing.Size(95, 20);
            this.colorFormatLabel.TabIndex = 1;
            this.colorFormatLabel.Text = "Color format:";

            this.colorFormatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.colorFormatComboBox.FormattingEnabled = true;
            this.colorFormatComboBox.Location = new System.Drawing.Point(116, 60);
            this.colorFormatComboBox.Name = "colorFormatComboBox";
            this.colorFormatComboBox.Size = new System.Drawing.Size(150, 28);
            this.colorFormatComboBox.TabIndex = 2;
            
            this.showCopyNotificationCheckBox.AutoSize = true;
            this.showCopyNotificationCheckBox.Location = new System.Drawing.Point(15, 94);
            this.showCopyNotificationCheckBox.Name = "showCopyNotificationCheckBox";
            this.showCopyNotificationCheckBox.Size = new System.Drawing.Size(174, 24);
            this.showCopyNotificationCheckBox.TabIndex = 3;
            this.showCopyNotificationCheckBox.Text = "Show copy notification";
            this.showCopyNotificationCheckBox.UseVisualStyleBackColor = true;

            // 
            // generalGroupBox
            // 
            this.generalGroupBox.Controls.Add(this.runAtStartupCheckBox);
            this.generalGroupBox.Location = new System.Drawing.Point(6, 278);
            this.generalGroupBox.Name = "generalGroupBox";
            this.generalGroupBox.Size = new System.Drawing.Size(395, 70);
            this.generalGroupBox.TabIndex = 2;
            this.generalGroupBox.TabStop = false;
            this.generalGroupBox.Text = "Application";

            this.runAtStartupCheckBox.AutoSize = true;
            this.runAtStartupCheckBox.Location = new System.Drawing.Point(15, 30);
            this.runAtStartupCheckBox.Name = "runAtStartupCheckBox";
            this.runAtStartupCheckBox.Size = new System.Drawing.Size(127, 24);
            this.runAtStartupCheckBox.TabIndex = 0;
            this.runAtStartupCheckBox.Text = "Run at startup";
            this.runAtStartupCheckBox.UseVisualStyleBackColor = true;

            // 
            // updateGroupBox
            // 
            this.updateGroupBox.Controls.Add(this.lastUpdateCheckLabel);
            this.updateGroupBox.Controls.Add(this.checkForUpdatesButton);
            this.updateGroupBox.Controls.Add(this.updateIntervalNumericUpDown);
            this.updateGroupBox.Controls.Add(this.updateIntervalLabel);
            this.updateGroupBox.Controls.Add(this.autoUpdateCheckBox);
            this.updateGroupBox.Location = new System.Drawing.Point(6, 6);
            this.updateGroupBox.Name = "updateGroupBox";
            this.updateGroupBox.Size = new System.Drawing.Size(395, 150);
            this.updateGroupBox.TabIndex = 0;
            this.updateGroupBox.TabStop = false;
            this.updateGroupBox.Text = "Update Settings";

            this.autoUpdateCheckBox.AutoSize = true;
            this.autoUpdateCheckBox.Location = new System.Drawing.Point(15, 30);
            this.autoUpdateCheckBox.Name = "autoUpdateCheckBox";
            this.autoUpdateCheckBox.Size = new System.Drawing.Size(242, 24);
            this.autoUpdateCheckBox.TabIndex = 0;
            this.autoUpdateCheckBox.Text = "Automatically check for updates";
            this.autoUpdateCheckBox.UseVisualStyleBackColor = true;
            this.autoUpdateCheckBox.CheckedChanged += new System.EventHandler(this.autoUpdateCheckBox_CheckedChanged);
            
            this.updateIntervalLabel.AutoSize = true;
            this.updateIntervalLabel.Location = new System.Drawing.Point(15, 63);
            this.updateIntervalLabel.Name = "updateIntervalLabel";
            this.updateIntervalLabel.Size = new System.Drawing.Size(169, 20);
            this.updateIntervalLabel.TabIndex = 1;
            this.updateIntervalLabel.Text = "Check interval (seconds):";

            this.updateIntervalNumericUpDown.Location = new System.Drawing.Point(190, 61);
            this.updateIntervalNumericUpDown.Maximum = new decimal(new int[] { 86400, 0, 0, 0 }); // 1 day
            this.updateIntervalNumericUpDown.Minimum = new decimal(new int[] { 30, 0, 0, 0 });
            this.updateIntervalNumericUpDown.Name = "updateIntervalNumericUpDown";
            this.updateIntervalNumericUpDown.Size = new System.Drawing.Size(70, 27);
            this.updateIntervalNumericUpDown.TabIndex = 2;
            this.updateIntervalNumericUpDown.Value = new decimal(new int[] { 3600, 0, 0, 0 });

            this.checkForUpdatesButton.Location = new System.Drawing.Point(15, 100);
            this.checkForUpdatesButton.Name = "checkForUpdatesButton";
            this.checkForUpdatesButton.Size = new System.Drawing.Size(150, 33);
            this.checkForUpdatesButton.TabIndex = 3;
            this.checkForUpdatesButton.Text = "Check for Updates";
            this.checkForUpdatesButton.UseVisualStyleBackColor = true;
            this.checkForUpdatesButton.Click += new System.EventHandler(this.checkForUpdatesButton_Click);

            this.lastUpdateCheckLabel.AutoSize = true;
            this.lastUpdateCheckLabel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lastUpdateCheckLabel.Location = new System.Drawing.Point(171, 106);
            this.lastUpdateCheckLabel.Name = "lastUpdateCheckLabel";
            this.lastUpdateCheckLabel.Size = new System.Drawing.Size(120, 20);
            this.lastUpdateCheckLabel.TabIndex = 4;
            this.lastUpdateCheckLabel.Text = "Last check: Never";
            this.lastUpdateCheckLabel.Click += new System.EventHandler(this.lastUpdateCheckLabel_Click);

            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(231, 435);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(94, 29);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);

            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(331, 435);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(94, 29);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);

            // 
            // resetToDefaultsButton
            // 
            this.resetToDefaultsButton.Location = new System.Drawing.Point(12, 435);
            this.resetToDefaultsButton.Name = "resetToDefaultsButton";
            this.resetToDefaultsButton.Size = new System.Drawing.Size(140, 29);
            this.resetToDefaultsButton.TabIndex = 3;
            this.resetToDefaultsButton.Text = "Reset to Defaults";
            this.resetToDefaultsButton.UseVisualStyleBackColor = true;
            this.resetToDefaultsButton.Click += new System.EventHandler(this.resetToDefaultsButton_Click);

            // 
            // SettingsForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(439, 476);
            this.Controls.Add(this.resetToDefaultsButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SimplePicker Settings";

            // Resume layout
            this.tabControl.ResumeLayout(false);
            this.hotkeysTabPage.ResumeLayout(false);
            this.generalTabPage.ResumeLayout(false);
            this.updatesTabPage.ResumeLayout(false);
            this.hotkeyGroupBox.ResumeLayout(false);
            this.hotkeyGroupBox.PerformLayout();
            this.colorSelectorHotkeyGroupBox.ResumeLayout(false);
            this.colorSelectorHotkeyGroupBox.PerformLayout();
            this.popupGroupBox.ResumeLayout(false);
            this.popupGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.durationNumericUpDown)).EndInit();
            this.autoCopyGroupBox.ResumeLayout(false);
            this.autoCopyGroupBox.PerformLayout();
            this.generalGroupBox.ResumeLayout(false);
            this.generalGroupBox.PerformLayout();
            this.updateGroupBox.ResumeLayout(false);
            this.updateGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updateIntervalNumericUpDown)).EndInit();
            this.ResumeLayout(false);

            InitializeColorFormatComboBox();
        }

        private void InitializeColorFormatComboBox()
        {
            colorFormatComboBox.Items.Clear();
            // Using a more robust way to add items to avoid potential issues with enum changes
            colorFormatComboBox.DataSource = Enum.GetValues(typeof(ColorFormat));
            colorFormatComboBox.SelectedIndex = 0;
        }
    }
}
