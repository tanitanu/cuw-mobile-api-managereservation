using System;

namespace United.Mobile.Model.UpgradeCabin
{
    [Serializable()]
    public class MOBUpgradePlusPointWebMyTripResponse : MOBResponse
    {
        private string sessionId;
        private string redirectUrl;
        private Boolean isEligible;
        private string webShareToken = string.Empty;
        private string webSessionShareUrl = string.Empty;

        public string SessionId { get { return this.sessionId; } set { this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public Boolean IsEligible { get { return this.isEligible; } set { this.isEligible = value; } }
        public string RedirectUrl { get { return this.redirectUrl; } set { this.redirectUrl = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public string WebShareToken { get { return this.webShareToken; } set { this.webShareToken = value; } }
        public string WebSessionShareUrl { get { return this.webSessionShareUrl; } set { this.webSessionShareUrl = value; } }
    }
}
