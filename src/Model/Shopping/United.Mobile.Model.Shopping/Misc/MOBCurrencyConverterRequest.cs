using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class CurrencyConverterRequest : MOBRequest
    {
        private long sessionID;
        private string fromCurrencyCode = string.Empty;
        private string toCurrencyCode = string.Empty;
        private double amount;
        private string messageFormat = string.Empty;

        public long SessionID
        {
            get
            {
                return this.sessionID;
            }
            set
            {
                this.sessionID = value;
            }
        }

        public string FromCurrencyCode
        {
            get
            {
                return this.fromCurrencyCode;
            }
            set
            {
                this.fromCurrencyCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ToCurrencyCode
        {
            get
            {
                return this.toCurrencyCode;
            }
            set
            {
                this.toCurrencyCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public double Amount
        {
            get
            {
                return this.amount;
            }
            set
            {
                this.amount = value;
            }
        }

        public string MessageFormat
        {
            get
            {
                return this.messageFormat;
            }
            set
            {
                this.messageFormat = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
