using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Travelers
{
    [Serializable()]
    public class MOBRegisterTravelersRequest : MOBRequest
    {
        private string sessionId = string.Empty;
        private string cartId = string.Empty;
        private string token = string.Empty;
        private string profileKey = string.Empty;
        private int profileId;
        private int profileOwnerId;
        private string profileOwnerKey = string.Empty;
        private string mileagePlusNumber = string.Empty;
        private MOBCPTraveler profileOwner;
        private List<MOBCPTraveler> travelers;
        private string flow;
        private bool continueToChangeTravelers;
        private bool isRegisterTravelerFromRTI;
        private bool isOmniCartSavedTripFlow;
        

        public bool IsOmniCartSavedTripFlow
        {
            get { return isOmniCartSavedTripFlow; }
            set { isOmniCartSavedTripFlow = value; }
        }



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

        public bool IsRegisterTravelerFromRTI
        {
            get { return isRegisterTravelerFromRTI; }
            set { isRegisterTravelerFromRTI = value; }
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

        public string ProfileKey
        {
            get
            {
                return this.profileKey;
            }
            set
            {
                this.profileKey = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public int ProfileId
        {
            get
            {
                return this.profileId;
            }
            set
            {
                this.profileId = value;
            }
        }

        public int ProfileOwnerId
        {
            get
            {
                return this.profileOwnerId;
            }
            set
            {
                this.profileOwnerId = value;
            }
        }

        public string ProfileOwnerKey
        {
            get
            {
                return this.profileOwnerKey;
            }
            set
            {
                this.profileOwnerKey = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public MOBCPTraveler ProfileOwner
        {
            get
            {
                return this.profileOwner;
            }
            set
            {
                this.profileOwner = value;
            }
        }

        public List<MOBCPTraveler> Travelers
        {
            get
            {
                return this.travelers;
            }
            set
            {
                this.travelers = value;
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

        public bool ContinueToChangeTravelers
        {
            get { return continueToChangeTravelers; }
            set { continueToChangeTravelers = value; }
        }

        public MOBRegisterTravelersRequest()
            : base()
        {
        }
    }
}
