namespace United.Mobile.Model.FeedBack
{
    public class BetaRequestData
    {
        public string MileagePlusAccountNumber { get; set; } = string.Empty;
        public int FeatureId { get; set; }
        public int ApplicationId { get; set; }
        public string ApplicationVersion { get; set; } = string.Empty;
    }
}
