using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class RegisterOfferRequest : MOBRequest
    {
        private string cartId = string.Empty;
        private string cartKey = string.Empty;
        private string sessionId = string.Empty;
        private string pointOfSale = string.Empty;
        private List<RegisterOffer> offers;
        private ClubPassPurchaseRequest clubPassPurchaseRequest;
        private List<BundleRegisterOffer> products;
        private List<BundleRegisterOffer> productsToBeRemoved;
        private string flow = string.Empty;

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

        public string CartKey
        {
            get
            {
                return this.cartKey;
            }
            set
            {
                this.cartKey = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }


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

        public string PointOfSale
        {
            get
            {
                return this.pointOfSale;
            }
            set
            {
                this.pointOfSale = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }


        public List<RegisterOffer> Offers { get; set; } 

        public ClubPassPurchaseRequest ClubPassPurchaseRequest { get; set; }

        public List<BundleRegisterOffer> Products { get; set; } 

        public List<BundleRegisterOffer> ProductsToBeRemoved { get; set; }

        public string Flow
        {
            get
            {
                return this.flow;
            }
            set
            {
                this.flow = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public RegisterOfferRequest()
        {
            Offers = new List<RegisterOffer>();
            Products = new List<BundleRegisterOffer>();
            ProductsToBeRemoved = new List<BundleRegisterOffer>();
        }

    }
}
