using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Shopping;

namespace United.Mobile.Model.ReShop
{
    [Serializable]
    public class MOBSHOPProductSearchResponse : MOBResponse
    {
        private string sessionId = string.Empty;
        private string cartId = string.Empty;
        private string flow = string.Empty;
        private OfferSource offerSource;
        private List<BundleProducts> bundleProducts;

        public string SessionId
        {
            get
            {
                return this.sessionId;
            }
            set
            {
                this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string CartId
        {
            get
            {
                return this.cartId;
            }
            set
            {
                this.cartId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Flow
        {
            get
            {
                return this.flow;
            }
            set
            {
                this.flow = value;
            }
        }

        public OfferSource OfferSource
        {
            get
            {
                return this.offerSource;
            }
            set
            {
                this.offerSource = value;
            }
        }

        public List<BundleProducts> BundleProducts
        {
            get
            {
                return this.bundleProducts;
            }
            set
            {
                this.bundleProducts = value;
            }
        }
    }
}
