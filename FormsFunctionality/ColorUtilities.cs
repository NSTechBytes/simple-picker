using System;
using System.Drawing;
using System.Windows.Forms;

namespace simple_picker
{
    /// <summary>
    /// Defines the string format for colors.
    /// </summary>
    public enum ColorFormat
    {
        Hex,
        RGB
    }

    public static class ColorUtilities
    {
        /// <summary>
        /// Converts a color to the specified format string.
        /// </summary>
        /// <param name="color">The color to convert.</param>
        /// <param name="format">The desired format.</param>
        /// <returns>Color string in the specified format.</returns>
        public static string ColorToString(Color color, ColorFormat format)
        {
            return format switch
            {
                ColorFormat.Hex => $"#{color.R:X2}{color.G:X2}{color.B:X2}",
                ColorFormat.RGB => $"rgb({color.R}, {color.G}, {color.B})",
                _ => $"#{color.R:X2}{color.G:X2}{color.B:X2}" // Default to Hex
            };
        }

        /// <summary>
        /// Copies text to clipboard with error handling.
        /// </summary>
        /// <param name="text">Text to copy.</param>
        /// <returns>True if successful, false otherwise.</returns>
        public static bool CopyToClipboard(string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                    return false;

                // Try multiple times in case clipboard is busy
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        Clipboard.SetText(text);
                        return true;
                    }
                    catch (System.Runtime.InteropServices.ExternalException)
                    {
                        // Clipboard might be busy, wait a bit
                        System.Threading.Thread.Sleep(50);
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error copying to clipboard: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Converts HSB (Hue, Saturation, Brightness) values to a Color.
        /// </summary>
        /// <param name="hue">The hue value (0-360).</param>
        /// <param name="saturation">The saturation value (0-1).</param>
        /// <param name="brightness">The brightness value (0-1).</param>
        /// <returns>The corresponding System.Drawing.Color.</returns>
        public static Color HSBToColor(float hue, float saturation, float brightness)
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

        /// <summary>
        /// Converts a Color to its HSB (Hue, Saturation, Brightness) components.
        /// </summary>
        /// <param name="color">The color to convert.</param>
        /// <param name="h">The output hue value (0-360).</param>
        /// <param name="s">The output saturation value (0-1).</param>
        /// <param name="b">The output brightness value (0-1).</param>
        public static void ColorToHSB(Color color, out float h, out float s, out float b)
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


        /// <summary>
        /// Gets the display name for a color format.
        /// </summary>
        /// <param name="format">Color format.</param>
        /// <returns>User-friendly display name.</returns>
        public static string GetFormatDisplayName(ColorFormat format)
        {
            return format switch
            {
                ColorFormat.Hex => "Hex (#RRGGBB)",
                ColorFormat.RGB => "RGB (rgb(r, g, b))",
                _ => "Unknown"
            };
        }

        /// <summary>
        /// Shows a temporary notification tooltip.
        /// </summary>
        /// <param name="message">Message to show.</param>
        /// <param name="duration">Duration in milliseconds.</param>
        public static void ShowNotification(string message, int duration = 2000)
        {
            try
            {
                NotifyIcon notifyIcon = new NotifyIcon
                {
                    Icon = SystemIcons.Information,
                    Visible = true,
                    BalloonTipTitle = "SimplePicker",
                    BalloonTipText = message,
                    BalloonTipIcon = ToolTipIcon.Info
                };

                notifyIcon.ShowBalloonTip(duration);

                // Auto-dispose the notification after showing
                System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer
                {
                    Interval = duration + 1000 // Give extra time for the balloon to disappear
                };
                
                timer.Tick += (s, e) =>
                {
                    timer.Stop();
                    timer.Dispose();
                    notifyIcon.Visible = false;
                    notifyIcon.Dispose();
                };
                
                timer.Start();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error showing notification: {ex.Message}");
            }
        }
    }
}
