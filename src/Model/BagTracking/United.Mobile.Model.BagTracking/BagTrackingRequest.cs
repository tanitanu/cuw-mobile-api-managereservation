namespace United.Mobile.Model.BagTracking
{
    public class BagTrackingRequest
    {
        //[Required]
        public string DeviceId { get; set; }
        public int ApplicationId { get; set; }
        public string AppVersion { get; set; }
        public string AccessCode { get; set; }
        public string transactionId { get; set; }
        public string BagTagNumber { get; set; }
        public string RecordLocator { get; set; }
        public string LastName { get; set; }
        public string mileagePlusAccountNumber { get; set; }
        public string LanguageCode { get; set; }
        public string SessionId { get; set; } = string.Empty;
    }
}
