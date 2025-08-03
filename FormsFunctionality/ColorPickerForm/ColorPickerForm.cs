using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace simple_picker
{
    public partial class ColorPickerForm : Form
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll")]
        private static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        private Settings settings;
        private bool isPickingColor = false;
        private Cursor? crosshairCursor; // Made nullable to fix CS8618
        private MagnifierForm? magnifierForm;
        private System.Windows.Forms.Timer? updateTimer;

        public event Action<Color>? ColorSelected; // Made nullable to fix CS8618

        public ColorPickerForm(Settings settings)
        {
            this.settings = settings;
            InitializeComponent();
            CreateCrosshairCursor();
            InitializeMagnifier();
        }

        private void InitializeMagnifier()
        {
            magnifierForm = new MagnifierForm();
            
            // Setup timer for updating magnifier
            updateTimer = new System.Windows.Forms.Timer();
            updateTimer.Interval = 16; // ~60 FPS
            updateTimer.Tick += UpdateMagnifier;
        }

        private void CreateCrosshairCursor()
        {
            Bitmap cursorBitmap = new Bitmap(32, 32);
            using (Graphics g = Graphics.FromImage(cursorBitmap))
            {
                g.Clear(Color.Transparent);
                Pen pen = new Pen(Color.Red, 2);
                g.DrawLine(pen, 16, 0, 16, 32);
                g.DrawLine(pen, 0, 16, 32, 16);
                g.DrawEllipse(pen, 12, 12, 8, 8);
            }
            crosshairCursor = new Cursor(cursorBitmap.GetHicon());
        }

        public void StartColorPicking()
        {
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.Black;
            this.Opacity = 0.01; // Nearly transparent
            this.TopMost = true;
            this.Cursor = crosshairCursor ?? Cursors.Cross; // Fallback to default cross cursor
            this.Show();
            this.BringToFront();
            isPickingColor = true;

            // Show magnifier and start updating
            magnifierForm?.Show();
            updateTimer?.Start();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (isPickingColor && e.Button == MouseButtons.Left)
            {
                Color color = GetPixelColor(Cursor.Position.X, Cursor.Position.Y);
                ColorSelected?.Invoke(color);
                StopColorPicking();
            }
            else if (e.Button == MouseButtons.Right)
            {
                // Cancel color picking
                StopColorPicking();
            }
            base.OnMouseClick(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                StopColorPicking();
            }
            base.OnKeyDown(e);
        }

        private void StopColorPicking()
        {
            this.Hide();
            isPickingColor = false;
            updateTimer?.Stop();
            magnifierForm?.Hide();
        }

        private void UpdateMagnifier(object? sender, EventArgs e)
        {
            if (isPickingColor && magnifierForm != null)
            {
                magnifierForm.UpdateMagnifier(Cursor.Position);
            }
        }

        private Color GetPixelColor(int x, int y)
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(hdc, x, y);
            ReleaseDC(IntPtr.Zero, hdc);

            Color color = Color.FromArgb(
                (int)(pixel & 0x000000FF),
                (int)((pixel & 0x0000FF00) >> 8),
                (int)((pixel & 0x00FF0000) >> 16)
            );
            return color;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                updateTimer?.Stop();
                updateTimer?.Dispose();
                magnifierForm?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}