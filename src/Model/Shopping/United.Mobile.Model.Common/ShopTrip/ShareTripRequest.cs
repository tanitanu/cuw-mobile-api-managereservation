using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class ShareTripRequest : MOBRequest
    {
        private string sessionId = string.Empty;
        private bool overrideFarelockValidation = false;
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
        public bool OverrideFarelockValidation
        {
            get
            {
                return this.overrideFarelockValidation;
            }
            set
            {
                this.overrideFarelockValidation = value;
            }
        }
    }
}
