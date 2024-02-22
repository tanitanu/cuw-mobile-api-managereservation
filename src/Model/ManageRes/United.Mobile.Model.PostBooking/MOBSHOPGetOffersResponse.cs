using System;
using United.Mobile.Model;
using United.Mobile.Model.MPRewards;
using United.Mobile.Model.Shopping.Pcu;

namespace United.Mobile.Model.PostBooking
{
    [Serializable()]
    public class MOBSHOPGetOffersResponse : MOBResponse
    {
        private MOBSHOPGetOffersRequest request;
        private MOBPremiumCabinUpgrade premiumCabinUpgrade;
        private string sessionId;
        private MOBPriorityBoarding priorityBoarding;

        public MOBSHOPGetOffersRequest Request
        {
            get { return request; }
            set { request = value; }
        }

        public MOBPriorityBoarding PriorityBoarding
        {
            get { return priorityBoarding; }
            set { priorityBoarding = value; }
        }

        public MOBPremiumCabinUpgrade PremiumCabinUpgrade
        {
            get { return premiumCabinUpgrade; }
            set { premiumCabinUpgrade = value; }
        }

        public string SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }
    }
}
