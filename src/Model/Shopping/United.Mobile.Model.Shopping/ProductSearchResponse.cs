using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class ProductSearchResponse : MOBResponse
    {
        private string sessionId = string.Empty;
        private string cartId = string.Empty;
        private string flow = string.Empty;

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

        public string Flow { get; set; } = string.Empty;

        public OfferSource OfferSource { get; set; } 

        public List<BundleProducts> BundleProducts { get; set; }

        public ProductSearchResponse()
        {
            BundleProducts = new List<BundleProducts>();
        }
    }
}
