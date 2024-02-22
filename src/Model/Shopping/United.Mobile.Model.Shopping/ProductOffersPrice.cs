using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping

{
    [Serializable]
    public class ProductOffersPrice
    {
        private string offerID = string.Empty;
        private string offerDescriptionHeader = string.Empty;
        private string offerDescription = string.Empty;

        private string productCode = string.Empty;
        private string productDescription = string.Empty;
        private string productDisplayName = string.Empty;

        private string subproductID = string.Empty;
        private string subproductName = string.Empty;
        private string subproductCode = string.Empty;
        private List<string> subproductDescriptions;
        private string subproductGroupCode = string.Empty;
        private string subproductSubGroupCode = string.Empty;

        private string priceID = string.Empty;
        private List<string> priceIds;
        private OfferPriceAssociation association;
        private List<OfferPaymentOption> paymentOptions;


        public string OfferID
        {
            get
            {
                return this.offerID;
            }
            set
            {
                this.offerID = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string OfferDescriptionHeader
        {
            get
            {
                return this.offerDescriptionHeader;
            }
            set
            {
                this.offerDescriptionHeader = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string OfferDescription
        {
            get
            {
                return this.offerDescription;
            }
            set
            {
                this.offerDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ProductCode
        {
            get
            {
                return this.productCode;
            }
            set
            {
                this.productCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ProductDescription
        {
            get
            {
                return this.productDescription;
            }
            set
            {
                this.productDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ProductDisplayName
        {
            get
            {
                return this.productDisplayName;
            }
            set
            {
                this.productDisplayName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string SubproductID
        {
            get
            {
                return this.subproductID;
            }
            set
            {
                this.subproductID = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string SubproductName
        {
            get
            {
                return this.subproductName;
            }
            set
            {
                this.subproductName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string SubproductCode
        {
            get
            {
                return this.subproductCode;
            }
            set
            {
                this.subproductCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<string> SubproductDescriptions { get; set; }

        public string SubproductGroupCode
        {
            get
            {
                return this.subproductGroupCode;
            }
            set
            {
                this.subproductGroupCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string SubproductSubGroupCode
        {
            get
            {
                return this.subproductSubGroupCode;
            }
            set
            {
                this.subproductSubGroupCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string PriceID
        {
            get
            {
                return this.priceID;
            }
            set
            {
                this.priceID = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<string> PriceIds { get; set; } 

        public OfferPriceAssociation Association { get; set; }

        public List<OfferPaymentOption> PaymentOptions { get; set; }

        public ProductOffersPrice()
        {
            SubproductDescriptions = new List<string>();
            PriceIds = new List<string>();
            PaymentOptions = new List<OfferPaymentOption>();
        }
    }
}
