using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class MOBVisaCheckout
    {
        private MOBCreditCard visaCheckoutCard;
        private string visaCheckOutCallID;

        public string VisaCheckOutCallID
        {
            get { return visaCheckOutCallID; }
            set { visaCheckOutCallID = value; }
        }

        public MOBCreditCard VisaCheckoutCard
        {
            get { return visaCheckoutCard; }
            set { visaCheckoutCard = value; }
        }
    }
}
