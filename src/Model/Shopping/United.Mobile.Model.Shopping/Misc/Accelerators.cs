using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class Accelerators
    {
        private MOBOfferTile offerTile;
        private List<MOBAcceleratorTraveler> eligibleTravelers;
        private List<MOBTNCItem> optionsTnCs;
        private List<MOBTNCItem> paymentTnCs;
        private List<MOBItem> captions;
        private List<MOBMobileCMSContentMessages> awardAcceleratorTnCs;
        private List<MOBMobileCMSContentMessages> premierAcceleratorTnCs;
        private List<MOBMobileCMSContentMessages> termsAndConditions;

        public MOBOfferTile OfferTile
        {
            get { return offerTile; }
            set { offerTile = value; }
        }

        public List<MOBAcceleratorTraveler> EligibleTravelers
        {
            get { return eligibleTravelers; }
            set { eligibleTravelers = value; }
        }

        public List<MOBTNCItem> OptionsTnCs
        {
            get { return optionsTnCs; }
            set { optionsTnCs = value; }
        }

        public List<MOBItem> Captions
        {
            get { return captions; }
            set { captions = value; }
        }

        public List<MOBTNCItem> PaymentTnCs
        {
            get { return paymentTnCs; }
            set { paymentTnCs = value; }
        }

        public List<MOBMobileCMSContentMessages> AwardAcceleratorTnCs
        {
            get { return awardAcceleratorTnCs; }
            set { awardAcceleratorTnCs = value; }
        }

        public List<MOBMobileCMSContentMessages> PremierAcceleratorTnCs
        {
            get { return premierAcceleratorTnCs; }
            set { premierAcceleratorTnCs = value; }
        }

        public List<MOBMobileCMSContentMessages> TermsAndConditions
        {
            get { return termsAndConditions; }
            set { termsAndConditions = value; }
        }
    }
}
