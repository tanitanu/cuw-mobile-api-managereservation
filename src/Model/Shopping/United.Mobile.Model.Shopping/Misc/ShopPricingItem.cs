using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class ShopPricingItem
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public List<ShopPriceDetail> PricingDetails { get; set; }
        public string PricingType { get; set; } = string.Empty;
        public ShopPricingItem()
        {
            PricingDetails = new List<ShopPriceDetail>();
        }
    }
}
