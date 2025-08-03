using System;
using System.Drawing;
using System.Windows.Forms;

namespace simple_picker
{
    public partial class ColorResultForm : Form
    {
        private Color selectedColor;
        private Settings settings;
        private System.Windows.Forms.Timer? autoCloseTimer; // Fully qualified to fix ambiguity

        public ColorResultForm(Color color, Settings settings)
        {
            this.selectedColor = color;
            this.settings = settings;
            InitializeComponent();
            SetupForm();
            SetupAutoClose();
        }

        private void SetupForm()
        {
            this.TopMost = settings.TopMost;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.StartPosition = FormStartPosition.Manual;
            
            // Position the form
            if (settings.PopupX >= 0 && settings.PopupY >= 0)
            {
                this.Location = new Point(settings.PopupX, settings.PopupY);
            }
            else
            {
                // Center on screen
                this.Location = new Point(
                    (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,
                    (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2
                );
            }

            // Update UI with color information
            colorPanel.BackColor = selectedColor;
            rgbLabel.Text = $"RGB: {selectedColor.R}, {selectedColor.G}, {selectedColor.B}";
            hexLabel.Text = $"HEX: #{selectedColor.R:X2}{selectedColor.G:X2}{selectedColor.B:X2}";
        }

        private void SetupAutoClose()
        {
            if (settings.PopupDuration > 0)
            {
                autoCloseTimer = new System.Windows.Forms.Timer(); // Fully qualified
                autoCloseTimer.Interval = settings.PopupDuration;
                autoCloseTimer.Tick += (s, e) => {
                    autoCloseTimer.Stop();
                    this.Close();
                };
                autoCloseTimer.Start();
            }
        }

        private void copyButton_Click(object sender, EventArgs e)
        {
            string rgbText = $"{selectedColor.R}, {selectedColor.G}, {selectedColor.B}";
            Clipboard.SetText(rgbText);
            
            // Show brief feedback
            copyButton.Text = "Copied!";
            System.Windows.Forms.Timer feedbackTimer = new System.Windows.Forms.Timer(); // Fully qualified
            feedbackTimer.Interval = 1000;
            feedbackTimer.Tick += (s, args) => {
                copyButton.Text = "Copy RGB";
                feedbackTimer.Stop();
                feedbackTimer.Dispose();
            };
            feedbackTimer.Start();
        }

        private void copyHexButton_Click(object sender, EventArgs e)
        {
            string hexText = $"#{selectedColor.R:X2}{selectedColor.G:X2}{selectedColor.B:X2}";
            Clipboard.SetText(hexText);
            
            // Show brief feedback
            copyHexButton.Text = "Copied!";
            System.Windows.Forms.Timer feedbackTimer = new System.Windows.Forms.Timer(); // Fully qualified
            feedbackTimer.Interval = 1000;
            feedbackTimer.Tick += (s, args) => {
                copyHexButton.Text = "Copy HEX";
                feedbackTimer.Stop();
                feedbackTimer.Dispose();
            };
            feedbackTimer.Start();
        }
    }
}