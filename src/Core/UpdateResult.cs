namespace simple_picker
{
    /// <summary>
    /// Represents the result of an update check operation.
    /// </summary>
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
