using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.MicrositeServices
{
    [Serializable()]
    public class MOBGetBaggageInformationRequest : MOBRequest
    {
        
        public bool IsBooking { get; set; }

        public string SessionId { get; set; } = string.Empty;

        public string RecordLocator { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

    }
}
