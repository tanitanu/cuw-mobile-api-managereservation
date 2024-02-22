using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Common;
using Newtonsoft.Json;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBMPPINPWDValidateRequest : MOBRequest
    {
        [JsonIgnore()]
        public string ObjectName { get; set; } = "United.Definition.MPPINPWD.MOBMPPINPWDValidateRequest";
        private string mileagePlusNumber = string.Empty;
        private int customerID;
        private string passWord = string.Empty;
        private string sessionID = string.Empty;
        private bool signInWithTouchID;
        private MOBMPSignInPath mpSignInPath;
        private string hashValue = string.Empty;
        private bool rememberDevice;

        public MOBMPPINPWDValidateRequest()
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

        public string PassWord
        {
            get
            {
                return this.passWord;
            }
            set
            {
                this.passWord = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
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

        public MOBMPSignInPath MPSignInPath
        {
            get
            {
                return this.mpSignInPath;
            }
            set
            {
                this.mpSignInPath = value;
            }
        }

        public bool SignInWithTouchID
        {
            get { return this.signInWithTouchID; }
            set { this.signInWithTouchID = value; }
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

        public bool RememberDevice
        {
            get { return rememberDevice; }
            set { rememberDevice = value; }
        }
    }
}
