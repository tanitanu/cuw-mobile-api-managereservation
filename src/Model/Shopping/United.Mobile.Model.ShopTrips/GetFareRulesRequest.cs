using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class GetFareRulesRequest : MOBRequest
    {
        private string sessionId = string.Empty;
        private string cartId = string.Empty;

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
            get
            {
                return this.cartId;
            }
            set
            {
                this.cartId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
    }
}
