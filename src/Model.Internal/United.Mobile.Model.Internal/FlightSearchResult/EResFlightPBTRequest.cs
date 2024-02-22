using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Internal.FlightSearchResult
{
    public class EResFlightPBTRequest
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string FlightNumber { get; set; }
        public string FlightDate { get; set; }
        public string CarrierCode { get; set; }
        public string SessionId { get; set; }
    }

}
