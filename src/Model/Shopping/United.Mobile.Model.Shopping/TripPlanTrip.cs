using System;
using United.Services.FlightShopping.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class TripPlanTrip
    {
        private string cartID;
        private ShopRequest cslShopRequest;
        private United.Services.FlightShopping.Common.ShopResponse cslShopResponse;
        

        public string CartID
        {
            get
            {
                return cartID;
            }
            set
            {
                cartID = value;
            }
        }

        public ShopRequest CslShopRequest
        {
            get
            {
                return cslShopRequest;
            }
            set
            {
                cslShopRequest = value;
            }
        }

        public United.Services.FlightShopping.Common.ShopResponse CslShopResponse
        {
            get
            {
                return cslShopResponse;
            }
            set
            {
                cslShopResponse = value;
            }
        }
    }
}