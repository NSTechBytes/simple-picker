#nullable enable
using System.Drawing;
using System.Windows.Forms;

namespace simple_picker
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer? components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                // Dispose managed resources
                trayIcon?.Dispose();
                globalHotkey?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(284, 261);
            this.Name = "MainForm";
            this.Text = "SimplePicker";
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
        }
    }
}