using System;
using United.Mobile.Model.Shopping.Pcu;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class Ancillary
    {
        private Accelerators awardAccelerators;
        private MOBPremiumCabinUpgrade premiumCabinUpgrade;
        private PriorityBoarding priorityBoarding;
        private PlacePass placePass;
        private TravelOptionsBundle travelOptionsBundle;
        private BasicEconomyBuyOut basicEconomyBuyOut;

        public PlacePass PlacePass
        {
            get { return placePass; }
            set { placePass = value; }
        }

        public Accelerators AwardAccelerators
        {
            get { return awardAccelerators; }
            set { awardAccelerators = value; }
        }

        public MOBPremiumCabinUpgrade PremiumCabinUpgrade
        {
            get { return premiumCabinUpgrade; }
            set { premiumCabinUpgrade = value; }
        }

        public PriorityBoarding PriorityBoarding
        {
            get { return priorityBoarding; }
            set { priorityBoarding = value; }
        }

        public TravelOptionsBundle TravelOptionsBundle
        {
            get { return travelOptionsBundle; }
            set { travelOptionsBundle = value; }
        }

        public BasicEconomyBuyOut BasicEconomyBuyOut
        {
            get { return basicEconomyBuyOut; }
            set { basicEconomyBuyOut = value; }
        }
    }
}
