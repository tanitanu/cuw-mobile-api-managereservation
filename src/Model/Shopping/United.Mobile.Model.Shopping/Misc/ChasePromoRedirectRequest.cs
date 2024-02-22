using System;
using System.Xml.Serialization;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    [XmlRoot("MOBSHOPChasePromoRedirectRequest")]

    public class ChasePromoRedirectRequest: MOBRequest
    {
        public string SessionId { get; set; } = string.Empty;
        public string PromoType { get; set; } = string.Empty;
        public string MileagePlusNumber { get; set; } = string.Empty;
        public string HashPinCode { get; set; } = string.Empty;
    }
}