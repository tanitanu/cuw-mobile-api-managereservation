using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class OfferPaymentOption
    {
        private string eddCode = string.Empty;
        private List<OfferPriceComponent> priceComponents;
        private string type = string.Empty;

        public string EddCode
        {
            get
            {
                return this.eddCode;
            }
            set
            {
                this.eddCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<OfferPriceComponent> PriceComponents
        {
            get
            {
                return this.priceComponents;
            }
            set
            {
                this.priceComponents = value;
            }
        }

        public string Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        //public string EddCode { get; set; } = string.Empty;

        //public List<OfferPriceComponent> PriceComponents { get; set; }

        //public string Type { get; set; } = string.Empty;

    }
}
