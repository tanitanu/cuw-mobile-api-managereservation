using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.FormofPayment
{
    [Serializable()]
    public class FOPEligibilityRequest : MOBRequest
    {
        private string sessionId = string.Empty;
        private string cartId = string.Empty;
        private string flow = string.Empty;
        private Collection<FOPProduct> products;
        //private string pointOfSale = string.Empty;

        public string SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }
        public string CartId
        {
            get { return cartId; }
            set { cartId = value; }
        }
        public string Flow
        {
            get { return flow; }
            set { flow = value; }
        }

        public Collection<FOPProduct> Products
        {
            get { return products; }
            set { products = value; }
        }

        //public string PointOfSale
        //{
        //    get { return pointOfSale; }
        //    set { pointOfSale = value; }
        //}
    }

    public class FOPProduct
    {
        private string code = string.Empty;
        private string productDescription = string.Empty;

        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        public string ProductDescription
        {
            get { return productDescription; }
            set { productDescription = value; }
        }
    }
}
