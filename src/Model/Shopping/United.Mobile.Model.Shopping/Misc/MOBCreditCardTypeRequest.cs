using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class CreditCardTypeRequest: MOBRequest
    {
        private string sessionId;
        private string cardNumber;
        private int cardLength;

        public string SessionId
        {
            get
            {
                return this.sessionId;
            }
            set
            {
                this.sessionId = value;
            }
        }
        public string CardNumber
        {
            get
            {
                return this.cardNumber == null ? "": this.cardNumber;
            }
            set
            {
                this.cardNumber = value;
            }
        }
        public int CardLength
        {
            get
            {
                return this.cardLength;
            }
            set
            {
                this.cardLength = value;
            }
        }

    }
}
