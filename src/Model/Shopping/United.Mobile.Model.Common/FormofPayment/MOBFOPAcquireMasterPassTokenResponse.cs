using System;

namespace United.Mobile.Model.Shopping.FormofPayment
{
    [Serializable()]
    public class FOPAcquireMasterPassTokenResponse : AcquireMasterpassTokenResponse
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
