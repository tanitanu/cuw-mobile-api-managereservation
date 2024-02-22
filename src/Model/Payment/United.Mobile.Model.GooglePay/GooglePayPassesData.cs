namespace United.Mobile.Model.GooglePay
{
    public class GooglePayPassesData
    {
        public string ConfirmationNumber { get; set; } = string.Empty;
        public string ClassId { get; set; } = string.Empty;
        public string ObjectId { get; set; } = string.Empty;
        public string FilterKey { get; set; } = string.Empty;
        public string DeleteKey { get; set; } = string.Empty;
        public int ApplicationId { get; set; }
        public string AppVersion { get; set; } = string.Empty;
        public string DeviceId { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;
    }
}
