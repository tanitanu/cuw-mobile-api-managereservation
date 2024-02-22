using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class OfferDisplayPrice
    {
        public List<OfferDisplayPriceSubItem> Adjustments { get; set; }
        public OfferDisplayPriceSubItem BasePrice { get; set; } 
        public List<OfferDisplayPriceSubItem> Fees { get; set; } 
        public List<OfferDisplayPriceSubItem> Totals { get; set; }
        public string FareType { get; set; } = string.Empty;

        public OfferDisplayPrice()
        {
            Adjustments = new List<OfferDisplayPriceSubItem>();
            Fees = new List<OfferDisplayPriceSubItem>();
            Totals = new List<OfferDisplayPriceSubItem>();

        }
       
    }
}
