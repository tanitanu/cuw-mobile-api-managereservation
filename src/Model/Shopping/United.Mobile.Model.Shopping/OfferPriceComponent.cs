using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class OfferPriceComponent
    {
        public Dictionary<string, List<String>> Descriptions { get; set; } 
        public Dictionary<string, List<String>> Disclaimers { get; set; } 
        public OfferDisplayPrice Price { get; set; } 
    }
}
