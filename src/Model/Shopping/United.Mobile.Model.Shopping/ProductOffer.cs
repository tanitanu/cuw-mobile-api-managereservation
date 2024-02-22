using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using United.Mobile.Model.Shopping.Misc;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class ProductOffer
    {
        public List<ProductOffersFlight> ProductOffersPerSegment { get; set; }

        public List<TnC> TnCs { get; set; }

        public List<OfferBenefit> Benefits { get; set; }
        public ProductOffer()
        {
            ProductOffersPerSegment = new List<ProductOffersFlight>();
            TnCs = new List<TnC>();
            Benefits = new List<OfferBenefit>();
        }

    }
}
