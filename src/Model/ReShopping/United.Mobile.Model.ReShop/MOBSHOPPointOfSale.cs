using System;

namespace United.Mobile.Model.ReShop
{

    [Serializable()]
    public class MOBSHOPPointOfSale
    {
        private string redirectUrl;
        private string countryNotSupportedMessage;
        private string webShareToken = string.Empty;
        private string webSessionShareUrl = string.Empty;

        public string WebShareToken { get { return this.webShareToken; } set { this.webShareToken = value; } }
        public string WebSessionShareUrl { get { return this.webSessionShareUrl; } set { this.webSessionShareUrl = value; } }

        public string CountryNotSupportedMessage
        {
            get { return countryNotSupportedMessage; }
            set { countryNotSupportedMessage = value; }
        }

        public string RedirectUrl
        {
            get { return redirectUrl; }
            set { redirectUrl = value; }
        }
    }
}
