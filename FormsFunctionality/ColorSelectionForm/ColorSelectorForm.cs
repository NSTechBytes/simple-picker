using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

namespace simple_picker
{
    public partial class ColorSelectorForm : Form
    {
        private Color selectedColor = Color.Red;
        private float hue = 0f;
        private float saturation = 1f;
        private float brightness = 1f;
        private bool updatingFromCode = false;

        private Bitmap? colorWheelBitmap;
        private Bitmap? brightnessBarBitmap;
        private Bitmap? colorWheelWithIndicator; // Cache for wheel with indicator
        private Bitmap? brightnessBarWithIndicator; // Cache for brightness bar with indicator
        private Point colorWheelCenter;
        private int colorWheelRadius;
        private Rectangle brightnessBarRect;

        private bool isDraggingWheel = false;
        private bool isDraggingBrightness = false;
        private bool needsColorWheelRedraw = true;
        private bool needsBrightnessBarRedraw = true;

        public event Action<Color>? ColorSelected;

        public ColorSelectorForm()
        {
            InitializeComponent();
            SetupForm();
            CreateColorWheel();
            CreateBrightnessBar();
            UpdateUI();
        }

        private void SetupForm()
        {
            this.Text = "Color Selector";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(750, 550);

            // Enhanced double buffering to reduce flickering
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.UserPaint |
                         ControlStyles.DoubleBuffer |
                         ControlStyles.OptimizedDoubleBuffer |
                         ControlStyles.ResizeRedraw, true);

            // Set background color for better appearance
            this.BackColor = SystemColors.Control;
        }

        private void CreateColorWheel()
        {
            int wheelSize = 220;
            colorWheelRadius = wheelSize / 2 - 15; // Leave some border space
            colorWheelCenter = new Point(wheelSize / 2, wheelSize / 2);

            colorWheelBitmap = new Bitmap(wheelSize, wheelSize);
            colorWheelWithIndicator = new Bitmap(wheelSize, wheelSize); // Initialize cache

            using (Graphics g = Graphics.FromImage(colorWheelBitmap))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);

                // Create the color wheel using optimized pixel setting
                for (int x = 0; x < wheelSize; x++)
                {
                    for (int y = 0; y < wheelSize; y++)
                    {
                        int dx = x - colorWheelCenter.X;
                        int dy = y - colorWheelCenter.Y;
                        double distance = Math.Sqrt(dx * dx + dy * dy);

                        if (distance <= colorWheelRadius)
                        {
                            double angle = Math.Atan2(dy, dx);
                            if (angle < 0)
                                angle += 2 * Math.PI;

                            float hueValue = (float)(angle * 180 / Math.PI);
                            float satValue = (float)(distance / colorWheelRadius);

                            Color pixelColor = HSBToColor(hueValue, satValue, 1.0f);
                            colorWheelBitmap.SetPixel(x, y, pixelColor);
                        }
                    }
                }

                // Add a subtle border
                using (Pen borderPen = new Pen(Color.Gray, 2))
                {
                    g.DrawEllipse(borderPen, colorWheelCenter.X - colorWheelRadius,
                                 colorWheelCenter.Y - colorWheelRadius,
                                 colorWheelRadius * 2, colorWheelRadius * 2);
                }
            }

            needsColorWheelRedraw = true;
        }

        private void CreateBrightnessBar()
        {
            // Use the actual panel size instead of hardcoded values
            int barWidth = brightnessBarPanel.Width - 2; // Account for border
            int barHeight = brightnessBarPanel.Height - 2; // Account for border
            brightnessBarRect = new Rectangle(0, 0, barWidth, barHeight);

            brightnessBarBitmap = new Bitmap(barWidth, barHeight);
            brightnessBarWithIndicator = new Bitmap(barWidth, barHeight); // Initialize cache
            UpdateBrightnessBar();
        }

        private void UpdateBrightnessBar()
        {
            if (brightnessBarBitmap == null) return;

            using (Graphics g = Graphics.FromImage(brightnessBarBitmap))
            {
                g.Clear(Color.Transparent);

                // Create gradient brush for smoother appearance - fill entire bitmap
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    new Rectangle(0, 0, brightnessBarBitmap.Width, brightnessBarBitmap.Height),
                    HSBToColor(hue, saturation, 1.0f), // Bright at top
                    HSBToColor(hue, saturation, 0.0f), // Dark at bottom
                    LinearGradientMode.Vertical))
                {
                    // Fill the entire bitmap area
                    g.FillRectangle(brush, 0, 0, brightnessBarBitmap.Width, brightnessBarBitmap.Height);
                }
            }

            needsBrightnessBarRedraw = true;
        }

        // Optimized method to create color wheel with indicator
        private void UpdateColorWheelWithIndicator()
        {
            if (colorWheelBitmap == null || colorWheelWithIndicator == null) return;

            // Copy the base color wheel
            using (Graphics g = Graphics.FromImage(colorWheelWithIndicator))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);
                g.DrawImage(colorWheelBitmap, 0, 0);

                // Draw indicator
                double angleRad = hue * Math.PI / 180.0;
                double distance = saturation * colorWheelRadius;
                int indicatorX = colorWheelCenter.X + (int)(distance * Math.Cos(angleRad));
                int indicatorY = colorWheelCenter.Y + (int)(distance * Math.Sin(angleRad));

                int indicatorSize = 10;
                Rectangle indicatorRect = new Rectangle(
                    indicatorX - indicatorSize / 2,
                    indicatorY - indicatorSize / 2,
                    indicatorSize,
                    indicatorSize
                );

                // Draw indicator with current selected color
                using (SolidBrush brush = new SolidBrush(selectedColor))
                {
                    g.FillEllipse(brush, indicatorRect);
                }

                // Draw white border around indicator for visibility
                using (Pen indicatorPen = new Pen(Color.White, 2))
                {
                    g.DrawEllipse(indicatorPen, indicatorRect);
                }

                // Draw center crosshair to show the exact center
                using (Pen centerPen = new Pen(Color.Black, 1))
                {
                    // Draw horizontal line
                    g.DrawLine(centerPen, colorWheelCenter.X - 5, colorWheelCenter.Y,
                              colorWheelCenter.X + 5, colorWheelCenter.Y);
                    // Draw vertical line
                    g.DrawLine(centerPen, colorWheelCenter.X, colorWheelCenter.Y - 5,
                              colorWheelCenter.X, colorWheelCenter.Y + 5);
                }

                // Draw white outline for center crosshair for better visibility
                using (Pen centerOutlinePen = new Pen(Color.White, 3))
                {
                    // Draw horizontal line
                    g.DrawLine(centerOutlinePen, colorWheelCenter.X - 5, colorWheelCenter.Y,
                              colorWheelCenter.X + 5, colorWheelCenter.Y);
                    // Draw vertical line
                    g.DrawLine(centerOutlinePen, colorWheelCenter.X, colorWheelCenter.Y - 5,
                              colorWheelCenter.X, colorWheelCenter.Y + 5);
                }

                // Redraw the black center crosshair on top
                using (Pen centerPen = new Pen(Color.Black, 1))
                {
                    // Draw horizontal line
                    g.DrawLine(centerPen, colorWheelCenter.X - 5, colorWheelCenter.Y,
                              colorWheelCenter.X + 5, colorWheelCenter.Y);
                    // Draw vertical line
                    g.DrawLine(centerPen, colorWheelCenter.X, colorWheelCenter.Y - 5,
                              colorWheelCenter.X, colorWheelCenter.Y + 5);
                }
            }

            needsColorWheelRedraw = false;
        }

        // Optimized method to create brightness bar with indicator
        private void UpdateBrightnessBarWithIndicator()
        {
            if (brightnessBarBitmap == null || brightnessBarWithIndicator == null) return;

            using (Graphics g = Graphics.FromImage(brightnessBarWithIndicator))
            {
                g.Clear(Color.Transparent);
                g.DrawImage(brightnessBarBitmap, 0, 0);

                // Draw brightness indicator
                int indicatorY = (int)((1.0f - brightness) * brightnessBarBitmap.Height);

                // Draw indicator line across the full width
                using (Pen whitePen = new Pen(Color.White, 3))
                {
                    g.DrawLine(whitePen, 0, indicatorY, brightnessBarBitmap.Width, indicatorY);
                }
                using (Pen blackPen = new Pen(Color.Black, 1))
                {
                    g.DrawLine(blackPen, 0, indicatorY, brightnessBarBitmap.Width, indicatorY);
                }
            }

            needsBrightnessBarRedraw = false;
        }

        private Color HSBToColor(float hue, float saturation, float brightness)
        {
            if (saturation == 0)
            {
                int gray = (int)(brightness * 255);
                return Color.FromArgb(gray, gray, gray);
            }

            float h = hue / 60f;
            int i = (int)Math.Floor(h);
            float f = h - i;
            float p = brightness * (1 - saturation);
            float q = brightness * (1 - saturation * f);
            float t = brightness * (1 - saturation * (1 - f));

            float r, g, b;
            switch (i % 6)
            {
                case 0: r = brightness; g = t; b = p; break;
                case 1: r = q; g = brightness; b = p; break;
                case 2: r = p; g = brightness; b = t; break;
                case 3: r = p; g = q; b = brightness; break;
                case 4: r = t; g = p; b = brightness; break;
                default: r = brightness; g = p; b = q; break;
            }

            return Color.FromArgb(
                Math.Max(0, Math.Min(255, (int)(r * 255))),
                Math.Max(0, Math.Min(255, (int)(g * 255))),
                Math.Max(0, Math.Min(255, (int)(b * 255)))
            );
        }

        private void ColorToHSB(Color color, out float h, out float s, out float b)
        {
            float r = color.R / 255f;
            float g = color.G / 255f;
            float bl = color.B / 255f;

            float max = Math.Max(r, Math.Max(g, bl));
            float min = Math.Min(r, Math.Min(g, bl));
            float delta = max - min;

            // Brightness
            b = max;

            // Saturation
            s = max == 0 ? 0 : delta / max;

            // Hue
            if (delta == 0)
            {
                h = 0;
            }
            else if (max == r)
            {
                h = 60 * (((g - bl) / delta) % 6);
            }
            else if (max == g)
            {
                h = 60 * ((bl - r) / delta + 2);
            }
            else
            {
                h = 60 * ((r - g) / delta + 4);
            }

            if (h < 0) h += 360;
        }

        private void UpdateSelectedColor()
        {
            selectedColor = HSBToColor(hue, saturation, brightness);
            UpdateBrightnessBar();
            needsColorWheelRedraw = true;
            needsBrightnessBarRedraw = true;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (updatingFromCode) return;

            updatingFromCode = true;

            // Update color preview
            colorPreviewPanel.BackColor = selectedColor;

            // Update RGB values
            redNumericUpDown.Value = selectedColor.R;
            greenNumericUpDown.Value = selectedColor.G;
            blueNumericUpDown.Value = selectedColor.B;

            // Update HEX value
            hexTextBox.Text = $"#{selectedColor.R:X2}{selectedColor.G:X2}{selectedColor.B:X2}";

            // Update labels - Show both RGB and HEX values in the preview
            rgbValueLabel.Text = $"RGB({selectedColor.R}, {selectedColor.G}, {selectedColor.B})\n" +
                                $"HEX: #{selectedColor.R:X2}{selectedColor.G:X2}{selectedColor.B:X2}";

            updatingFromCode = false;

            // Only invalidate specific panels instead of the entire form
            if (needsColorWheelRedraw)
                colorWheelPanel.Invalidate();
            if (needsBrightnessBarRedraw)
                brightnessBarPanel.Invalidate();
        }

        private void OnRGBValueChanged(object sender, EventArgs e)
        {
            if (updatingFromCode) return;

            Color newColor = Color.FromArgb(
                (int)redNumericUpDown.Value,
                (int)greenNumericUpDown.Value,
                (int)blueNumericUpDown.Value
            );

            ColorToHSB(newColor, out hue, out saturation, out brightness);
            selectedColor = newColor;
            UpdateBrightnessBar();
            needsColorWheelRedraw = true;
            needsBrightnessBarRedraw = true;
            UpdateUI();
        }

        private void OnHexTextChanged(object sender, EventArgs e)
        {
            if (updatingFromCode) return;

            string hexText = hexTextBox.Text.Replace("#", "");
            if (hexText.Length == 6)
            {
                try
                {
                    int r = int.Parse(hexText.Substring(0, 2), NumberStyles.HexNumber);
                    int g = int.Parse(hexText.Substring(2, 2), NumberStyles.HexNumber);
                    int b = int.Parse(hexText.Substring(4, 2), NumberStyles.HexNumber);

                    Color newColor = Color.FromArgb(r, g, b);
                    ColorToHSB(newColor, out hue, out saturation, out brightness);
                    selectedColor = newColor;
                    UpdateBrightnessBar();
                    needsColorWheelRedraw = true;
                    needsBrightnessBarRedraw = true;
                    UpdateUI();
                }
                catch
                {
                    // Invalid hex format - silently ignore
                }
            }
        }

        private void OnColorWheelMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                HandleColorWheelClick(e.Location);
                isDraggingWheel = true;
                ((Control)sender).Capture = true;
            }
        }

        private void OnColorWheelMouseMove(object sender, MouseEventArgs e)
        {
            if (isDraggingWheel)
            {
                HandleColorWheelClick(e.Location);
            }
        }

        private void OnColorWheelMouseUp(object sender, MouseEventArgs e)
        {
            if (isDraggingWheel)
            {
                isDraggingWheel = false;
                ((Control)sender).Capture = false;
            }
        }

        private void HandleColorWheelClick(Point point)
        {
            int dx = point.X - colorWheelCenter.X;
            int dy = point.Y - colorWheelCenter.Y;
            double distance = Math.Sqrt(dx * dx + dy * dy);

            if (distance <= colorWheelRadius)
            {
                double angle = Math.Atan2(dy, dx);
                if (angle < 0)
                    angle += 2 * Math.PI;

                hue = (float)(angle * 180 / Math.PI);
                saturation = Math.Min(1.0f, (float)(distance / colorWheelRadius));

                UpdateSelectedColor();
            }
        }

        private void OnBrightnessBarMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                HandleBrightnessBarClick(e.Y);
                isDraggingBrightness = true;
                ((Control)sender).Capture = true;
            }
        }

        private void OnBrightnessBarMouseMove(object sender, MouseEventArgs e)
        {
            if (isDraggingBrightness)
            {
                HandleBrightnessBarClick(e.Y);
            }
        }

        private void OnBrightnessBarMouseUp(object sender, MouseEventArgs e)
        {
            if (isDraggingBrightness)
            {
                isDraggingBrightness = false;
                ((Control)sender).Capture = false;
            }
        }

        private void HandleBrightnessBarClick(int y)
        {
            // Use the actual bitmap height instead of the original rect height
            float relativeY = Math.Max(0, Math.Min(1, (float)y / brightnessBarBitmap.Height));
            brightness = 1.0f - relativeY;
            UpdateSelectedColor();
        }

        private void OnColorWheelPaint(object sender, PaintEventArgs e)
        {
            if (needsColorWheelRedraw)
            {
                UpdateColorWheelWithIndicator();
            }

            if (colorWheelWithIndicator != null)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawImage(colorWheelWithIndicator, 0, 0);
            }
        }

        private void OnBrightnessBarPaint(object sender, PaintEventArgs e)
        {
            if (needsBrightnessBarRedraw)
            {
                UpdateBrightnessBarWithIndicator();
            }

            if (brightnessBarWithIndicator != null)
            {
                // Draw the brightness bar to fill the entire panel area
                e.Graphics.DrawImage(brightnessBarWithIndicator, 1, 1); // Offset by 1 pixel for border
            }
        }

        private void OnCopyRGBButtonClick(object sender, EventArgs e)
        {
            string rgbText = $"{selectedColor.R}, {selectedColor.G}, {selectedColor.B}";
            Clipboard.SetText(rgbText);
            ShowCopyFeedback(copyRGBButton, "✓");
        }

        private void OnCopyHexButtonClick(object sender, EventArgs e)
        {
            string hexText = hexTextBox.Text;
            Clipboard.SetText(hexText);
            ShowCopyFeedback(copyHexButton, "✓");
        }

        private void ShowCopyFeedback(Button button, string message)
        {
            string originalText = button.Text;
            button.Text = message;
            button.BackColor = Color.LightGreen;

            System.Windows.Forms.Timer feedbackTimer = new System.Windows.Forms.Timer();
            feedbackTimer.Interval = 1000;
            feedbackTimer.Tick += (s, args) => {
                button.Text = originalText;
                button.BackColor = SystemColors.Control;
                button.UseVisualStyleBackColor = true;
                feedbackTimer.Stop();
                feedbackTimer.Dispose();
            };
            feedbackTimer.Start();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                colorWheelBitmap?.Dispose();
                brightnessBarBitmap?.Dispose();
                colorWheelWithIndicator?.Dispose();
                brightnessBarWithIndicator?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}