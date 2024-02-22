using System;
using United.Services.FlightShopping.Common;

namespace United.Mobile.Model.TripPlannerGetService
{
    [Serializable()]
    public class TripPlanTrip
    {
        private string cartID;
        private ShopRequest cslShopRequest;
        private ShopResponse cslShopResponse;
        

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

        public ShopResponse CslShopResponse
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