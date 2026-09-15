using System;
using System.Drawing;
using System.Windows.Forms;

namespace simple_picker
{
    public partial class ColorResultForm : Form
    {
        private Color selectedColor;
        private Settings settings;
        private System.Windows.Forms.Timer? autoCloseTimer;
        private bool isClosing = false; // Flag to prevent multiple close attempts

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
                autoCloseTimer = new System.Windows.Forms.Timer();
                autoCloseTimer.Interval = settings.PopupDuration;
                autoCloseTimer.Tick += AutoCloseTimer_Tick;
                autoCloseTimer.Start();
            }
        }

        private void AutoCloseTimer_Tick(object? sender, EventArgs e)
        {
            if (!isClosing)
            {
                isClosing = true;
                autoCloseTimer?.Stop();
                this.Close();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            isClosing = true;
            autoCloseTimer?.Stop();
            base.OnFormClosing(e);
        }

        private void copyButton_Click(object sender, EventArgs e)
        {
            // Stop auto-close timer when user interacts with the form
            autoCloseTimer?.Stop();

            string rgbText = $"{selectedColor.R}, {selectedColor.G}, {selectedColor.B}";

            try
            {
                Clipboard.SetText(rgbText);

                // Show brief feedback
                copyButton.Text = "Copied!";
                System.Windows.Forms.Timer feedbackTimer = new System.Windows.Forms.Timer();
                feedbackTimer.Interval = 1000;
                feedbackTimer.Tick += (s, args) => {
                    if (!isClosing)
                    {
                        copyButton.Text = "Copy RGB";
                    }
                    feedbackTimer.Stop();
                    feedbackTimer.Dispose();
                };
                feedbackTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to copy to clipboard: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void copyHexButton_Click(object sender, EventArgs e)
        {
            // Stop auto-close timer when user interacts with the form
            autoCloseTimer?.Stop();

            string hexText = $"#{selectedColor.R:X2}{selectedColor.G:X2}{selectedColor.B:X2}";

            try
            {
                Clipboard.SetText(hexText);

                // Show brief feedback
                copyHexButton.Text = "Copied!";
                System.Windows.Forms.Timer feedbackTimer = new System.Windows.Forms.Timer();
                feedbackTimer.Interval = 1000;
                feedbackTimer.Tick += (s, args) => {
                    if (!isClosing)
                    {
                        copyHexButton.Text = "Copy HEX";
                    }
                    feedbackTimer.Stop();
                    feedbackTimer.Dispose();
                };
                feedbackTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to copy to clipboard: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                autoCloseTimer?.Stop();
                autoCloseTimer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}