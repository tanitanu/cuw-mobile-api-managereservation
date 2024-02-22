using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class ShoppingTripFareTypeDetailsRequest : MOBRequest
    {
        private string sessionId;
        private string cartID;

        public string SessionId
        {
            get
            {
                return sessionId;
            }
            set
            {
                sessionId = value;
            }
        }

        public string CartID
        {
            get
            {
                return cartID;
            }
            set
            {
                cartID = value;
            }
        }

    }
}
