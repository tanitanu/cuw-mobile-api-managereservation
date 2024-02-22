using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class ProductSearchRequest : MOBRequest
    {

        private string cartId = string.Empty;
        private string cartKey = string.Empty;
        private string sessionId = string.Empty;
        private string pointOfSale = string.Empty;
        private string flow = string.Empty;
        private List<string> productCodes;
        private List<MOBSHOPTraveler> travelers;


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

        public string Flow { get; set; } = string.Empty;

        public List<string> ProductCodes { get; set; } 

        public List<MOBSHOPTraveler> Travelers { get; set; }

        public ProductSearchRequest()
        {
            ProductCodes = new List<string>();
            Travelers = new List<MOBSHOPTraveler>();
        }
    }
}
