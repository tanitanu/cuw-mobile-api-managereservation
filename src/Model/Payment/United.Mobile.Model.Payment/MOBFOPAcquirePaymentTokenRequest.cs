using System;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.Shopping;

namespace United.Mobile.Model.Payment
{
    [Serializable()]
    //v788383: 11/02/2018 - Generic Method for Token generation
    public class MOBFOPAcquirePaymentTokenRequest : MOBRequest
    {
        private string countryCode = string.Empty;
        private string sessionId = string.Empty;
        private string cartId = string.Empty;
        private string flow = string.Empty;
        private string amount;
        private string formofPaymentCode = string.Empty;

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
        public string Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        public string FormofPaymentCode
        {
            get { return formofPaymentCode; }
            set { formofPaymentCode = value; }
        }

        public string CountryCode
        {
            get { return countryCode; }
            set { countryCode = value; }
        }
    }
}
