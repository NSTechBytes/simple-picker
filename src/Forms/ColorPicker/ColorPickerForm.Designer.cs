#nullable enable
using System.Drawing;
using System.Windows.Forms;

namespace simple_picker
{
    partial class ColorPickerForm
    {
        private System.ComponentModel.IContainer? components = null;

        // FIX: Removed duplicate Dispose method - only keep the one in ColorPickerForm.cs

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(284, 261);
            this.Name = "ColorPickerForm";
            this.Text = "ColorPickerForm";
            this.KeyPreview = true;
            this.ResumeLayout(false);
            this.ShowInTaskbar = false;
        }
    }
}
