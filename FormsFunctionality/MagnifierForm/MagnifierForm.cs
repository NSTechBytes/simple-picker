using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace simple_picker
{
    public partial class MagnifierForm : Form
    {
        // P/Invoke declarations for screen capture
        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll")]
        private static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        [DllImport("gdi32.dll")]
        private static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

        // Constants for magnifier appearance and behavior
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

        /// <summary>
        /// Configures the form to act as a borderless, topmost magnifier window.
        /// </summary>
        private void SetupMagnifier()
        {
            // Calculate the total size of the form needed to accommodate the magnifier and its borders.
            int totalSize = MAGNIFIER_SIZE + (BORDER_PADDING * 2) + (BORDER_WIDTH * 4);
            this.Size = new Size(totalSize, totalSize);
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.ShowInTaskbar = false;
            this.BackColor = Color.Black;
            this.StartPosition = FormStartPosition.Manual;

            // Create the bitmap and graphics objects that will hold the magnified image.
            magnifierBitmap = new Bitmap(MAGNIFIER_SIZE, MAGNIFIER_SIZE);
            magnifierGraphics = Graphics.FromImage(magnifierBitmap);
            // Use NearestNeighbor for a blocky, pixelated zoom effect.
            magnifierGraphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            magnifierGraphics.PixelOffsetMode = PixelOffsetMode.Half;
        }

        /// <summary>
        /// Updates the magnifier's position and content based on the cursor's location.
        /// </summary>
        /// <param name="cursorPosition">The current position of the mouse cursor.</param>
        public void UpdateMagnifier(Point cursorPosition)
        {
            if (lastCursorPos == cursorPosition) return;
            lastCursorPos = cursorPosition;

            // Position the magnifier window with an offset from the cursor.
            int offsetX = 30;
            int offsetY = 30;

            int magnifierX = cursorPosition.X + offsetX;
            int magnifierY = cursorPosition.Y + offsetY;

            // Adjust the window position to ensure it stays within the screen bounds.
            if (Screen.PrimaryScreen != null)
            {
                if (magnifierX + this.Width > Screen.PrimaryScreen.Bounds.Width)
                    magnifierX = cursorPosition.X - this.Width - offsetX;
                if (magnifierY + this.Height > Screen.PrimaryScreen.Bounds.Height)
                    magnifierY = cursorPosition.Y - this.Height - offsetY;
            }

            this.Location = new Point(magnifierX, magnifierY);

            // Capture the screen area around the cursor and draw the magnified view.
            CaptureAndMagnify(cursorPosition);
            this.Invalidate(); // Force the form to repaint.
        }

        /// <summary>
        /// Captures a small area of the screen around the cursor and draws it magnified onto the bitmap.
        /// </summary>
        /// <param name="cursorPosition">The position of the cursor to center the capture on.</param>
        private void CaptureAndMagnify(Point cursorPosition)
        {
            if (magnifierGraphics == null) return;

            try
            {
                // Clear the previous image.
                magnifierGraphics.Clear(Color.Black);

                // Define the area on the screen to capture.
                int captureX = cursorPosition.X - CAPTURE_SIZE / 2;
                int captureY = cursorPosition.Y - CAPTURE_SIZE / 2;

                // Get the device context for the entire screen.
                IntPtr screenDC = GetDC(IntPtr.Zero);

                // Use a temporary bitmap to hold the captured screen portion.
                using (Bitmap captureBitmap = new Bitmap(CAPTURE_SIZE, CAPTURE_SIZE))
                {
                    using (Graphics captureGraphics = Graphics.FromImage(captureBitmap))
                    {
                        IntPtr captureDC = captureGraphics.GetHdc();
                        // Copy the screen pixels into our temporary bitmap.
                        BitBlt(captureDC, 0, 0, CAPTURE_SIZE, CAPTURE_SIZE, screenDC, captureX, captureY, SRCCOPY);
                        captureGraphics.ReleaseHdc(captureDC);
                    }

                    // Draw the captured image onto our main magnifier bitmap, scaling it up.
                    magnifierGraphics.DrawImage(captureBitmap,
                        new Rectangle(0, 0, MAGNIFIER_SIZE, MAGNIFIER_SIZE),
                        new Rectangle(0, 0, CAPTURE_SIZE, CAPTURE_SIZE),
                        GraphicsUnit.Pixel);

                    // Draw the crosshair in the center.
                    DrawCrosshair();
                }

                ReleaseDC(IntPtr.Zero, screenDC);
            }
            catch
            {
                // Ignore any errors that might occur during the screen capture process.
            }
        }

        /// <summary>
        /// Draws a crosshair and a center pixel indicator on the magnifier.
        /// </summary>
        private void DrawCrosshair()
        {
            if (magnifierGraphics == null) return;

            using (Pen crosshairPen = new Pen(Color.Red, 2))
            {
                int centerX = MAGNIFIER_SIZE / 2;
                int centerY = MAGNIFIER_SIZE / 2;
                int crosshairSize = 10;

                // Draw horizontal and vertical lines for the crosshair.
                magnifierGraphics.DrawLine(crosshairPen, centerX - crosshairSize, centerY, centerX + crosshairSize, centerY);
                magnifierGraphics.DrawLine(crosshairPen, centerX, centerY - crosshairSize, centerX, centerY + crosshairSize);

                // Draw a rectangle indicating the area of the center pixel.
                magnifierGraphics.DrawRectangle(crosshairPen,
                    centerX - ZOOM_FACTOR / 2, centerY - ZOOM_FACTOR / 2,
                    ZOOM_FACTOR, ZOOM_FACTOR);
            }
        }

        /// <summary>
        /// Overrides the default paint behavior to draw the custom magnifier view.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (magnifierBitmap != null)
            {
                e.Graphics.Clear(Color.Black);

                // Define the outer border rectangle.
                int borderX = BORDER_PADDING;
                int borderY = BORDER_PADDING;
                int borderWidth = this.ClientSize.Width - (BORDER_PADDING * 2);
                int borderHeight = this.ClientSize.Height - (BORDER_PADDING * 2);

                // Draw the white border.
                using (Pen borderPen = new Pen(Color.White, BORDER_WIDTH))
                {
                    e.Graphics.DrawRectangle(borderPen, borderX, borderY, borderWidth, borderHeight);
                }

                // Define the content area inside the border.
                int contentX = BORDER_PADDING + BORDER_WIDTH;
                int contentY = BORDER_PADDING + BORDER_WIDTH;
                int availableWidth = borderWidth - (BORDER_WIDTH * 2);
                int availableHeight = borderHeight - (BORDER_WIDTH * 2);

                // **MODIFIED**
                // Stretch the magnifier bitmap to fill the entire available content area.
                // This will make the content appear "wide" as requested.
                e.Graphics.DrawImage(magnifierBitmap,
                    new Rectangle(contentX, contentY, availableWidth, availableHeight),
                    new Rectangle(0, 0, MAGNIFIER_SIZE, MAGNIFIER_SIZE),
                    GraphicsUnit.Pixel);
            }
            base.OnPaint(e);
        }

        /// <summary>
        /// Cleans up managed and unmanaged resources.
        /// </summary>
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
            this.SuspendLayout();
            // 
            // MagnifierForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(200, 200);
            this.Name = "MagnifierForm";
            this.Text = "Magnifier";
            this.ResumeLayout(false);
        }
    }
}
