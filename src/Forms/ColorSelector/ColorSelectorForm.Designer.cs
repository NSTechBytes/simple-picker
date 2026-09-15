#nullable enable
using System;
using System.Drawing;
using System.Windows.Forms;

namespace simple_picker
{
    partial class ColorSelectorForm
    {
        private System.ComponentModel.IContainer? components = null;
        private Panel colorPreviewPanel = null!;
        private Label rgbValueLabel = null!;
        private NumericUpDown redNumericUpDown = null!;
        private NumericUpDown greenNumericUpDown = null!;
        private NumericUpDown blueNumericUpDown = null!;
        private TextBox hexTextBox = null!;
        private Button copyRGBButton = null!;
        private Button copyHexButton = null!;
        private Label redLabel = null!;
        private Label greenLabel = null!;
        private Label blueLabel = null!;
        private Label hexLabel = null!;
        private GroupBox rgbGroupBox = null!;
        private GroupBox hexGroupBox = null!;
        private GroupBox previewGroupBox = null!;

        // Custom panel class with enhanced double buffering
        private class DoubleBufferedPanel : Panel
        {
            public DoubleBufferedPanel()
            {
                this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                             ControlStyles.UserPaint |
                             ControlStyles.DoubleBuffer |
                             ControlStyles.OptimizedDoubleBuffer |
                             ControlStyles.ResizeRedraw, true);
            }
        }

        private void InitializeComponent()
        {
            colorWheelPanel = new DoubleBufferedPanel();
            brightnessBarPanel = new DoubleBufferedPanel();
            colorPreviewPanel = new Panel();
            rgbValueLabel = new Label();
            redNumericUpDown = new NumericUpDown();
            greenNumericUpDown = new NumericUpDown();
            blueNumericUpDown = new NumericUpDown();
            hexTextBox = new TextBox();
            copyRGBButton = new Button();
            copyHexButton = new Button();
            redLabel = new Label();
            greenLabel = new Label();
            blueLabel = new Label();
            hexLabel = new Label();
            rgbGroupBox = new GroupBox();
            hexGroupBox = new GroupBox();
            previewGroupBox = new GroupBox();
            ((System.ComponentModel.ISupportInitialize)redNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)greenNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)blueNumericUpDown).BeginInit();
            rgbGroupBox.SuspendLayout();
            hexGroupBox.SuspendLayout();
            previewGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // colorWheelPanel
            // 
            colorWheelPanel.BorderStyle = BorderStyle.FixedSingle;
            colorWheelPanel.Location = new Point(23, 27);
            colorWheelPanel.Margin = new Padding(3, 4, 3, 4);
            colorWheelPanel.Name = "colorWheelPanel";
            colorWheelPanel.Size = new Size(251, 293);
            colorWheelPanel.TabIndex = 0;
            colorWheelPanel.Paint += OnColorWheelPaint;
            colorWheelPanel.MouseDown += OnColorWheelMouseDown;
            colorWheelPanel.MouseMove += OnColorWheelMouseMove;
            colorWheelPanel.MouseUp += OnColorWheelMouseUp;
            // 
            // brightnessBarPanel
            // 
            brightnessBarPanel.BorderStyle = BorderStyle.FixedSingle;
            brightnessBarPanel.Location = new Point(309, 27);
            brightnessBarPanel.Margin = new Padding(3, 4, 3, 4);
            brightnessBarPanel.Name = "brightnessBarPanel";
            brightnessBarPanel.Size = new Size(40, 293);
            brightnessBarPanel.TabIndex = 1;
            brightnessBarPanel.Paint += OnBrightnessBarPaint;
            brightnessBarPanel.MouseDown += OnBrightnessBarMouseDown;
            brightnessBarPanel.MouseMove += OnBrightnessBarMouseMove;
            brightnessBarPanel.MouseUp += OnBrightnessBarMouseUp;
            // 
            // colorPreviewPanel
            // 
            colorPreviewPanel.BorderStyle = BorderStyle.FixedSingle;
            colorPreviewPanel.Location = new Point(17, 33);
            colorPreviewPanel.Margin = new Padding(3, 4, 3, 4);
            colorPreviewPanel.Name = "colorPreviewPanel";
            colorPreviewPanel.Size = new Size(137, 106);
            colorPreviewPanel.TabIndex = 0;
            // 
            // rgbValueLabel
            // 
            rgbValueLabel.Font = new Font("Consolas", 9F);
            rgbValueLabel.Location = new Point(171, 40);
            rgbValueLabel.Name = "rgbValueLabel";
            rgbValueLabel.Size = new Size(165, 93);
            rgbValueLabel.TabIndex = 1;
            rgbValueLabel.Text = "RGB(255, 0, 0)\nHEX: #FF0000";
            // 
            // redNumericUpDown
            // 
            redNumericUpDown.Font = new Font("Segoe UI", 9F);
            redNumericUpDown.Location = new Point(40, 36);
            redNumericUpDown.Margin = new Padding(3, 4, 3, 4);
            redNumericUpDown.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            redNumericUpDown.Name = "redNumericUpDown";
            redNumericUpDown.Size = new Size(63, 27);
            redNumericUpDown.TabIndex = 1;
            redNumericUpDown.ValueChanged += OnRGBValueChanged;
            // 
            // greenNumericUpDown
            // 
            greenNumericUpDown.Font = new Font("Segoe UI", 9F);
            greenNumericUpDown.Location = new Point(143, 36);
            greenNumericUpDown.Margin = new Padding(3, 4, 3, 4);
            greenNumericUpDown.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            greenNumericUpDown.Name = "greenNumericUpDown";
            greenNumericUpDown.Size = new Size(63, 27);
            greenNumericUpDown.TabIndex = 3;
            greenNumericUpDown.ValueChanged += OnRGBValueChanged;
            // 
            // blueNumericUpDown
            // 
            blueNumericUpDown.Font = new Font("Segoe UI", 9F);
            blueNumericUpDown.Location = new Point(246, 36);
            blueNumericUpDown.Margin = new Padding(3, 4, 3, 4);
            blueNumericUpDown.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            blueNumericUpDown.Name = "blueNumericUpDown";
            blueNumericUpDown.Size = new Size(63, 27);
            blueNumericUpDown.TabIndex = 5;
            blueNumericUpDown.ValueChanged += OnRGBValueChanged;
            // 
            // hexTextBox
            // 
            hexTextBox.Font = new Font("Consolas", 10F);
            hexTextBox.Location = new Point(63, 36);
            hexTextBox.Margin = new Padding(3, 4, 3, 4);
            hexTextBox.Name = "hexTextBox";
            hexTextBox.Size = new Size(137, 27);
            hexTextBox.TabIndex = 1;
            hexTextBox.Text = "#FF0000";
            hexTextBox.TextChanged += OnHexTextChanged;
            // 
            // copyRGBButton
            // 
            copyRGBButton.Font = new Font("Segoe UI", 8F);
            copyRGBButton.Location = new Point(17, 66);
            copyRGBButton.Margin = new Padding(3, 4, 3, 4);
            copyRGBButton.Name = "copyRGBButton";
            copyRGBButton.Size = new Size(80, 33);
            copyRGBButton.TabIndex = 6;
            copyRGBButton.Text = "Copy RGB";
            copyRGBButton.UseVisualStyleBackColor = true;
            copyRGBButton.Click += OnCopyRGBButtonClick;
            // 
            // copyHexButton
            // 
            copyHexButton.Font = new Font("Segoe UI", 8F);
            copyHexButton.Location = new Point(23, 66);
            copyHexButton.Margin = new Padding(3, 4, 3, 4);
            copyHexButton.Name = "copyHexButton";
            copyHexButton.Size = new Size(80, 33);
            copyHexButton.TabIndex = 2;
            copyHexButton.Text = "Copy HEX";
            copyHexButton.UseVisualStyleBackColor = true;
            copyHexButton.Click += OnCopyHexButtonClick;
            // 
            // redLabel
            // 
            redLabel.AutoSize = true;
            redLabel.Font = new Font("Segoe UI", 9F);
            redLabel.Location = new Point(17, 40);
            redLabel.Name = "redLabel";
            redLabel.Size = new Size(21, 20);
            redLabel.TabIndex = 0;
            redLabel.Text = "R:";
            // 
            // greenLabel
            // 
            greenLabel.AutoSize = true;
            greenLabel.Font = new Font("Segoe UI", 9F);
            greenLabel.Location = new Point(120, 40);
            greenLabel.Name = "greenLabel";
            greenLabel.Size = new Size(22, 20);
            greenLabel.TabIndex = 2;
            greenLabel.Text = "G:";
            // 
            // blueLabel
            // 
            blueLabel.AutoSize = true;
            blueLabel.Font = new Font("Segoe UI", 9F);
            blueLabel.Location = new Point(223, 40);
            blueLabel.Name = "blueLabel";
            blueLabel.Size = new Size(21, 20);
            blueLabel.TabIndex = 4;
            blueLabel.Text = "B:";
            // 
            // hexLabel
            // 
            hexLabel.AutoSize = true;
            hexLabel.Font = new Font("Segoe UI", 9F);
            hexLabel.Location = new Point(17, 40);
            hexLabel.Name = "hexLabel";
            hexLabel.Size = new Size(40, 20);
            hexLabel.TabIndex = 0;
            hexLabel.Text = "HEX:";
            // 
            // rgbGroupBox
            // 
            rgbGroupBox.Controls.Add(redLabel);
            rgbGroupBox.Controls.Add(greenLabel);
            rgbGroupBox.Controls.Add(blueLabel);
            rgbGroupBox.Controls.Add(redNumericUpDown);
            rgbGroupBox.Controls.Add(greenNumericUpDown);
            rgbGroupBox.Controls.Add(blueNumericUpDown);
            rgbGroupBox.Controls.Add(copyRGBButton);
            rgbGroupBox.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            rgbGroupBox.Location = new Point(23, 387);
            rgbGroupBox.Margin = new Padding(3, 4, 3, 4);
            rgbGroupBox.Name = "rgbGroupBox";
            rgbGroupBox.Padding = new Padding(3, 4, 3, 4);
            rgbGroupBox.Size = new Size(326, 107);
            rgbGroupBox.TabIndex = 3;
            rgbGroupBox.TabStop = false;
            rgbGroupBox.Text = "RGB Values";
            // 
            // hexGroupBox
            // 
            hexGroupBox.Controls.Add(hexLabel);
            hexGroupBox.Controls.Add(hexTextBox);
            hexGroupBox.Controls.Add(copyHexButton);
            hexGroupBox.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            hexGroupBox.Location = new Point(371, 387);
            hexGroupBox.Margin = new Padding(3, 4, 3, 4);
            hexGroupBox.Name = "hexGroupBox";
            hexGroupBox.Padding = new Padding(3, 4, 3, 4);
            hexGroupBox.Size = new Size(217, 107);
            hexGroupBox.TabIndex = 4;
            hexGroupBox.TabStop = false;
            hexGroupBox.Text = "HEX Value";
            // 
            // previewGroupBox
            // 
            previewGroupBox.Controls.Add(colorPreviewPanel);
            previewGroupBox.Controls.Add(rgbValueLabel);
            previewGroupBox.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            previewGroupBox.Location = new Point(377, 27);
            previewGroupBox.Margin = new Padding(3, 4, 3, 4);
            previewGroupBox.Name = "previewGroupBox";
            previewGroupBox.Padding = new Padding(3, 4, 3, 4);
            previewGroupBox.Size = new Size(342, 160);
            previewGroupBox.TabIndex = 2;
            previewGroupBox.TabStop = false;
            previewGroupBox.Text = "Color Preview";
            // 
            // ColorSelectorForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(731, 613);
            Controls.Add(hexGroupBox);
            Controls.Add(rgbGroupBox);
            Controls.Add(previewGroupBox);
            Controls.Add(brightnessBarPanel);
            Controls.Add(colorWheelPanel);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ColorSelectorForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Color Selector";
            ((System.ComponentModel.ISupportInitialize)redNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)greenNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)blueNumericUpDown).EndInit();
            rgbGroupBox.ResumeLayout(false);
            rgbGroupBox.PerformLayout();
            hexGroupBox.ResumeLayout(false);
            hexGroupBox.PerformLayout();
            previewGroupBox.ResumeLayout(false);
            ResumeLayout(false);
        }
        private DoubleBufferedPanel colorWheelPanel;
        private DoubleBufferedPanel brightnessBarPanel;
    }
}