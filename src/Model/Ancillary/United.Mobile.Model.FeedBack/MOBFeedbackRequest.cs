using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.FeedBack
{
    [Serializable()]
    public class MOBFeedbackRequest : MOBRequest
    {
        public string SessionId { get; set; } = string.Empty;

        public string FeedType { get; set; } = string.Empty;

        public string PageSize { get; set; } = string.Empty;

        public string RequestedPage { get; set; } = string.Empty;

        public double StarRating { get; set; }

        public string Category { get; set; } = string.Empty;

        public string TaskAnswer { get; set; } = string.Empty;

        public string MileagePlusAccountNumber { get; set; } = string.Empty;

        public string DeviceModel { get; set; } = string.Empty;

        public string DeviceOSVersion { get; set; } = string.Empty;

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Pnrs { get; set; } = string.Empty;

        public string Answer1 { get; set; } = string.Empty;

        public string Answer2 { get; set; } = string.Empty;
    }
}
