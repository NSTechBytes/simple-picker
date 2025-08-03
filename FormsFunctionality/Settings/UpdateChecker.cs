using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace simple_picker
{
    public class UpdateManager
    {
        private readonly Settings settings;
        private readonly MainForm? mainForm; // Reference to MainForm for showing dialogs
        private static readonly HttpClient httpClient = new HttpClient();

        public UpdateManager(Settings settings, MainForm? mainForm = null)
        {
            this.settings = settings;
            this.mainForm = mainForm;
            
            // Initialize registry if needed
            settings.InitializeRegistryIfNeeded();
        }

        public async Task<UpdateResult> CheckForUpdatesAsync(bool showNoUpdateMessage = false)
        {
            try
            {
                string response = await httpClient.GetStringAsync(settings.UpdateUrl);
                
                // Parse version from INI format
                var match = Regex.Match(response, @"version\s*=\s*(.+)", RegexOptions.IgnoreCase);
                if (!match.Success)
                {
                    return new UpdateResult 
                    { 
                        Success = false, 
                        ErrorMessage = "Unable to parse version information from remote server" 
                    };
                }

                string latestVersion = match.Groups[1].Value.Trim();
                
                // Get current version from registry
                string currentVersionString = settings.CurrentVersion;
                
                try 
                {
                    Version current = new Version(currentVersionString);
                    Version latest = new Version(latestVersion);

                    settings.LastUpdateCheck = DateTime.Now;

                    if (latest > current)
                    {
                        return new UpdateResult
                        {
                            Success = true,
                            UpdateAvailable = true,
                            LatestVersion = latestVersion,
                            CurrentVersion = currentVersionString
                        };
                    }
                    else
                    {
                        return new UpdateResult
                        {
                            Success = true,
                            UpdateAvailable = false,
                            LatestVersion = latestVersion,
                            CurrentVersion = currentVersionString,
                            ShowNoUpdateMessage = showNoUpdateMessage
                        };
                    }
                }
                catch (Exception versionParseEx)
                {
                    return new UpdateResult
                    {
                        Success = false,
                        ErrorMessage = $"Version parsing error - Current: '{currentVersionString}', Latest: '{latestVersion}'. Error: {versionParseEx.Message}"
                    };
                }
            }
            catch (HttpRequestException httpEx)
            {
                return new UpdateResult
                {
                    Success = false,
                    ErrorMessage = $"Network error while checking for updates: {httpEx.Message}"
                };
            }
            catch (Exception ex)
            {
                return new UpdateResult
                {
                    Success = false,
                    ErrorMessage = $"Update check failed: {ex.Message}"
                };
            }
        }

        public bool ShouldCheckForUpdates()
        {
            if (!settings.AutoCheckForUpdates)
                return false;

            if (settings.LastUpdateCheck == DateTime.MinValue)
                return true;

            // Changed from days to seconds
            return DateTime.Now.Subtract(settings.LastUpdateCheck).TotalSeconds >= settings.UpdateCheckIntervalSeconds;
        }

        public void ShowUpdateDialog(UpdateResult result)
        {
            if (!result.Success)
            {
                MessageBox.Show($"Update check failed: {result.ErrorMessage}", 
                    "Update Check Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (result.UpdateAvailable)
            {
                // Mark that dialog has been shown this session
                settings.UpdateDialogShownThisSession = true;
                
                // Get app info from registry for display
                var (appName, currentVersion, publisher) = settings.GetAppInfoFromRegistry();
                
                DialogResult dialogResult = MessageBox.Show(
                    $"A new version of {appName} is available!\n\n" +
                    $"Publisher: {publisher}\n" +
                    $"Current Version: {result.CurrentVersion}\n" +
                    $"Latest Version: {result.LatestVersion}\n\n" +
                    $"Would you like to visit the download page?",
                    "Update Available",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = "https://github.com/NSTechBytes/simple-picker/releases",
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Unable to open browser: {ex.Message}", 
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (result.ShowNoUpdateMessage)
            {
                var (appName, _, _) = settings.GetAppInfoFromRegistry();
                MessageBox.Show($"You are using the latest version of {appName} ({result.CurrentVersion}).", 
                    "No Updates Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public async Task CheckForUpdatesInBackground()
        {
            if (!ShouldCheckForUpdates())
                return;

            var result = await CheckForUpdatesAsync();
            
            // Only show dialog if update is available AND dialog hasn't been shown this session
            if (result.Success && result.UpdateAvailable && !settings.UpdateDialogShownThisSession)
            {
                // Use MainForm reference to show dialog on main thread
                if (mainForm != null)
                {
                    mainForm.ShowUpdateDialogOnMainThread(result);
                }
                else
                {
                    // Fallback method: Create a temporary invisible form to invoke on UI thread
                    try
                    {
                        var tempForm = new Form { WindowState = FormWindowState.Minimized, ShowInTaskbar = false };
                        tempForm.Load += (s, e) =>
                        {
                            tempForm.Hide();
                            ShowUpdateDialog(result);
                            tempForm.Close();
                        };
                        tempForm.Show();
                    }
                    catch
                    {
                        // Last resort: show on any available form
                        if (Application.OpenForms.Count > 0)
                        {
                            foreach (Form form in Application.OpenForms)
                            {
                                if (form.InvokeRequired)
                                {
                                    form.Invoke(new Action(() => ShowUpdateDialog(result)));
                                }
                                else
                                {
                                    ShowUpdateDialog(result);
                                }
                                break;
                            }
                        }
                        else
                        {
                            // If no forms available, show directly (may not be on UI thread)
                            ShowUpdateDialog(result);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Updates the version in registry (useful after successful update installation)
        /// </summary>
        /// <param name="newVersion">New version to set</param>
        /// <returns>True if successful</returns>
        public bool UpdateRegistryVersion(string newVersion)
        {
            return settings.SetVersionInRegistry(newVersion);
        }

        /// <summary>
        /// Gets detailed version information for debugging
        /// </summary>
        /// <returns>String containing version details</returns>
        public string GetVersionInfo()
        {
            var (appName, version, publisher) = settings.GetAppInfoFromRegistry();
            return $"App: {appName}\nVersion: {version}\nPublisher: {publisher}\nSource: Registry (HKEY_CURRENT_USER\\Software\\SimplePicker)";
        }
    }

    public class UpdateResult
    {
        public bool Success { get; set; }
        public bool UpdateAvailable { get; set; }
        public string CurrentVersion { get; set; } = string.Empty;
        public string LatestVersion { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public bool ShowNoUpdateMessage { get; set; } = false;
    }
}