using System;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBProfileRequest : MOBRequest
    {
        public string SessionId { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;

        public string MileagePlusNumber { get; set; } = string.Empty;

        public int CustomerId { get; set; }
        public string SessionID { get; set; } = string.Empty;
        public string PinCode { get; set; } = string.Empty;
        public bool IncludeSecureTravelers { get; set; }
        public bool IncludeAddresses { get; set; }
        public bool IncludePhones { get; set; }
        public bool IncludeEmails { get; set; }
        public bool IncludePaymentInfos { get; set; }


    }
}
