using System;
using System.Collections.Generic;
using United.Mobile.Model.Shopping;

namespace United.Mobile.Model.ReShop
{
    [Serializable()]
    public class MOBSHOPShopResponse : MOBResponse
    {
        private MOBSHOPShopRequest shopRequest;
        private MOBSHOPAvailability availability;
        private List<string> disclaimer;
        private string cartId = string.Empty;
        private string refreshResultsData = string.Empty;
        private bool isRevenueLowestPriceForAwardSearchEnabled = false;
        private bool hideBackButtonOnFSRForAndroid = false;
        private MOBSHOPPointOfSale pointOfSale;
        private MOBSHOPPointOfSale internationalPointofSale;


        public MOBSHOPPointOfSale InternationalPointofSale
        {
            get { return internationalPointofSale; }
            set { internationalPointofSale = value; }
        }
        public MOBSHOPPointOfSale PointOfSale
        {
            get { return pointOfSale; }
            set { pointOfSale = value; }
        }


        public MOBSHOPShopRequest ShopRequest
        {
            get
            {
                return this.shopRequest;
            }
            set
            {
                this.shopRequest = value;
            }
        }

        public MOBSHOPAvailability Availability
        {
            get
            {
                return this.availability;
            }
            set
            {
                this.availability = value;
            }
        }

        public List<string> Disclaimer
        {
            get
            {
                return this.disclaimer;
            }
            set
            {
                this.disclaimer = value;
            }
        }
        public string CartId
        {
            get { return cartId; }
            set { cartId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
        public string RefreshResultsData
        {
            get { return refreshResultsData; }
            set { refreshResultsData = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private bool noFlightsWithStops;
        public bool NoFlightsWithStops
        {
            get { return noFlightsWithStops; }
            set { noFlightsWithStops = value; }
        }
        public bool IsRevenueLowestPriceForAwardSearchEnabled
        {
            get { return isRevenueLowestPriceForAwardSearchEnabled; }
            set { isRevenueLowestPriceForAwardSearchEnabled = value; }
        }
        //To Hide the back button in the FSR1 Screen in Andriod
        public bool HideBackButtonOnFSRForAndroid
        {
            get { return hideBackButtonOnFSRForAndroid; }
            set { hideBackButtonOnFSRForAndroid = value; }
        }
    }
}
