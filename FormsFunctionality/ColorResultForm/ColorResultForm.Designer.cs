#nullable enable
using System;
using System.Drawing;
using System.Windows.Forms;

namespace simple_picker
{
    partial class ColorResultForm
    {
        private System.ComponentModel.IContainer? components = null;
        private Panel colorPanel = null!;
        private Label rgbLabel = null!;
        private Label hexLabel = null!;
        private Button copyButton = null!;
        private Button copyHexButton = null!;
        private Button closeButton = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                // Dispose timer
                autoCloseTimer?.Stop();
                autoCloseTimer?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.ShowInTaskbar = false;
            this.colorPanel = new Panel();
            this.rgbLabel = new Label();
            this.hexLabel = new Label();
            this.copyButton = new Button();
            this.copyHexButton = new Button();
            this.closeButton = new Button();
            this.SuspendLayout();

            // colorPanel
            this.colorPanel.BorderStyle = BorderStyle.FixedSingle;
            this.colorPanel.Location = new Point(12, 12);
            this.colorPanel.Name = "colorPanel";
            this.colorPanel.Size = new Size(80, 80);
            this.colorPanel.TabIndex = 0;

            // rgbLabel
            this.rgbLabel.AutoSize = true;
            this.rgbLabel.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.rgbLabel.Location = new Point(105, 20);
            this.rgbLabel.Name = "rgbLabel";
            this.rgbLabel.Size = new Size(91, 14);
            this.rgbLabel.TabIndex = 1;
            this.rgbLabel.Text = "RGB: 255, 255, 255";

            // hexLabel
            this.hexLabel.AutoSize = true;
            this.hexLabel.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.hexLabel.Location = new Point(105, 45);
            this.hexLabel.Name = "hexLabel";
            this.hexLabel.Size = new Size(70, 14);
            this.hexLabel.TabIndex = 2;
            this.hexLabel.Text = "HEX: #FFFFFF";

            // copyButton
            this.copyButton.Location = new Point(105, 70);
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new Size(75, 23);
            this.copyButton.TabIndex = 3;
            this.copyButton.Text = "Copy RGB";
            this.copyButton.UseVisualStyleBackColor = true;
            this.copyButton.Click += new EventHandler(this.copyButton_Click);

            // copyHexButton
            this.copyHexButton.Location = new Point(190, 70);
            this.copyHexButton.Name = "copyHexButton";
            this.copyHexButton.Size = new Size(75, 23);
            this.copyHexButton.TabIndex = 4;
            this.copyHexButton.Text = "Copy HEX";
            this.copyHexButton.UseVisualStyleBackColor = true;
            this.copyHexButton.Click += new EventHandler(this.copyHexButton_Click);

            // closeButton
            this.closeButton.Location = new Point(275, 70);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new Size(50, 23);
            this.closeButton.TabIndex = 5;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += (s, e) => this.Close();

            // ColorResultForm
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(340, 110);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.copyHexButton);
            this.Controls.Add(this.copyButton);
            this.Controls.Add(this.hexLabel);
            this.Controls.Add(this.rgbLabel);
            this.Controls.Add(this.colorPanel);
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ColorResultForm";
            this.ShowIcon = false;
            this.StartPosition = FormStartPosition.Manual;
            this.Text = "Color Picked";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}