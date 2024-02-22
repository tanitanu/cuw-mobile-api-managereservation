using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class CSLShopResponse : IPersist
    {
        #region IPersist Members

        private string objectName = "United.Persist.Definition.Shopping.CSLShopResponse";
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

        #endregion

        public string SessionId { get; set; }

        private United.Services.FlightShopping.Common.ShopResponse _cslShopResponse;
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
    }
}
