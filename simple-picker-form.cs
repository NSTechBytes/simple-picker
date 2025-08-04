using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace simple_picker
{
    public partial class MainForm : Form
    {
        private NotifyIcon? trayIcon;
        private GlobalHotkey? globalHotkey;
        private Settings settings = new Settings();
        private ColorPickerForm? colorPickerForm;
        private UpdateManager? updateManager;

        // Correctly define paths for settings file
        private readonly string appDataDirectory;
        private readonly string settingsPath;

        private System.Threading.Timer? updateTimer;

        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        public MainForm()
        {
            // Initialize paths in the constructor
            string userAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            appDataDirectory = Path.Combine(userAppData, "SimplePicker");
            settingsPath = Path.Combine(appDataDirectory, "settings.json");

            SetProcessDPIAware();
            InitializeComponent();
            LoadSettings();
            InitializeTrayIcon();
            InitializeGlobalHotkey();
            InitializeUpdateManager();
        }

        private void LoadSettings()
        {
            try
            {
                if (File.Exists(settingsPath))
                {
                    string json = File.ReadAllText(settingsPath);
                    settings = JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
                }
                else
                {
                    settings = new Settings();
                    SaveSettings(); // Save initial settings if file doesn't exist
                }

                // Reset session flag when loading settings (new program session)
                settings.UpdateDialogShownThisSession = false;
            }
            catch (Exception ex) // Catch specific exceptions for better debugging
            {
                // Optionally log the exception
                // Console.WriteLine($"Error loading settings: {ex.Message}");
                settings = new Settings();
                settings.UpdateDialogShownThisSession = false;
            }
        }

        private void SaveSettings()
        {
            try
            {
                // Ensure the directory exists before saving the file
                Directory.CreateDirectory(appDataDirectory);

                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(settings, options);
                File.WriteAllText(settingsPath, json);
            }
            catch (Exception ex) // Catch specific exceptions for better debugging
            {
                // Optionally log the exception
                // Console.WriteLine($"Error saving settings: {ex.Message}");
            }
        }

        private void InitializeTrayIcon()
        {
            trayIcon = new NotifyIcon()
            {
                Icon = CreateTrayIcon(),
                ContextMenuStrip = CreateContextMenu(),
                Text = "SimplePicker - Double Click to select colors",
                Visible = true
            };

            trayIcon.DoubleClick += (s, e) => TriggerColorPicker();
        }

        private Icon CreateTrayIcon()
        {
            try
            {
                // It's safer to use the base directory of the application for resource paths
                string iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resources", "icon.ico");
                return new Icon(iconPath);
            }
            catch
            {
                // Fallback to a basic programmatic icon if file load fails
                Bitmap bitmap = new Bitmap(16, 16);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.Clear(Color.White);
                    using (SolidBrush brush = new SolidBrush(Color.Blue))
                    {
                        g.FillEllipse(brush, 2, 2, 12, 12);
                    }
                    using (Pen pen = new Pen(Color.Black, 1))
                    {
                        g.DrawEllipse(pen, 2, 2, 12, 12);
                    }
                }
                return Icon.FromHandle(bitmap.GetHicon());
            }
        }

        private ContextMenuStrip CreateContextMenu()
        {
            ContextMenuStrip menu = new ContextMenuStrip();

            ToolStripMenuItem pickColor = new ToolStripMenuItem("Pick Color from Screen");
            pickColor.Click += (s, e) => TriggerColorPicker();

            ToolStripMenuItem colorSelector = new ToolStripMenuItem("Color Selector");
            colorSelector.Click += (s, e) => ShowColorSelector();

            ToolStripMenuItem settingsItem = new ToolStripMenuItem("Settings");
            settingsItem.Click += (s, e) => ShowSettings();

            ToolStripMenuItem checkUpdates = new ToolStripMenuItem("Check for Updates");
            checkUpdates.Click += async (s, e) => await CheckForUpdatesManually();

            ToolStripMenuItem exit = new ToolStripMenuItem("Exit");
            exit.Click += (s, e) => ExitApplication();

            menu.Items.Add(pickColor);
            menu.Items.Add(colorSelector);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(settingsItem);
            menu.Items.Add(checkUpdates);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(exit);
            return menu;
        }

        private void InitializeGlobalHotkey()
        {
            globalHotkey = new GlobalHotkey();

            // Register color picker hotkey
            globalHotkey.RegisterColorPickerHotkey(settings.HotkeyModifiers, settings.HotkeyKey, TriggerColorPicker);

            // Register color selector hotkey
            globalHotkey.RegisterColorSelectorHotkey(settings.ColorSelectorHotkeyModifiers, settings.ColorSelectorHotkeyKey, ShowColorSelector);
        }

        private void InitializeUpdateManager()
        {
            updateManager = new UpdateManager(settings, this);

            // Start initial update check after a short delay
            Task.Delay(2000).ContinueWith(async _ => await CheckForUpdatesInBackground());

            // Set up periodic update checks using a timer
            SetupUpdateTimer();
        }

        private void SetupUpdateTimer()
        {
            // Dispose existing timer if any
            updateTimer?.Dispose();

            if (settings.AutoCheckForUpdates)
            {
                // Create timer that runs every interval specified in settings
                int intervalMs = settings.UpdateCheckIntervalSeconds * 1000;
                updateTimer = new System.Threading.Timer(async _ => await CheckForUpdatesInBackground(),
                    null, intervalMs, intervalMs);
            }
        }

        private async Task CheckForUpdatesInBackground()
        {
            if (updateManager != null)
            {
                await updateManager.CheckForUpdatesInBackground();
                SaveSettings();
            }
        }

        private async Task CheckForUpdatesManually()
        {
            if (updateManager != null)
            {
                var result = await updateManager.CheckForUpdatesAsync(showNoUpdateMessage: true);
                updateManager.ShowUpdateDialog(result);
                SaveSettings();
            }
        }

        public void ShowUpdateDialogOnMainThread(UpdateResult result)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<UpdateResult>(ShowUpdateDialogOnMainThread), result);
                return;
            }

            if (updateManager != null)
            {
                updateManager.ShowUpdateDialog(result);
            }
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

        private void ShowColorSelector()
        {
            using (ColorSelectorForm colorSelectorForm = new ColorSelectorForm())
            {
                colorSelectorForm.ColorSelected += OnColorSelected;
                colorSelectorForm.ShowDialog();
            }
        }

        private void OnColorSelected(Color color)
        {
            // Handle auto-copy functionality
            if (settings.AutoCopyEnabled)
            {
                string colorString = ColorUtilities.ColorToString(color, settings.AutoCopyFormat);
                bool copySuccess = ColorUtilities.CopyToClipboard(colorString);

                if (copySuccess && settings.ShowCopyNotification)
                {
                    string formatName = ColorUtilities.GetFormatDisplayName(settings.AutoCopyFormat);
                    ColorUtilities.ShowNotification($"Copied to clipboard: {colorString}\n({formatName})");
                }
                else if (!copySuccess && settings.ShowCopyNotification)
                {
                    ColorUtilities.ShowNotification("Failed to copy color to clipboard");
                }
            }

            // Show the color result form only if enabled
            if (settings.ShowPopupOnPick)
            {
                using (ColorResultForm resultForm = new ColorResultForm(color, settings))
                {
                    resultForm.Show();
                }
            }
        }

        private void ShowSettings()
        {
            using (SettingsForm settingsForm = new SettingsForm(settings))
            {
                if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    SaveSettings();
                    // Restart hotkeys with new settings
                    globalHotkey?.Dispose();
                    InitializeGlobalHotkey();

                    // Reinitialize update manager and timer with new settings
                    updateManager = new UpdateManager(settings, this);
                    SetupUpdateTimer();
                }
            }
        }

        private void ExitApplication()
        {
            if (trayIcon != null)
            {
                trayIcon.Visible = false;
                trayIcon.Dispose();
            }
            globalHotkey?.Dispose();
            updateTimer?.Dispose();
            Application.Exit();
        }

        protected override void SetVisibleCore(bool value)
        {
            // Keep main form hidden to run as a tray application
            base.SetVisibleCore(false);
        }
    }

}
