using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class OfferProduct
    {
        private string code = string.Empty;
        private string description = string.Empty;
        private string displayName = string.Empty;
        private List<OfferSubProduct> subProducts;

        public string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DisplayName
        {
            get
            {
                return this.displayName;
            }
            set
            {
                this.displayName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public List<OfferSubProduct> SubProducts { get; set; }
        public OfferProduct()
        {
            SubProducts = new List<OfferSubProduct>();
        }
    }
}
