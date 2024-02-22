using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class ClubDayPassOffer
    {
        private string offerDescriptionHeader = string.Empty;
        private string offerDescription = string.Empty;
        private string productCode = string.Empty;
        private string productName = string.Empty;
        private List<MOBSHOPPrice> prices;
        private List<string> descriptions;
        private List<string> termsAndConditions;

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

        public List<MOBSHOPPrice> Prices
        {
            get
            {
                return this.prices;
            }
            set
            {
                this.prices = value;
            }
        }

        public List<string> Descriptions
        {
            get
            {
                return this.descriptions;
            }
            set
            {
                this.descriptions = value;
            }
        }

        public List<string> TermsAndConditions
        {
            get
            {
                return this.termsAndConditions;
            }
            set
            {
                this.termsAndConditions = value;
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
                this.productCode = value;
            }
        }
        public string ProductName
        {
            get
            {
                return this.productName;
            }
            set
            {
                this.productName = value;
            }
        }
    }
}
