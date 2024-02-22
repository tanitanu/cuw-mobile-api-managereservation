using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.BagTracking
{
    [Serializable]
    public class BagsDetailsResponse
    {
        public List<BagsDetails> BagsDetails { get; set; }
        public string LastUpdatedTimeGMT { get; set; } = string.Empty;

        public BagsDetailsResponse()
        {
            BagsDetails = new List<BagsDetails>();
        }

    }

    [Serializable]
    public class BagTrackingDetailsResponse : MOBResponse
    {
        public BagsDetailsRequest BagTrackingDetailsRequest { get; set; }
        public List<BagsDetails> BagsDetails { get; set; }
        public string LastUpdatedTimeGMT { get; set; } = string.Empty;
        public string ClaimIndicator { get; set; } = string.Empty;
        public string ClaimButtonText { get; set; } = string.Empty;
        public string ClaimBagUrl { get; set; } = string.Empty;
        public BaggageDelivery baggageDelivery { get; set; }
        public BagTrackingDetailsResponse()
        {
            BagsDetails = new List<BagsDetails>();
        }
    }

    [Serializable]
    public class GetBagForPNRsResponse
    {
        public GetBagsForPNRsRequest GetBagsForPNRsRequest { get; set; }
        public List<BagsDetails> BagsDetails { get; set; }
        public string LastUpdatedTimeGMT { get; set; } = string.Empty;        

        public GetBagForPNRsResponse()
        {
            BagsDetails = new List<BagsDetails>();
        }
    }
    [Serializable]
    public class BaggageDelivery
    {
        private List<MOBItem> content;        
        public List<MOBItem> Content
        {
            get { return content; }
            set { content = value; }
        }
        private string bagChecked { get; set; } = string.Empty;
        public string BagChecked
        {
            get
            {
                return bagChecked;
            }
            set
            {
                this.bagChecked = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        private string bagReceived { get; set; } = string.Empty;
        public string BagReceived
        {
            get
            {
                return bagReceived;
            }
            set
            {
                this.bagReceived = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        private string lastSeen { get; set; } = string.Empty;
        public string LastSeen
        {
            get
            {
                return lastSeen;
            }
            set
            {
                this.lastSeen = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        private string agent { get; set; } = string.Empty;
        public string Agent
        {
            get
            {
                return agent;
            }
            set
            {
                this.agent = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        private string fileType { get; set; } = string.Empty;
        public string FileType
        {
            get
            {
                return fileType;
            }
            set
            {
                this.fileType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        private string fileStatus { get; set; } = string.Empty;
        public string FileStatus
        {
            get
            {
                return fileStatus;
            }
            set
            {
                this.fileStatus = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        private string createDate { get; set; } = string.Empty;
        public string CreateDate
        {
            get
            {
                return createDate;
            }
            set
            {
                this.createDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        private int noOfCustomers { get; set; }
        public int NoOfCustomers
        {
            get
            {
                return noOfCustomers;
            }
            set
            {
                this.noOfCustomers = value;
            }
        }

    }
}
