using System;

namespace United.Mobile.Model.Shopping
{

    [Serializable()]
    public class PointOfSale
    {
        public string WebShareToken { get; set; } = string.Empty;

        public string WebSessionShareUrl { get; set; } = string.Empty;

        public string CountryNotSupportedMessage { get; set; } = string.Empty;

        public string RedirectUrl { get; set; } = string.Empty;

    }
}
