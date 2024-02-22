using System;

namespace United.Mobile.Model.BagTracking
{
    [Serializable]
    public class BagTag
    {
        private string issueDateTime = string.Empty;

        public string BagTagNumber { get; set; } = string.Empty;
        private string departureAirport { get; set; } = string.Empty;
        public string FinalArrivalAirport { get; set; } = string.Empty;
        private string boltFileReferenceNumber { get; set; } = string.Empty;
        public string BoltFileReferenceNumber
        {
            get
            {
                return boltFileReferenceNumber;
            }
            set
            {
                this.boltFileReferenceNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string IssueDateTime
        {
            get
            {
                return issueDateTime;
            }
            set
            {
                this.issueDateTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string DepartureAirport
        {
            get
            {
                return departureAirport;
            }
            set
            {
                this.departureAirport = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string IssueStationCode { get; set; } = string.Empty;
        public string TypeCode { get; set; } = string.Empty;
        public string UniqueKeyNumber { get; set; } = string.Empty;
        public bool ClaimIndicator { get; set; }
        public string ClaimUrl { get; set; } = string.Empty;
        public string DeliveryCompany { get; set; } = string.Empty;

    }
}
