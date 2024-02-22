using System;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBSHOPFormOfPayment
    {
        public MOBSHOPFormOfPayment()
        {
            this.formOfPayment = MOBFormofPayment.CreditCard;
        }

        private string formOfPaymentType = string.Empty;
        private MOBCreditCard creditCard;
        private string visaCheckOutCallID = string.Empty;
        private MOBPayPal payPal;
        private MOBFormofPayment? formOfPayment = null;
        private MOBApplePay applePayInfo;
        private MOBMasterpass masterpass;
        public MOBApplePay ApplePayInfo
        {
            get
            {
                return applePayInfo;
            }

            set
            {
                applePayInfo = value;
            }
        }

        public string FormOfPaymentType
        {
            get
            {
                return this.formOfPaymentType;
            }
            set
            {
                this.formOfPaymentType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public MOBCreditCard CreditCard
        {
            get
            {
                return this.creditCard;
            }
            set
            {
                this.creditCard = value;
            }
        }

        public string VISACheckOutCallID
        {
            get
            {
                return this.visaCheckOutCallID;
            }
            set
            {
                this.visaCheckOutCallID = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }

        public MOBFormofPayment? FormOfPayment
        {
            get { return formOfPayment; }
            set { formOfPayment = value == null ? MOBFormofPayment.CreditCard : value; }
        }

        public MOBPayPal PayPal
        {
            get { return payPal; }
            set { payPal = value; }
        }

        public MOBMasterpass Masterpass
        {
            get { return masterpass; }

            set { masterpass = value; }
        }
    }
}
