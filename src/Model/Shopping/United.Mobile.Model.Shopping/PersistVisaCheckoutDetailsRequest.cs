using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
   public class PersistVisaCheckoutDetailsRequest: MOBRequest
    {
        private string sessionId;
        private string mPNumber;
        private string customerId;
        private MOBVisaCheckout visaCheckOutDetails;
        private MOBFormofPayment formOfPaymentType;

        public MOBVisaCheckout VisaCheckOutDetails { get; set; } 

        public MOBSHOPFormOfPayment FormOfPaymentType { get; set; }
        public string MPNumber
        {
            get { return mPNumber; }
            set { this.mPNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string CustomerId
        {
            get { return customerId; }
            set { this.customerId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string SessionId
        {
            get { return sessionId; }
            set { this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

    }
}
