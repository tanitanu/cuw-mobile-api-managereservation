using System;
using United.Mobile.Model.Common;
using United.Services.FlightShopping.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class CSLShopRequest : IPersist
    {
        #region IPersist Members

        private string objectName = "United.Persist.Definition.Shopping.CSLShopRequest";
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


        private ShopRequest _cslShopRequest;
        public ShopRequest ShopRequest
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
