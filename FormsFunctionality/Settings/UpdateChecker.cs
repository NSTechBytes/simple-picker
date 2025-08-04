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

                // Parse version from INI format - improved regex
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

                // Get current version from registry
                string currentVersionString = settings.CurrentVersion;

                // Debug logging
                System.Diagnostics.Debug.WriteLine($"Update Check - Current: '{currentVersionString}', Latest: '{latestVersion}'");

                try
                {
                    // Normalize version strings by ensuring they have at least 2 parts (major.minor)
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

        /// <summary>
        /// Normalizes version string to ensure it's compatible with System.Version
        /// Ensures at least major.minor format (e.g., "1" becomes "1.0")
        /// </summary>
        private string NormalizeVersion(string version)
        {
            if (string.IsNullOrWhiteSpace(version))
                return "1.0";

            // Remove any non-numeric characters except dots
            string cleanVersion = Regex.Replace(version, @"[^\d\.]", "");

            // Split by dots and ensure we have at least 2 parts
            string[] parts = cleanVersion.Split('.');

            // If we only have one part (like "1"), make it "1.0"
            if (parts.Length == 1)
            {
                return parts[0] + ".0";
            }

            // If we have 2 or more parts, take first 4 (major.minor.build.revision max for System.Version)
            return string.Join(".", parts, 0, Math.Min(parts.Length, 4));
        }

        public bool ShouldCheckForUpdates()
        {
            if (!settings.AutoCheckForUpdates)
                return false;

            if (settings.LastUpdateCheck == DateTime.MinValue)
                return true;

            // Changed from days to seconds
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

                // Mark that dialog has been shown this session AFTER user interaction
                settings.UpdateDialogShownThisSession = true;
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
            System.Diagnostics.Debug.WriteLine("CheckForUpdatesInBackground called");

            if (!ShouldCheckForUpdates())
            {
                System.Diagnostics.Debug.WriteLine("Should not check for updates, skipping");
                return;
            }

            System.Diagnostics.Debug.WriteLine("Checking for updates in background...");
            var result = await CheckForUpdatesAsync();

            System.Diagnostics.Debug.WriteLine($"Update check result - Success: {result.Success}, UpdateAvailable: {result.UpdateAvailable}, DialogShownThisSession: {settings.UpdateDialogShownThisSession}");

            // Only show dialog if update is available AND dialog hasn't been shown this session
            if (result.Success && result.UpdateAvailable && !settings.UpdateDialogShownThisSession)
            {
                System.Diagnostics.Debug.WriteLine("Attempting to show update dialog");

                // Use MainForm reference to show dialog on main thread
                if (mainForm != null)
                {
                    mainForm.ShowUpdateDialogOnMainThread(result);
                }
                else
                {
                    // Fallback: Show dialog on UI thread
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
                        // Last resort: show directly (may not be on UI thread)
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

        /// <summary>
        /// Force check for updates regardless of session state (useful for manual checks)
        /// </summary>
        public async Task<UpdateResult> ForceCheckForUpdatesAsync(bool showNoUpdateMessage = true)
        {
            System.Diagnostics.Debug.WriteLine("Force checking for updates...");
            var result = await CheckForUpdatesAsync(showNoUpdateMessage);

            if (result.Success)
            {
                // Always show dialog for manual checks, regardless of session state
                if (result.UpdateAvailable || showNoUpdateMessage)
                {
                    ShowUpdateDialog(result);
                }
            }

            return result;
        }

        /// <summary>
        /// Reset the session state to allow showing update dialog again
        /// </summary>
        public void ResetSessionState()
        {
            settings.UpdateDialogShownThisSession = false;
            System.Diagnostics.Debug.WriteLine("Update session state reset");
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
            return $"App: {appName}\nVersion: {version}\nPublisher: {publisher}\nSource: Registry (HKEY_CURRENT_USER\\Software\\SimplePicker)\n\nUpdate Settings:\nAuto-check: {settings.AutoCheckForUpdates}\nInterval: {settings.UpdateCheckIntervalSeconds}s\nLast check: {settings.LastUpdateCheck}\nDialog shown this session: {settings.UpdateDialogShownThisSession}";
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