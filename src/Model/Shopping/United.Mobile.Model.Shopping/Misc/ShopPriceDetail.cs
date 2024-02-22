using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class ShopPriceDetail
    {
        public double Amount { get; set; } 

        public string Currency { get; set; } = string.Empty;

        public string DetailDescription { get; set; } = string.Empty;

        public string DetailType { get; set; } = string.Empty;

        public string PriceSubtype { get; set; } = string.Empty;

        public string PriceType { get; set; } = string.Empty;
    }
}
