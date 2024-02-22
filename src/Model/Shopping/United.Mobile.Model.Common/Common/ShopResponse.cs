using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using United.Mobile.Model.Catalog;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    [XmlRoot("MOBSHOPShopResponse")]
    public class ShopResponse : MOBResponse
    {
        public MOBSHOPShopRequest ShopRequest { get; set; }
        public MOBSHOPAvailability Availability { get; set; }
        public List<string> Disclaimer { get; set; }
        public string CartId { get; set; } = string.Empty;
        public string RefreshResultsData { get; set; } = string.Empty;
        public bool NoFlightsWithStops { get; set; }
        public bool IsRevenueLowestPriceForAwardSearchEnabled { get; set; }
        //To Hide the back button in the FSR1 Screen in Andriod
        public bool HideBackButtonOnFSRForAndroid { get; set; }
        public PointOfSale PointOfSale { get; set; }
        public PointOfSale InternationalPointofSale { get; set; }
        public List<MOBOptimizelyQMData> ExperimentEvents { get; set; }
        public MOBPromoAlertMessage PromoAlertMessage { get; set; }
        public bool ShowEditSearchHeaderOnFSRBooking { get; set; }

        public ShopResponse()
        {
            Disclaimer = new List<string>();
            ExperimentEvents = new List<MOBOptimizelyQMData>();
        }
    }
}
