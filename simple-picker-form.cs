using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using System.Text.Json; // Using System.Text.Json instead of Newtonsoft.Json

namespace simple_picker
{
    public partial class MainForm : Form
    {
        private NotifyIcon? trayIcon;
        private GlobalHotkey? globalHotkey;
        private Settings settings = new Settings(); // FIX: Initialize to avoid CS8618
        private ColorPickerForm? colorPickerForm;
        private string settingsPath = "settings.json";

        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        public MainForm()
        {
            SetProcessDPIAware();
            InitializeComponent();
            LoadSettings();
            InitializeTrayIcon();
            InitializeGlobalHotkey();
        }

        private void LoadSettings()
        {
            try
            {
                if (File.Exists(settingsPath))
                {
                    string json = File.ReadAllText(settingsPath);
                    settings = System.Text.Json.JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
                }
                else
                {
                    settings = new Settings();
                    SaveSettings();
                }
            }
            catch
            {
                settings = new Settings();
            }
        }

        private void SaveSettings()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = System.Text.Json.JsonSerializer.Serialize(settings, options);
                File.WriteAllText(settingsPath, json);
            }
            catch { }
        }

        private void InitializeTrayIcon()
        {
            trayIcon = new NotifyIcon()
            {
                Icon = CreateTrayIcon(),
                ContextMenuStrip = CreateContextMenu(),
                Text = "SimplePicker - Click to pick colors",
                Visible = true
            };

            trayIcon.DoubleClick += (s, e) => TriggerColorPicker();
        }

        private Icon CreateTrayIcon()
        {
            try
            {
                string iconPath = Path.Combine(Application.StartupPath, "resources", "icon.ico");
                return new Icon(iconPath);
            }
            catch
            {
                // Fallback to a basic programmatic icon if file load fails
                Bitmap bitmap = new Bitmap(16, 16);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.Clear(Color.White);
                    g.FillEllipse(new SolidBrush(Color.Blue), 2, 2, 12, 12);
                    g.DrawEllipse(new Pen(Color.Black, 1), 2, 2, 12, 12);
                }
                return Icon.FromHandle(bitmap.GetHicon());
            }
        }


        private ContextMenuStrip CreateContextMenu()
        {
            ContextMenuStrip menu = new ContextMenuStrip();

            ToolStripMenuItem pickColor = new ToolStripMenuItem("Pick Color");
            pickColor.Click += (s, e) => TriggerColorPicker();

            ToolStripMenuItem settings = new ToolStripMenuItem("Settings");
            settings.Click += (s, e) => ShowSettings();

            ToolStripMenuItem exit = new ToolStripMenuItem("Exit");
            exit.Click += (s, e) => ExitApplication();

            menu.Items.Add(pickColor);
            menu.Items.Add(settings);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(exit);
            return menu;
        }

        private void InitializeGlobalHotkey()
        {
            globalHotkey = new GlobalHotkey();
            globalHotkey.RegisterHotkey(settings.HotkeyModifiers, settings.HotkeyKey, TriggerColorPicker);
        }

        private void TriggerColorPicker()
        {
            if (colorPickerForm == null || colorPickerForm.IsDisposed)
            {
                colorPickerForm = new ColorPickerForm(settings);
                colorPickerForm.ColorSelected += OnColorSelected;
            }
            colorPickerForm.StartColorPicking();
        }

        private void OnColorSelected(Color color)
        {
            ColorResultForm resultForm = new ColorResultForm(color, settings);
            resultForm.Show();
        }

        private void ShowSettings()
        {
            SettingsForm settingsForm = new SettingsForm(settings);
            if (settingsForm.ShowDialog() == DialogResult.OK)
            {
                SaveSettings();
                // Restart hotkey with new settings
                globalHotkey?.Dispose();
                InitializeGlobalHotkey();
            }
        }

        private void ExitApplication()
        {
            if (trayIcon != null)
            {
                trayIcon.Visible = false;
            }
            globalHotkey?.Dispose();
            Application.Exit();
        }

        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(false); // Keep form hidden
        }

        // Removed duplicate Dispose method - only keeping the one in Designer.cs
    }
}