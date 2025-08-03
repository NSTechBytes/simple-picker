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
        private GroupBox popupGroupBox;
        private CheckBox topMostCheckBox;
        private Label durationLabel;
        private NumericUpDown durationNumericUpDown;
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
            this.hotkeyGroupBox = new GroupBox();
            this.hotkeyComboBox = new ComboBox();
            this.controlModifierCheckBox = new CheckBox();
            this.altModifierCheckBox = new CheckBox();
            this.shiftModifierCheckBox = new CheckBox();
            this.popupGroupBox = new GroupBox();
            this.topMostCheckBox = new CheckBox();
            this.durationLabel = new Label();
            this.durationNumericUpDown = new NumericUpDown();
            this.okButton = new Button();
            this.cancelButton = new Button();
            this.hotkeyGroupBox.SuspendLayout();
            this.popupGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.durationNumericUpDown)).BeginInit();
            this.SuspendLayout();

            // hotkeyGroupBox
            this.hotkeyGroupBox.Controls.Add(this.shiftModifierCheckBox);
            this.hotkeyGroupBox.Controls.Add(this.altModifierCheckBox);
            this.hotkeyGroupBox.Controls.Add(this.controlModifierCheckBox);
            this.hotkeyGroupBox.Controls.Add(this.hotkeyComboBox);
            this.hotkeyGroupBox.Location = new Point(12, 12);
            this.hotkeyGroupBox.Name = "hotkeyGroupBox";
            this.hotkeyGroupBox.Size = new Size(360, 100);
            this.hotkeyGroupBox.TabIndex = 0;
            this.hotkeyGroupBox.TabStop = false;
            this.hotkeyGroupBox.Text = "Global Hotkey";

            // hotkeyComboBox
            this.hotkeyComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.hotkeyComboBox.FormattingEnabled = true;
            this.hotkeyComboBox.Items.AddRange(new object[] {
                "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12",
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "D0"
            });
            this.hotkeyComboBox.Location = new Point(15, 25);
            this.hotkeyComboBox.Name = "hotkeyComboBox";
            this.hotkeyComboBox.Size = new Size(100, 23);
            this.hotkeyComboBox.TabIndex = 0;

            // controlModifierCheckBox
            this.controlModifierCheckBox.AutoSize = true;
            this.controlModifierCheckBox.Checked = true;
            this.controlModifierCheckBox.Location = new Point(15, 60);
            this.controlModifierCheckBox.Name = "controlModifierCheckBox";
            this.controlModifierCheckBox.Size = new Size(63, 19);
            this.controlModifierCheckBox.TabIndex = 1;
            this.controlModifierCheckBox.Text = "Control";
            this.controlModifierCheckBox.UseVisualStyleBackColor = true;

            // altModifierCheckBox
            this.altModifierCheckBox.AutoSize = true;
            this.altModifierCheckBox.Location = new Point(90, 60);
            this.altModifierCheckBox.Name = "altModifierCheckBox";
            this.altModifierCheckBox.Size = new Size(40, 19);
            this.altModifierCheckBox.TabIndex = 2;
            this.altModifierCheckBox.Text = "Alt";
            this.altModifierCheckBox.UseVisualStyleBackColor = true;

            // shiftModifierCheckBox
            this.shiftModifierCheckBox.AutoSize = true;
            this.shiftModifierCheckBox.Location = new Point(145, 60);
            this.shiftModifierCheckBox.Name = "shiftModifierCheckBox";
            this.shiftModifierCheckBox.Size = new Size(50, 19);
            this.shiftModifierCheckBox.TabIndex = 3;
            this.shiftModifierCheckBox.Text = "Shift";
            this.shiftModifierCheckBox.UseVisualStyleBackColor = true;

            // popupGroupBox
            this.popupGroupBox.Controls.Add(this.durationNumericUpDown);
            this.popupGroupBox.Controls.Add(this.durationLabel);
            this.popupGroupBox.Controls.Add(this.topMostCheckBox);
            this.popupGroupBox.Location = new Point(12, 125);
            this.popupGroupBox.Name = "popupGroupBox";
            this.popupGroupBox.Size = new Size(360, 80);
            this.popupGroupBox.TabIndex = 1;
            this.popupGroupBox.TabStop = false;
            this.popupGroupBox.Text = "Popup Settings";

            // topMostCheckBox
            this.topMostCheckBox.AutoSize = true;
            this.topMostCheckBox.Checked = true;
            this.topMostCheckBox.Location = new Point(15, 25);
            this.topMostCheckBox.Name = "topMostCheckBox";
            this.topMostCheckBox.Size = new Size(110, 19);
            this.topMostCheckBox.TabIndex = 0;
            this.topMostCheckBox.Text = "Always on top";
            this.topMostCheckBox.UseVisualStyleBackColor = true;

            // durationLabel
            this.durationLabel.AutoSize = true;
            this.durationLabel.Location = new Point(15, 50);
            this.durationLabel.Name = "durationLabel";
            this.durationLabel.Size = new Size(130, 15);
            this.durationLabel.TabIndex = 1;
            this.durationLabel.Text = "Auto-close after (ms):";

            // durationNumericUpDown
            this.durationNumericUpDown.Location = new Point(155, 48);
            this.durationNumericUpDown.Maximum = new decimal(new int[] { 30000, 0, 0, 0 });
            this.durationNumericUpDown.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            this.durationNumericUpDown.Name = "durationNumericUpDown";
            this.durationNumericUpDown.Size = new Size(80, 23);
            this.durationNumericUpDown.TabIndex = 2;
            this.durationNumericUpDown.Value = new decimal(new int[] { 5000, 0, 0, 0 });

            // okButton
            this.okButton.Location = new Point(216, 220);
            this.okButton.Name = "okButton";
            this.okButton.Size = new Size(75, 25);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new EventHandler(this.okButton_Click);

            // cancelButton
            this.cancelButton.Location = new Point(297, 220);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new Size(75, 25);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new EventHandler(this.cancelButton_Click);

            // SettingsForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(384, 257);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.popupGroupBox);
            this.Controls.Add(this.hotkeyGroupBox);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "SimplePicker Settings";
            this.hotkeyGroupBox.ResumeLayout(false);
            this.hotkeyGroupBox.PerformLayout();
            this.popupGroupBox.ResumeLayout(false);
            this.popupGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.durationNumericUpDown)).EndInit();
            this.ResumeLayout(false);
        }
    }
}