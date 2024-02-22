using System;
using System.Collections.Generic;

namespace United.Mobile.Model.CodeTable
{
    [Serializable]
    public class MOBCarriersDetailsResponse
    {
        public List<MOBCarriersDetails> CarriersDetails { get; set; }
        public MOBCarriersDetailsResponse()
        {
            CarriersDetails = new List<MOBCarriersDetails>();
        }
    }
    [Serializable]
    public class MOBCarriersDetails
    {
        public string CarrierCode { get; set; } = string.Empty;
        public string CarrierShortName { get; set; } = string.Empty;
        public string CarrierFullName { get; set; } = string.Empty;
        public string CarrierImageSrc { get; set; } = string.Empty;
        public string CarrierImageName { get; set; } = string.Empty;

    }
}
