using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.FormofPayment
{
    [Serializable()]
    public class PersistFormofPaymentRequest : MOBRequest
    {
        private string sessionId = string.Empty;
        private string cartId = string.Empty;
        private string flow = string.Empty;
        private string amount;
        private MOBFormofPaymentDetails formofPaymentDetails;
        private bool isCCSelectedForContactless;

        public bool IsCCSelectedForContactless
        {
            get { return isCCSelectedForContactless; }
            set { isCCSelectedForContactless = value; }
        }


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
        public MOBFormofPaymentDetails FormofPaymentDetails
        {
            get { return formofPaymentDetails; }
            set { formofPaymentDetails = value; }
        }
    }
}
