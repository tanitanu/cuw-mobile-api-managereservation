using System;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBMPPlusPointsRequest : MOBRequest
    {
        private string mileagePlusNumber = string.Empty;
        private string sessionId = string.Empty;
        private string hashValue = string.Empty;
        private string flow;

        public MOBMPPlusPointsRequest()
            : base()
        {
        }

        public string MileagePlusNumber
        {
            get
            {
                return this.mileagePlusNumber;
            }
            set
            {
                this.mileagePlusNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string SessionId
        {
            get
            {
                return this.sessionId;
            }
            set
            {
                this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }

        public string HashValue
        {
            get { return hashValue; }
            set { hashValue = value; }
        }
        public string Flow
        {
            get { return flow; }
            set { flow = value; }
        }
       
    }
}
