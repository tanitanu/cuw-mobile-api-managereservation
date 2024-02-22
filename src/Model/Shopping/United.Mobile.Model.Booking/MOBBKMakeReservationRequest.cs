using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBBKMakeReservationRequest : MOBRequest
    {
        private string sessionId = string.Empty;
        private MOBBKFormOfPayment formOfPayment;
        private double paymentAmount;
        private MOBAddress address;
        private string emailAddress = string.Empty;

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

        public MOBBKFormOfPayment FormOfPayment
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
    }
}
