using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class ShopPrice
    {
        public string CellId { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PriceSubtype { get; set; } = string.Empty;
        public string PriceType { get; set; } = string.Empty;
        public List<ShopPricingItem> PricingItems { get; set; }
        public string SolutionUrl { get; set; } = string.Empty;
        public ShopPrice()
        {
            PricingItems = new List<ShopPricingItem>();
        }
    }
}
