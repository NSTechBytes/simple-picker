using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace simple_picker
{
    /// <summary>
    /// Manages application update checking, version comparison, and update dialog display.
    /// </summary>
    public class UpdateManager
    {
        private readonly Settings settings;
        private readonly MainForm? mainForm;
        private static readonly HttpClient httpClient = new HttpClient();

        public UpdateManager(Settings settings, MainForm? mainForm = null)
        {
            this.settings = settings;
            this.mainForm = mainForm;
        }

        public async Task<UpdateResult> CheckForUpdatesAsync(bool showNoUpdateMessage = false)
        {
            try
            {
                string response = await httpClient.GetStringAsync(settings.UpdateUrl);

                // Parse version from INI format
                var match = Regex.Match(response, @"version\s*=\s*[""']?([^""'\r\n]+)[""']?", RegexOptions.IgnoreCase);
                if (!match.Success)
                {
                    return new UpdateResult
                    {
                        Success = false,
                        ErrorMessage = "Unable to parse version information from remote server. Response: " + response.Substring(0, Math.Min(100, response.Length))
                    };
                }

                string latestVersion = match.Groups[1].Value.Trim();

                // Get current version from assembly metadata (via settings)
                string currentVersionString = settings.CurrentVersion;

                System.Diagnostics.Debug.WriteLine($"Update Check - Current: '{currentVersionString}', Latest: '{latestVersion}'");

                try
                {
                    string normalizedCurrent = NormalizeVersion(currentVersionString);
                    string normalizedLatest = NormalizeVersion(latestVersion);

                    Version current = new Version(normalizedCurrent);
                    Version latest = new Version(normalizedLatest);

                    System.Diagnostics.Debug.WriteLine($"Normalized versions - Current: {current}, Latest: {latest}");

                    settings.LastUpdateCheck = DateTime.Now;

                    bool updateAvailable = latest > current;
                    System.Diagnostics.Debug.WriteLine($"Update available: {updateAvailable}");

                    if (updateAvailable)
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

        private string NormalizeVersion(string version)
        {
            if (string.IsNullOrWhiteSpace(version))
                return "1.0";

            string cleanVersion = Regex.Replace(version, @"[^\d\.]", "");
            string[] parts = cleanVersion.Split('.');

            if (parts.Length == 1)
                return parts[0] + ".0";

            return string.Join(".", parts, 0, Math.Min(parts.Length, 4));
        }

        public bool ShouldCheckForUpdates()
        {
            if (!settings.AutoCheckForUpdates)
                return false;

            if (settings.LastUpdateCheck == DateTime.MinValue)
                return true;

            bool shouldCheck = DateTime.Now.Subtract(settings.LastUpdateCheck).TotalSeconds >= settings.UpdateCheckIntervalSeconds;
            System.Diagnostics.Debug.WriteLine($"Should check for updates: {shouldCheck} (Last check: {settings.LastUpdateCheck}, Interval: {settings.UpdateCheckIntervalSeconds}s)");
            return shouldCheck;
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
                System.Diagnostics.Debug.WriteLine("Showing update dialog");

                string appName = settings.AppName;

                DialogResult dialogResult = MessageBox.Show(
                    $"A new version of {appName} is available!\n\n" +
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
                            FileName = "https://simple-picker.pages.dev/",
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Unable to open browser: {ex.Message}",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                settings.UpdateDialogShownThisSession = true;
            }
            else if (result.ShowNoUpdateMessage)
            {
                string appName = settings.AppName;
                MessageBox.Show($"You are using the latest version of {appName} ({result.CurrentVersion}).",
                    "No Updates Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public async Task CheckForUpdatesInBackground()
        {
            System.Diagnostics.Debug.WriteLine("CheckForUpdatesInBackground called");

            if (!ShouldCheckForUpdates())
            {
                System.Diagnostics.Debug.WriteLine("Should not check for updates, skipping");
                return;
            }

            System.Diagnostics.Debug.WriteLine("Checking for updates in background...");
            var result = await CheckForUpdatesAsync();

            System.Diagnostics.Debug.WriteLine($"Update check result - Success: {result.Success}, UpdateAvailable: {result.UpdateAvailable}, DialogShownThisSession: {settings.UpdateDialogShownThisSession}");

            if (result.Success && result.UpdateAvailable && !settings.UpdateDialogShownThisSession)
            {
                System.Diagnostics.Debug.WriteLine("Attempting to show update dialog");

                if (mainForm != null)
                {
                    mainForm.ShowUpdateDialogOnMainThread(result);
                }
                else
                {
                    if (Application.OpenForms.Count > 0)
                    {
                        foreach (Form form in Application.OpenForms)
                        {
                            try
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
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"Error showing update dialog: {ex.Message}");
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            ShowUpdateDialog(result);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error showing update dialog (direct): {ex.Message}");
                        }
                    }
                }
            }
            else
            {
                if (!result.Success)
                    System.Diagnostics.Debug.WriteLine($"Update check failed: {result.ErrorMessage}");
                else if (!result.UpdateAvailable)
                    System.Diagnostics.Debug.WriteLine("No update available");
                else if (settings.UpdateDialogShownThisSession)
                    System.Diagnostics.Debug.WriteLine("Update dialog already shown this session");
            }
        }

        public async Task<UpdateResult> ForceCheckForUpdatesAsync(bool showNoUpdateMessage = true)
        {
            System.Diagnostics.Debug.WriteLine("Force checking for updates...");
            var result = await CheckForUpdatesAsync(showNoUpdateMessage);

            if (result.Success)
            {
                if (result.UpdateAvailable || showNoUpdateMessage)
                {
                    ShowUpdateDialog(result);
                }
            }

            return result;
        }

        public void ResetSessionState()
        {
            settings.UpdateDialogShownThisSession = false;
            System.Diagnostics.Debug.WriteLine("Update session state reset");
        }

        public string GetVersionInfo()
        {
            return $"App: {settings.AppName}\nVersion: {settings.CurrentVersion}\nSource: Assembly Metadata (.csproj Version)\n\nUpdate Settings:\nAuto-check: {settings.AutoCheckForUpdates}\nInterval: {settings.UpdateCheckIntervalSeconds}s\nLast check: {settings.LastUpdateCheck}\nDialog shown this session: {settings.UpdateDialogShownThisSession}";
        }
    }
}
