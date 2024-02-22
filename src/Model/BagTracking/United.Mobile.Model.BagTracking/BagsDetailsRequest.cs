using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.BagTracking
{
    [Serializable]
    public class BagsDetailsRequest : MOBRequest
    {
        private string recordLocator = string.Empty;
        public string MileagePlusAccountNumber { get; set; } = string.Empty;
        private string bagTagId { get; set; } = string.Empty;
        public string LastNames { get; set; } = string.Empty;
        private string deeplinkUrlPath { get; set; } =  string.Empty;
        public bool LogAll { get; set; }

        public string RecordLocator
        {
            get
            {
                return recordLocator;
            }
            set
            {
                this.recordLocator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string DeeplinkUrlPath
        {
            get
            {
                return deeplinkUrlPath;
            }
            set
            {
                this.deeplinkUrlPath = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string BagTagId
        {
            get
            {
                return this.bagTagId;
            }
            set
            {
                this.bagTagId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }

    [Serializable]
    public class GetBagsForPNRsRequest : MOBRequest
    {
        public List<PNRListForBags> PNRList { get; set; }
        public string MileagePlusAccountNumber { get; set; } = string.Empty;
        public GetBagsForPNRsRequest()
        {
            PNRList = new List<PNRListForBags>();
        }

    }

    [Serializable]
    public class PNRListForBags
    {
        public string RecordLocator { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

}
