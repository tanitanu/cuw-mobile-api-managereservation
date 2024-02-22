using System;
using United.Mobile.Model.Common;
using United.Services.FlightShopping.Common;



namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class CSLSelectTrip : IPersist
    {
        #region IPersist Members

        private string objectName = "United.Persist.Definition.Shopping.CSLSelectTrip";
        public string ObjectName
        {
            get
            {
                return this.objectName;
            }
            set
            {
                this.objectName = value;
            }
        }
        #endregion IPersist Members
        private United.Services.FlightShopping.Common.ShopResponse _cslShopResponse;
        private United.Services.FlightShopping.Common.ShopRequest _cslShopRequest;
        private SelectTripResponse _mobSelectTripResponse;

        public SelectTripResponse MobSelectTripResponse
        {
            get
            {
                return _mobSelectTripResponse;
            }
            set
            {
                _mobSelectTripResponse = value;
            }
        }
        public United.Services.FlightShopping.Common.ShopResponse ShopCSLResponse
        {
            get
            {
                return _cslShopResponse;
            }
            set
            {
                _cslShopResponse = value;
            }
        }

        public United.Services.FlightShopping.Common.ShopRequest ShopCSLRequest
        {
            get
            {
                return _cslShopRequest;
            }
            set
            {
                _cslShopRequest = value;
            }
        }
    }
}
