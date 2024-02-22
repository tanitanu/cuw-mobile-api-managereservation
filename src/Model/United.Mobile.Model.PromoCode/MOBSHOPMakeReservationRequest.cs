using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.PromoCode
{
    [Serializable()]
    public class MOBSHOPMakeReservationRequest : MOBRequest
    {
        private string sessionId = string.Empty;
        private MOBSHOPFormOfPayment formOfPayment;
        private double paymentAmount;
        private MOBAddress address;
        private string emailAddress = string.Empty;
        private MOBCPPhone phone;
        //private MOBSHOPPhone phone;
        private bool removeFareLock;
        private bool fareLockAutoTicket;
        private double otpAmount;
        private string mileagePlusNumber = string.Empty;
        private string additionalData = string.Empty;
        private string token = string.Empty;
        private bool isTPI = false;
        private bool isSecondaryPayment = false;

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

        public bool RemoveFareLock
        {
            get
            {
                return this.removeFareLock;
            }
            set
            {
                this.removeFareLock = value;
            }
        }

        public bool FareLockAutoTicket
        {
            get
            {
                return this.fareLockAutoTicket;
            }
            set
            {
                this.fareLockAutoTicket = value;
            }
        }

        public string SessionId
        {
            get
            {
                return this.sessionId;
            }
            set
            {
                this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public MOBSHOPFormOfPayment FormOfPayment
        {
            get
            {
                return this.formOfPayment;
            }
            set
            {
                this.formOfPayment = value;
            }
        }

        public double PaymentAmount
        {
            get
            {
                return this.paymentAmount;
            }
            set
            {
                this.paymentAmount = value;
            }
        }

        public double OTPAmount
        {
            get
            {
                return this.otpAmount;
            }
            set
            {
                this.otpAmount = value;
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

        public string EmailAddress
        {
            get
            {
                return this.emailAddress;
            }
            set
            {
                this.emailAddress = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        //public MOBSHOPPhone Phone
        //{
        //    get
        //    {
        //        return this.phone;
        //    }
        //    set
        //    {
        //        this.phone = value;
        //    }
        //}

        public string Token
        {
            get
            {
                return token;
            }
            set
            {
                this.token = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool IsTPI
        {
            get
            {
                return this.isTPI;
            }
            set
            {
                this.isTPI = value;
            }
        }
        public bool IsSecondaryPayment
        {
            get
            {
                return this.isSecondaryPayment;
            }
            set
            {
                this.isSecondaryPayment = value;
            }
        }
    }
}
