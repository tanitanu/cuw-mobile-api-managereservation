namespace United.Mobile.Model.Internal.Common
{
    public class Payload
    {
        public string recordLocator { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string jsonPayLoad { get; set; } = string.Empty;
        public int applicationId { get; set; }
        public string deviceId { get; set; } = string.Empty;
        public string appVersion { get; set; } = string.Empty;
        public string guid { get; set; } = string.Empty;
    }
}
