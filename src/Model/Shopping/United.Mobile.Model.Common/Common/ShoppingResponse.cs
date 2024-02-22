using System;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]

    public class ShoppingResponse : MOBResponse
    {
        #region IPersist Members

        private string objectName = "United.Persist.Definition.Shopping.ShoppingResponse";
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
        public MOBSHOPShopRequest Request { get; set; }
        public ShopResponse Response { get; set; }
        public string Flow { get; set; } = string.Empty;
        public string CartId { get; set; }
        public ShopPricesCommon PriceSummary { get; set; }
    }
}
