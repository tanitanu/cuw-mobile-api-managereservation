using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class ClubDayPassPurchaseRequest : MOBRequest
    {
        public ClubDayPassPurchaseRequest()
            : base()
        {
        }

        private string firstName = string.Empty;
        private string lastName = string.Empty;
        private string email = string.Empty;
        private string mileagePlusNumber = string.Empty;
        private string location = string.Empty;
        private string deviceType = string.Empty;
        private string creditCardNumber = string.Empty;
        private string expirationDate = string.Empty;
        private int numberOfPasses;
        private string amountPaid = string.Empty;
        private bool testSystem;

        public string FirstName
        {
            get
            {
                return this.firstName;
            }
            set
            {
                this.firstName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string LastName
        {
            get
            {
                return this.lastName;
            }
            set
            {
                this.lastName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Email
        {
            get
            {
                return this.email;
            }
            set
            {
                this.email = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string MileagePlusNumber
        {
            get
            {
                return this.mileagePlusNumber;
            }
            set
            {
                this.mileagePlusNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Location
        {
            get
            {
                return this.location;
            }
            set
            {
                this.location = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DeviceType
        {
            get
            {
                return this.deviceType;
            }
            set
            {
                this.deviceType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string CreditCardNumber
        {
            get
            {
                return this.creditCardNumber;
            }
            set
            {
                this.creditCardNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ExpirationDate
        {
            get
            {
                return this.expirationDate;
            }
            set
            {
                this.expirationDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public int NumberOfPasses
        {
            get
            {
                return this.numberOfPasses;
            }
            set
            {
                this.numberOfPasses = value;
            }
        }

        public string AmountPaid
        {
            get
            {
                return this.amountPaid;
            }
            set
            {
                this.amountPaid = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool TestSystem
        {
            get
            {
                return this.testSystem;
            }
            set
            {
                this.testSystem = value;
            }
        }
    }

    [Serializable()]
    public class OTPPurchaseRequest : MOBRequest
    {
        public OTPPurchaseRequest()
            : base()
        {
        }

        private string firstName = string.Empty;
        private string lastName = string.Empty;
        private string email = string.Empty;
        private string mileagePlusNumber = string.Empty;
        private string location = string.Empty;
        private string deviceType = string.Empty;
        private int numberOfPasses;
        private bool testSystem;
        private string sessionId;
        private double amountPaid;
        private MOBCreditCard creditCard;
        private MOBAddress address;
        private MOBPayPal payPal;
        private MOBMasterpass masterpass;
        private MOBFormofPayment formOfPayment;

        public string FirstName
        {
            get
            {
                return this.firstName;
            }
            set
            {
                this.firstName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string LastName
        {
            get
            {
                return this.lastName;
            }
            set
            {
                this.lastName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Email
        {
            get
            {
                return this.email;
            }
            set
            {
                this.email = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string MileagePlusNumber
        {
            get
            {
                return this.mileagePlusNumber;
            }
            set
            {
                this.mileagePlusNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Location
        {
            get
            {
                return this.location;
            }
            set
            {
                this.location = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DeviceType
        {
            get
            {
                return this.deviceType;
            }
            set
            {
                this.deviceType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public int NumberOfPasses
        {
            get
            {
                return this.numberOfPasses;
            }
            set
            {
                this.numberOfPasses = value;
            }
        }

        public bool TestSystem
        {
            get
            {
                return this.testSystem;
            }
            set
            {
                this.testSystem = value;
            }
        }

        public string SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }

        public double AmountPaid
        {
            get { return amountPaid; }
            set { amountPaid = value; }
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

        public MOBAddress Address
        {
            get
            {
                return this.address;
            }
            set
            {
                this.address = value;
            }
        }
        public MOBFormofPayment FormOfPayment
        {
            get { return formOfPayment; }
            set { formOfPayment = value; }
        }


        public MOBPayPal PayPal
        {
            get { return payPal; }
            set { payPal = value; }
        }

        private MOBApplePay applePayInfo;

        public MOBApplePay ApplePayInfo
        {
            get { return applePayInfo; }
            set { applePayInfo = value; }
        }

        public MOBMasterpass Masterpass
        {
            get { return masterpass; }
            set { masterpass = value; }
        }
    }
}
