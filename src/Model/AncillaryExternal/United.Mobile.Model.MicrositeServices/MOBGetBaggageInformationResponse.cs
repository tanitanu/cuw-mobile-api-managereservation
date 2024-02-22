using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.MicrositeServices
{
    [Serializable()]
    public class MOBGetBaggageInformationResponse : MOBResponse
    {
        public List<MOBBaggageDetail> BaggageDetails { get; set; }
        public string CheckoutResourceUrl { get; set; } = string.Empty;
        public List<MOBSection> BaggageContentList { get; set; }

        public MOBGetBaggageInformationResponse()
        {
            BaggageDetails = new List<MOBBaggageDetail>();
            BaggageContentList = new List<MOBSection>();
        }
    }
}
