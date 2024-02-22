using System;
using United.Mobile.Model.Shopping.Pcu;

namespace United.Mobile.Model.ManageRes
{
    [Serializable()]
    public class MOBAncillary
    {
        private MOBAccelerators awardAccelerators;
        private MOBPremiumCabinUpgrade premiumCabinUpgrade;
        private United.Mobile.Model.MPRewards.MOBPriorityBoarding priorityBoarding;
        private MOBPlacePass placePass;
        private MOBTravelOptionsBundle travelOptionsBundle;
        private MOBBasicEconomyBuyOut basicEconomyBuyOut;

        public MOBPlacePass PlacePass
        {
            get { return placePass; }
            set { placePass = value; }
        }

        public MOBAccelerators AwardAccelerators
        {
            get { return awardAccelerators; }
            set { awardAccelerators = value; }
        }

        public MOBPremiumCabinUpgrade PremiumCabinUpgrade
        {
            get { return premiumCabinUpgrade; }
            set { premiumCabinUpgrade = value; }
        }

        public United.Mobile.Model.MPRewards.MOBPriorityBoarding PriorityBoarding
        {
            get { return priorityBoarding; }
            set { priorityBoarding = value; }
        }

        public MOBTravelOptionsBundle TravelOptionsBundle
        {
            get { return travelOptionsBundle; }
            set { travelOptionsBundle = value; }
        }

        public MOBBasicEconomyBuyOut BasicEconomyBuyOut
        {
            get { return basicEconomyBuyOut; }
            set { basicEconomyBuyOut = value; }
        }
    }
}
