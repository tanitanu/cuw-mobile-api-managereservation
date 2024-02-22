namespace United.Mobile.Model.Internal.Common
{
    public class Feedback
    {
        public int applicationId { get; set; }
        public string applicationVersion { get; set; } = string.Empty;
        public string mileagePlusAccountNumber { get; set; } = string.Empty;
        public string text { get; set; } = string.Empty;
        public double starRating { get; set; }
        public string category { get; set; } = string.Empty;
        public string taskAnswer { get; set; } = string.Empty;
        public string deviceModel { get; set; } = string.Empty;
        public string deviceOSVersion { get; set; } = string.Empty;
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string pnrs { get; set; } = string.Empty;
        public string answer1 { get; set; } = string.Empty;
        public string answer2 { get; set; } = string.Empty;
        public string opinionLabRequest { get; set; } = string.Empty;
        public string opinionLabResponse { get; set; } = string.Empty;
    }
}
