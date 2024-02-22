using System;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.Shopping;

namespace United.Mobile.Model.Shopping.FormofPayment
{
    [Serializable()]
    public class FOPBillingContactInfoRequest : ShoppingRequest
    {
        private MOBEmail email;
        private MOBCPPhone phone;
        private MOBAddress billingAddress;

        public MOBEmail Email
        {
            get { return email; }
            set { email = value; }
        }

        public MOBCPPhone Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public MOBAddress BillingAddress
        {
            get { return billingAddress; }
            set { billingAddress = value; }
        }

    }
}
