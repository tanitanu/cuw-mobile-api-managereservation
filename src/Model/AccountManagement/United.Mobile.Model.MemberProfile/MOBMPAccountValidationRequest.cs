using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBMPAccountValidationRequest : MOBRequest
    {
        private string mileagePlusNumber = string.Empty;
        private string pinCode = string.Empty;
        private string sessionId = string.Empty;
        private string hashValue;

        public MOBMPAccountValidationRequest()
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

        public string PinCode
        {
            get
            {
                return this.pinCode;
            }
            set
            {
                this.pinCode = string.IsNullOrEmpty(value) ? string.Empty : value;
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
    }
}
