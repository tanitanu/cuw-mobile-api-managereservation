using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class Offer
    {
        public string Id { get; set; } = string.Empty;
        
        public string OfferDescriptionHeader { get; set; } = string.Empty;
      
        public string OfferDescription { get; set; } = string.Empty;
       
        public List<OfferProduct> Products { get; set; }
        public Offer()
        {
            Products = new List<OfferProduct>();
        }
    }
}
