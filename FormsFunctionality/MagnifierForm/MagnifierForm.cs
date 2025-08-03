using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace simple_picker
{
    public partial class MagnifierForm : Form
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll")]
        private static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        [DllImport("gdi32.dll")]
        private static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

        private const int SRCCOPY = 0x00CC0020;
        private const int MAGNIFIER_SIZE = 150;
        private const int CAPTURE_SIZE = 20;
        private const int ZOOM_FACTOR = 10;
        private const int BORDER_WIDTH = 2;
        private const int BORDER_PADDING = 5;

        private Point lastCursorPos = Point.Empty;
        private Bitmap? magnifierBitmap;
        private Graphics? magnifierGraphics;

        public MagnifierForm()
        {
            InitializeComponent();
            SetupMagnifier();
        }

        private void SetupMagnifier()
        {
            // Calculate total form size needed
            int totalSize = MAGNIFIER_SIZE + (BORDER_PADDING * 2) + (BORDER_WIDTH * 4); // Extra space for border
            this.Size = new Size(totalSize, totalSize);
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.ShowInTaskbar = false;
            this.BackColor = Color.Black;
            this.StartPosition = FormStartPosition.Manual;

            // Create bitmap for magnifier content
            magnifierBitmap = new Bitmap(MAGNIFIER_SIZE, MAGNIFIER_SIZE);
            magnifierGraphics = Graphics.FromImage(magnifierBitmap);
            magnifierGraphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            magnifierGraphics.PixelOffsetMode = PixelOffsetMode.Half;
        }

        public void UpdateMagnifier(Point cursorPosition)
        {
            if (lastCursorPos == cursorPosition) return;
            lastCursorPos = cursorPosition;

            // Position magnifier window
            int offsetX = 30; // Offset from cursor
            int offsetY = 30;

            // Adjust position to keep magnifier on screen
            int magnifierX = cursorPosition.X + offsetX;
            int magnifierY = cursorPosition.Y + offsetY;

            // FIX: Add null checks for Screen.PrimaryScreen
            if (magnifierX + this.Width > (Screen.PrimaryScreen?.Bounds.Width ?? 1920))
                magnifierX = cursorPosition.X - this.Width - offsetX;
            if (magnifierY + this.Height > (Screen.PrimaryScreen?.Bounds.Height ?? 1080))
                magnifierY = cursorPosition.Y - this.Height - offsetY;

            this.Location = new Point(magnifierX, magnifierY);

            // Capture screen area around cursor
            CaptureAndMagnify(cursorPosition);
            this.Invalidate();
        }

        private void CaptureAndMagnify(Point cursorPosition)
        {
            if (magnifierGraphics == null) return;

            try
            {
                // Clear the magnifier bitmap
                magnifierGraphics.Clear(Color.Black);

                // Calculate capture area
                int captureX = cursorPosition.X - CAPTURE_SIZE / 2;
                int captureY = cursorPosition.Y - CAPTURE_SIZE / 2;

                // Get screen DC
                IntPtr screenDC = GetDC(IntPtr.Zero);
                IntPtr memDC = magnifierGraphics.GetHdc();

                // Capture and scale the screen area
                using (Bitmap captureBitmap = new Bitmap(CAPTURE_SIZE, CAPTURE_SIZE))
                {
                    using (Graphics captureGraphics = Graphics.FromImage(captureBitmap))
                    {
                        IntPtr captureDC = captureGraphics.GetHdc();

                        // Copy screen pixels to capture bitmap
                        BitBlt(captureDC, 0, 0, CAPTURE_SIZE, CAPTURE_SIZE, screenDC, captureX, captureY, SRCCOPY);

                        captureGraphics.ReleaseHdc(captureDC);
                    }

                    // Draw the captured area scaled up
                    magnifierGraphics.ReleaseHdc(memDC);
                    magnifierGraphics.DrawImage(captureBitmap,
                        new Rectangle(0, 0, MAGNIFIER_SIZE, MAGNIFIER_SIZE),
                        new Rectangle(0, 0, CAPTURE_SIZE, CAPTURE_SIZE),
                        GraphicsUnit.Pixel);

                    // Draw crosshair in center
                    DrawCrosshair();
                }

                ReleaseDC(IntPtr.Zero, screenDC);
            }
            catch
            {
                // Ignore errors during capture
            }
        }

        private void DrawCrosshair()
        {
            if (magnifierGraphics == null) return;

            using (Pen crosshairPen = new Pen(Color.Red, 2))
            {
                int centerX = MAGNIFIER_SIZE / 2;
                int centerY = MAGNIFIER_SIZE / 2;
                int crosshairSize = 10;

                // Draw crosshair lines
                magnifierGraphics.DrawLine(crosshairPen,
                    centerX - crosshairSize, centerY,
                    centerX + crosshairSize, centerY);
                magnifierGraphics.DrawLine(crosshairPen,
                    centerX, centerY - crosshairSize,
                    centerX, centerY + crosshairSize);

                // Draw center pixel outline
                magnifierGraphics.DrawRectangle(crosshairPen,
                    centerX - ZOOM_FACTOR / 2, centerY - ZOOM_FACTOR / 2,
                    ZOOM_FACTOR, ZOOM_FACTOR);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (magnifierBitmap != null)
            {
                // Draw black background
                e.Graphics.Clear(Color.Black);

                // Calculate border position - need to account for pen width
                int borderX = BORDER_PADDING;
                int borderY = BORDER_PADDING;
                int borderWidth = this.ClientSize.Width - (BORDER_PADDING * 2);
                int borderHeight = this.ClientSize.Height - (BORDER_PADDING * 2);

                // Draw white border
                using (Pen borderPen = new Pen(Color.White, BORDER_WIDTH))
                {
                    e.Graphics.DrawRectangle(borderPen, borderX, borderY, borderWidth, borderHeight);
                }

                // Draw magnifier content inside the border - centered
                int contentX = BORDER_PADDING + BORDER_WIDTH;
                int contentY = BORDER_PADDING + BORDER_WIDTH;

                // Calculate available space inside border for content
                int availableWidth = borderWidth - (BORDER_WIDTH * 2);
                int availableHeight = borderHeight - (BORDER_WIDTH * 2);

                // Scale the magnifier content to fit inside the border if needed
                int drawWidth = Math.Min(MAGNIFIER_SIZE, availableWidth);
                int drawHeight = Math.Min(MAGNIFIER_SIZE, availableHeight);

                e.Graphics.DrawImage(magnifierBitmap,
                    new Rectangle(contentX, contentY, drawWidth, drawHeight),
                    new Rectangle(0, 0, MAGNIFIER_SIZE, MAGNIFIER_SIZE),
                    GraphicsUnit.Pixel);
            }
            base.OnPaint(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                magnifierGraphics?.Dispose();
                magnifierBitmap?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.ShowInTaskbar = false;
            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(200, 200);
            this.Name = "MagnifierForm";
            this.Text = "Magnifier";
            this.ResumeLayout(false);
        }
    }
}