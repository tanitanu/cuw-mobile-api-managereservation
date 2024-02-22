using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBMPAccountValidation
    {
        private string mileagePlusNumber = string.Empty;
        private long customerId;
        private bool validPinCode;
        private bool deceasedAccount;
        private bool closedAccount;
        private bool needToAcceptTNC;
        private bool accountLocked;
        private string hashValue;
        private string message;
        private string authenticatedToken;
        private bool isTokenValid;
        private DateTime tokenExpireDateTime;
        private Double tokenExpirationSeconds;

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

        [XmlIgnore]
        public long CustomerId
        {
            get
            {
                return this.customerId;
            }
            set
            {
                this.customerId = value;
            }
        }

        public bool ValidPinCode
        {
            get
            {
                return this.validPinCode;
            }
            set
            {
                this.validPinCode = value;
            }
        }

        public bool DeceasedAccount
        {
            get
            {
                return this.deceasedAccount;
            }
            set
            {
                this.deceasedAccount = value;
            }
        }

        public bool ClosedAccount
        {
            get
            {
                return this.closedAccount;
            }
            set
            {
                this.closedAccount = value;
            }
        }

        public bool NeedToAcceptTNC
        {
            get
            {
                return this.needToAcceptTNC;
            }
            set
            {
                this.needToAcceptTNC = value;
            }
        }

        public bool AccountLocked
        {
            get
            {
                return this.accountLocked;
            }
            set
            {
                this.accountLocked = value;
            }
        }

        public string HashValue
        {
            get
            {
                return this.hashValue;
            }
            set
            {
                this.hashValue = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string Message
        {
            get
            {
                return this.message;
            }
            set
            {
                this.message = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string AuthenticatedToken
        {
            get
            {
                return this.authenticatedToken;
            }
            set
            {
                this.authenticatedToken = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool IsTokenValid
        {
            get
            {
                return this.isTokenValid;
            }
            set
            {
                this.isTokenValid = value;
            }
        }

        public DateTime TokenExpireDateTime
        {
            get
            {
                return this.tokenExpireDateTime;
            }
            set
            {
                this.tokenExpireDateTime = value;
            }
        }
        public Double TokenExpirationSeconds
        {
            get
            {
                return this.tokenExpirationSeconds;
            }
            set
            {
                this.tokenExpirationSeconds = value;
            }
        }
    }
}
