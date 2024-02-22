using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.BagTracking
{
    public class BaggageDeliveryResponse : MOBResponse
    {
        private List<BaggageDeliveryDetails> baggageDeliveryDetails { get; set; }
        public List<BaggageDeliveryDetails> BaggageDeliveryDetails
        {
            get
            {
                return baggageDeliveryDetails;
            }
            set
            {
                this.baggageDeliveryDetails = value;
            }
        }
    }
    public class BaggageDeliveryDetails
    {

        private string boltRefNumber { get; set; } = string.Empty;
        public string BoltRefNumber
        {
            get
            {
                return boltRefNumber;
            }
            set
            {
                this.boltRefNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string wTBoltRefNumber { get; set; } = string.Empty;
        public string WTBoltRefNumber
        {
            get
            {
                return wTBoltRefNumber;
            }
            set
            {
                this.wTBoltRefNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private bool statusCode { get; set; }
        public bool StatusCode
        {
            get
            {
                return statusCode;
            }
            set
            {
                this.statusCode = value;
            }
        }
        private string statusDescription { get; set; } = string.Empty;
        public string StatusDescription
        {
            get
            {
                return statusDescription;
            }
            set
            {
                this.statusDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private int? processCode { get; set; }
        public int? ProcessCode
        {
            get
            {
                return processCode;
            }
            set
            {
                this.processCode = value;
            }
        }
        private string processDescription { get; set; } = string.Empty;
        public string ProcessDescription
        {
            get
            {
                return processDescription;
            }
            set
            {
                this.processDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
