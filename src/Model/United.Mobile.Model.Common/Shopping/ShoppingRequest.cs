using System;

namespace United.Mobile.Model.Common.Shopping
{
    [Serializable()]
    public class ShoppingRequest : MOBRequest
    {
        private string sessionId = string.Empty;
        private string checkinSessionId = string.Empty;
        private string cartId = string.Empty;
        private string flow = string.Empty;
        private string pointOfSale = string.Empty;
        private string cartKey = string.Empty;

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
        public string CheckinSessionId
        {
            get
            {
                return this.checkinSessionId;
            }
            set
            {
                this.checkinSessionId = value;
            }
        }

        public string CartId
        {
            get
            {
                return this.cartId;
            }
            set
            {
                this.cartId = value;
            }
        }

        public string Flow
        {
            get
            {
                return this.flow;
            }
            set
            {
                this.flow = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
        public string PointOfSale
        {
            get
            {
                return this.pointOfSale;
            }
            set
            {
                this.pointOfSale = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
        public string CartKey
        {
            get
            {
                return this.cartKey;
            }
            set
            {
                this.cartKey = value;
            }
        }
    }
}
