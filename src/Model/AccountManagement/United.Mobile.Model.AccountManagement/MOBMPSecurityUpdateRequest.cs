using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBMPSecurityUpdateRequest : MOBRequest
    {

        private string sessionID = string.Empty;
        private string mileagePlusNumber = string.Empty;
        private int customerID;
        private string hashValue = string.Empty;
        private MOBMPSecurityUpdatePath securityUpdateType;
        private MOBMPPINPWDSecurityItems securityItemsToUpdate;
        private bool rememberDevice;

        public MOBMPSecurityUpdateRequest()
            : base()
        {
        }
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

        public string HashValue
        {
            get
            {
                return this.hashValue;
            }
            set
            {
                this.hashValue = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }

        public MOBMPSecurityUpdatePath SecurityUpdateType
        {
            get
            {
                return this.securityUpdateType;
            }
            set
            {
                this.securityUpdateType = value;
            }
        }

        public MOBMPPINPWDSecurityItems SecurityItemsToUpdate
        {
            get { return securityItemsToUpdate; }
            set { securityItemsToUpdate = value; }
        }

        public bool RememberDevice
        {
            get { return rememberDevice; }
            set { rememberDevice = value; }
        }
    }
}
