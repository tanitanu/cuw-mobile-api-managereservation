using Newtonsoft.Json;
using System;
using System.Xml.Serialization;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.FormofPayment
{
    [Serializable()]
    public class MOBFormofPaymentDetails
    {
        private string formOfPaymentType = string.Empty;
        private string emailAddress;
        private MOBAddress billingAddress;
        private MOBInternationalBilling internationalBilling;
        private MOBCPPhone phone;
        private MOBCreditCard creditCard;
        private MOBCreditCard secondaryCreditCard;
        private MOBPayPal payPal;
        private MOBPayPalPayor payPalPayor;
        private MOBMasterpass MasterPass;
        private MasterpassSessionDetails masterPassSessionDetails;
        private MOBApplePay applePayInfo;
        private bool clientCardType = false;
        private MOBEmail email;
        private MilesFOP milesFOP;
        private MOBFOPTravelCertificate travelCertificate;
        private bool isOtherFOPRequired;
        private MOBCreditCard uplift;
        private FOPTravelFutureFlightCredit travelFutureFlightCredit;
        private FOPMoneyPlusMilesCredit moneyPlusMilesCredit;
        private MOBFOPTravelCreditDetails travelCreditDetails;
        private MOBFOPTravelBankDetails travelBankDetails;//chNGE Nmw

        public MOBFOPTravelBankDetails TravelBankDetails
        {
            get { return travelBankDetails; }
            set { travelBankDetails = value; }
        }

        public FOPMoneyPlusMilesCredit MoneyPlusMilesCredit
        {
            get { return moneyPlusMilesCredit; }
            set { moneyPlusMilesCredit = value; }
        }
        public FOPTravelFutureFlightCredit TravelFutureFlightCredit
        {
            get { return travelFutureFlightCredit; }
            set { travelFutureFlightCredit = value; }
        }

        public bool IsOtherFOPRequired
        {
            get { return isOtherFOPRequired; }
            set { isOtherFOPRequired = value; }
        }

        [XmlElement("TravelCertificate")]
        public MOBFOPTravelCertificate TravelCertificate
        {
            get { return travelCertificate; }
            set { travelCertificate = value; }
        }


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
        public string EmailAddress
        {
            get
            {
                return this.emailAddress;
            }
            set
            {
                this.emailAddress = value;
            }
        }

        public MOBAddress BillingAddress
        {
            get
            {
                return this.billingAddress;
            }
            set
            {
                this.billingAddress = value;
            }
        }
        public MOBCPPhone Phone
        {
            get
            {
                return this.phone;
            }
            set
            {
                this.phone = value;
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
                this.formOfPaymentType = value;
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
        public MOBCreditCard SecondaryCreditCard
        {
            get
            {
                return this.secondaryCreditCard;
            }
            set
            {
                this.secondaryCreditCard = value;
            }
        }
        public bool ClientCardType
        {
            get
            {
                return this.clientCardType;
            }
            set
            {
                this.clientCardType = value;
            }
        }
        public MOBPayPal PayPal
        {
            get { return payPal; }
            set { payPal = value; }
        }
        public MOBPayPalPayor PayPalPayor
        {
            get { return payPalPayor; }
            set { payPalPayor = value; }
        }
        public MasterpassSessionDetails MasterPassSessionDetails
        {
            get { return masterPassSessionDetails; }

            set { masterPassSessionDetails = value; }
        }
        public MOBMasterpass masterPass
        {
            get { return MasterPass; }

            set { MasterPass = value; }
        }
        public MOBEmail Email
        {
            get { return email; }
            set { email = value; }
        }
        public MilesFOP MilesFOP
        {
            get { return milesFOP; }
            set { milesFOP = value; }
        }

        public MOBCreditCard Uplift
        {
            get { return uplift; }
            set { uplift = value; }
        }
        public MOBInternationalBilling InternationalBilling
        {
            get
            {
                return this.internationalBilling;
            }
            set
            {
                this.internationalBilling = value;
            }
        }
        public MOBFOPTravelCreditDetails TravelCreditDetails
        {
            get { return travelCreditDetails; }
            set { travelCreditDetails = value; }
        }

    }
}
