using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class OfferPrice
    {
        private string id = string.Empty;
        private OfferPriceAssociation association;
        private List<OfferPaymentOption> paymentOptions;

        public string Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public OfferPriceAssociation Association { get; set; } 

        public List<OfferPaymentOption> PaymentOptions { get; set; }
        public OfferPrice()
        {
            PaymentOptions = new List<OfferPaymentOption>();
        }
    }
}
