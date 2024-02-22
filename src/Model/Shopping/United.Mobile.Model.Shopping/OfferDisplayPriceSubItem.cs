using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class OfferDisplayPriceSubItem
    {
        public string Amount { get; set; } = string.Empty;
        
        public string CurrencyCode { get; set; } = string.Empty;
      
        public string Type { get; set; } = string.Empty;
        
    }
}
