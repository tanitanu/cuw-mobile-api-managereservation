using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class ChangeAddressRequest : MOBRequest
    {
        private string sessionId;
        private MOBAddress mobAddress;
        private MOBEmail mobEmail;
        private string cartId = string.Empty;
        private string flow = string.Empty;
        public MOBAddress MobAddress
        {
            get
            {
                return mobAddress;
            }
            set
            {
                mobAddress = value;
            }
        }

        public MOBEmail MobEmail
        {
            get
            {
                return mobEmail;
            }
            set
            {
                mobEmail = value;
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
    }
}
