using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Shopping.FormofPayment;

namespace United.Mobile.Model.Shopping.FormofPayment
{
    [Serializable()]
    public class FOPAcquireMasterPassTokenRequest : AcquireMasterpassTokenRequest
    {
        private string cartId = string.Empty;
        private string flow = string.Empty;

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
    }
}
