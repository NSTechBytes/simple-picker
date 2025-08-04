using System;
using System.Drawing;
using System.Windows.Forms;

namespace simple_picker
{
    public static class ColorUtilities
    {
        /// <summary>
        /// Converts a color to the specified format string
        /// </summary>
        /// <param name="color">The color to convert</param>
        /// <param name="format">The desired format</param>
        /// <returns>Color string in the specified format</returns>
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
        /// Copies text to clipboard with error handling
        /// </summary>
        /// <param name="text">Text to copy</param>
        /// <returns>True if successful, false otherwise</returns>
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
        /// Gets the display name for a color format
        /// </summary>
        /// <param name="format">Color format</param>
        /// <returns>User-friendly display name</returns>
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
        /// Shows a temporary notification tooltip
        /// </summary>
        /// <param name="message">Message to show</param>
        /// <param name="duration">Duration in milliseconds</param>
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
