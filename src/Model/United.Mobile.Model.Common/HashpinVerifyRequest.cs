namespace United.Mobile.Model.Common
{
    public class HashpinVerifyRequest
    {
        public string HashValue { get; set; }
        public string MpNumber { get; set; }
        public string ServiceName { get; set; }
        public string SessionID { get; set; }
        public string TransactionId { get; set; } = string.Empty;
        public int ApplicationId { get; set; }
        public string AppVersion { get; set; }
        public string DeviceId { get; set; } = string.Empty;
    }
}
