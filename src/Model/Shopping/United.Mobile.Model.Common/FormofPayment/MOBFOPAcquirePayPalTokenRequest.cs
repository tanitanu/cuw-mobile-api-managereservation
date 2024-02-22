using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.FormofPayment
{
    [Serializable()]
    public class FOPAcquirePayPalTokenRequest : MOBRequest
    {
        private string sessionId = string.Empty;
        private double amount = 0.0;
        private string key = string.Empty;
        private string countryCode = string.Empty;
        private string cartId = string.Empty;
        private string flow = string.Empty;

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

        public double Amount
        {
            get
            {
                return amount;
            }
            set
            {
                this.amount = value;
            }
        }
        public string CountryCode
        {
            get
            {
                return countryCode;
            }
            set
            {
                this.countryCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string Key
        {
            get
            {
                return key;
            }
            set
            {
                this.key = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
    
}

