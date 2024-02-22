using System;

namespace United.Mobile.Model.ManageRes
{
    [Serializable]
    public class MOBPayment
    {
        private string accountNumber;
        public string AccountNumber {
            get { return accountNumber; }
            set { accountNumber = value; }
        }

        private string paymentName;
        public string PaymentName {
            get { return paymentName; }
            set { paymentName = value; }
        }

        private string paymentType;
        public string PaymentType {
            get { return paymentType; } 
            set { paymentType = value; }
        }

    }
}
