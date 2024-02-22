using Microsoft.Extensions.Configuration;
using System;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBMPSecurityUpdateResponse : MOBResponse
    {
        private readonly IConfiguration _configuration;
        public MOBMPSecurityUpdateResponse()
            : base()
        {
        }
        private string mileagePlusNumber = string.Empty;
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

        private int customerID;
        public int CustomerID
        {
            get
            {
                return this.customerID;
            }
            set
            {
                this.customerID = value;
            }
        }

        private MOBMPSecurityUpdateRequest request;
        public MOBMPSecurityUpdateRequest Request
        {
            get
            {
                return this.request;
            }
            set
            {
                this.request = value;
            }
        }

        private bool securityUpdate;
        public bool SecurityUpdate
        {
            get { return this.securityUpdate; }
            set { this.securityUpdate = value; }
        }

        private MOBMPPINPWDSecurityUpdateDetails mpSecurityUpdateDetails;
        public MOBMPPINPWDSecurityUpdateDetails MPSecurityUpdateDetails
        {
            get
            {
                return this.mpSecurityUpdateDetails;
            }
            set
            {
                this.mpSecurityUpdateDetails = value;
            }
        }

        private string sessionID = string.Empty;
        public string SessionID
        {
            get
            {
                return this.sessionID;
            }
            set
            {
                this.sessionID = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }

        private string securityUpdateCompleteMessage;
        public string SecurityUpdateCompleteMessage
        {
            get
            {
                return this.securityUpdateCompleteMessage;
            }
            set
            {
                this.securityUpdateCompleteMessage = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }

        private string hashValue = string.Empty;

        public string HashValue
        {
            get { return hashValue; }
            set { hashValue = value; }
        }

        private MOBMPTFARememberMeFlags rememberMEFlags;
        public MOBMPTFARememberMeFlags RememberMEFlags
        {
            get
            {
                if (rememberMEFlags == null)
                {
                    rememberMEFlags = new MOBMPTFARememberMeFlags();
                }
                return this.rememberMEFlags;
            }
            set
            {
                this.rememberMEFlags = value;
            }
        }
        private bool showContinueAsGuestButton;
        public bool ShowContinueAsGuestButton
        {
            get
            {
                return showContinueAsGuestButton;
            }
            set { showContinueAsGuestButton = value; }
        }
    }
}
