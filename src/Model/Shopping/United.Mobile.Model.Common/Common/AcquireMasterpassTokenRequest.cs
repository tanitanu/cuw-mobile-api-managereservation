using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class AcquireMasterpassTokenRequest  : MOBRequest
    {

        private string sessionId = string.Empty;
        private double amount = 0.0;
        private string mileagePlusNumber;
        private string originCallingURL;
        private string originURL;
        private string pointOfSale;

        public string SessionId
        {
            get { return sessionId; }
            set { this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public double Amount
        {
            get { return amount; }
            set { this.amount = value; }
        }

        public string MileagePlusNumber
        {
            get { return mileagePlusNumber; }
            set { mileagePlusNumber = value; }
        }

        public string OriginCallingURL
        {
            get
            {
                return originCallingURL;
            }
            set
            {
                this.originCallingURL = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string OriginURL
        {
            get
            {
                return originURL;
            }
            set
            {
                this.originURL = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

    }

}

