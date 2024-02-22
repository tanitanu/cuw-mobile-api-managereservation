using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Travelers
{
    [Serializable()]
    public class MOBMPNameMissMatchRequest : MOBRequest
    {
        private string sessionId = string.Empty;
        private string cartId = string.Empty;
        private string token = string.Empty;
        private string mileagePlusNumber = string.Empty;
        private CPTraveler traveler;
        private List<int> alreadySelectedPAXIDs;
        private string flow;

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

        public string Token
        {
            get
            {
                return token;
            }
            set
            {
                this.token = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string MileagePlusNumber
        {
            get
            {
                return mileagePlusNumber;
            }
            set
            {
                this.mileagePlusNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public CPTraveler Traveler
        {
            get
            {
                return this.traveler;
            }
            set
            {
                this.traveler = value;
            }
        }

        public List<int> AlreadySelectedPAXIDs
        {
            get
            {
                return alreadySelectedPAXIDs;
            }
            set
            {
                this.alreadySelectedPAXIDs = value;
            }
        }

        public string Flow
        {
            get
            {
                return this.flow;
            }
            set
            {
                this.flow = value;
            }
        }

        public MOBMPNameMissMatchRequest()
            : base()
        {
        }
    }
}
