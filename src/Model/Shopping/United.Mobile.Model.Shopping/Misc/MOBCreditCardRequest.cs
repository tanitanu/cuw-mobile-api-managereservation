using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class CreditCardRequest : MOBRequest
    {
        private string sessionId = string.Empty;
        private string mileagePlusNumber = string.Empty;
        private int customerId;
        private string hashPinCode = string.Empty;

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

        public string MileagePlusNumber
        {
            get
            {
                return this.mileagePlusNumber;
            }
            set
            {
                this.mileagePlusNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public int CustomerId
        {
            get
            {
                return this.customerId;
            }
            set
            {
                this.customerId = value;
            }
        }

        public string HashPinCode
        {
            get
            {
                return this.hashPinCode;
            }
            set
            {
                this.hashPinCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
