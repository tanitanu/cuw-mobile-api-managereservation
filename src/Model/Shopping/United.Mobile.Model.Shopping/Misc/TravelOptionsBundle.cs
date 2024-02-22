using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class TravelOptionsBundle
    {
        private MOBOfferTile offerTile;
        private List<MOBBundleProduct> products;
        private MOBMobileCMSContentMessages termsAndCondition;
        private int numberOfTravelers;

        public MOBOfferTile OfferTile
        {
            get { return offerTile; }
            set { offerTile = value; }
        }

        public List<MOBBundleProduct> Products
        {
            get { return products; }
            set { products = value; }
        }

        public MOBMobileCMSContentMessages TermsAndCondition
        {
            get { return termsAndCondition; }
            set { termsAndCondition = value; }
        }

        public int NumberOfTravelers
        {
            get { return numberOfTravelers; }
            set { numberOfTravelers = value; }
        }
    }
}