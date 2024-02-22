using System;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping.FormofPayment;

namespace United.Mobile.Model.Shopping.FormofPayment
{
    [Serializable()]
    public class CheckOutRequest : MOBRequest
    {
        private string sessionId;
        private string cartId = string.Empty;
        private string flow = string.Empty;
        private string token;
        private MOBFormofPaymentDetails formofPaymentDetails;
        private string paymentAmount;
        private string additionalData = string.Empty;
        private string totalMiles;

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

        public string Token
        { get { return this.token; } set { this.token = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }

        public MOBFormofPaymentDetails FormofPaymentDetails
        {
            get { return formofPaymentDetails; }
            set { formofPaymentDetails = value; }
        }
        public string PaymentAmount
        {
            get { return paymentAmount; }
            set { paymentAmount = value; }
        }

        public string AdditionalData
        {
            get
            {
                return this.additionalData;
            }
            set
            {
                this.additionalData = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string TotalMiles
        {
            get { return totalMiles; }
            set { totalMiles = value; }
        }
    }
}
