using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.FareLock
{
    [Serializable()]
    public class MOBSHOPRegisterFareLockRequest : MOBRequest
    {
        private string sessionId = string.Empty;
        private string cartId = string.Empty;
        private string countryCode = string.Empty;
        private bool autoTicket = false;
        private string fareLockProductCode;

        public string SessionId
        {
            get
            {
                return sessionId;
            }
            set
            {
                this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string CartId
        {
            get
            {
                return this.cartId;
            }
            set
            {
                this.cartId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string CountryCode
        {
            get
            {
                return this.countryCode;
            }
            set
            {
                this.countryCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool AutoTicket
        {
            get
            {
                return this.autoTicket;
            }
            set
            {
                this.autoTicket = value;
            }
        }

        public string FareLockProductCode
        {
            get
            {
                return fareLockProductCode;
            }
            set
            {
                this.fareLockProductCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

    }
}
