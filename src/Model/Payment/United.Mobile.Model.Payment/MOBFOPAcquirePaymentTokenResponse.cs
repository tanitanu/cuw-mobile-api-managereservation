using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Payment
{
    [Serializable()]
    public class MOBFOPAcquirePaymentTokenResponse : MOBResponse
    {
        public string ObjectName { get; set; } = "United.Definition.MOBFOPAcquirePaymentTokenResponse";
        private string sessionId = string.Empty;
        private string cartId = string.Empty;
        private string flow = string.Empty;
        private string token = string.Empty;
        private string cslSessionId = string.Empty;

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
        public string Token
        {
            get { return token; }
            set { token = value; }
        }
        public string CslSessionId
        {
            get { return cslSessionId; }
            set { cslSessionId = value; }
        }
    }
}
