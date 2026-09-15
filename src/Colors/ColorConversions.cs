using System;
using System.Drawing;

namespace simple_picker
{
    /// <summary>
    /// Provides conversion methods between different color spaces (RGB, HSB).
    /// </summary>
    public static class ColorConversions
    {
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
    }
}
