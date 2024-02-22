using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common.Shopping;

namespace United.Mobile.Model.Shopping.FormofPayment
{
    [Serializable()]
    public class FutureFlightCreditRequest : ShoppingRequest
    {
        private string recordLocator;
        private string lastName;
        private string email;
        //private string mileagePlusNumber = string.Empty;
        //private string hashPinCode = string.Empty;
        private string sessionId = string.Empty;

        public string Email { get { return email; } set { email = value; } }
        public string RecordLocator { get { return recordLocator; } set { recordLocator = value; } }
        public string LastName { get { return lastName; } set { lastName = value; } }
        //public string HashPinCode { get { return this.hashPinCode; } set { this.hashPinCode = value; } }
        //public string MileagePlusNumber { get { return mileagePlusNumber; } set { mileagePlusNumber = value; } }
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

    }

}
