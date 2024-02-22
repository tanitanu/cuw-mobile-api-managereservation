using System;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBFlightSegment
    {
        public string ArrivalAirport { get; set; } 

        public string ArrivalAirportName { get; set; }

        public string CarrierCode { get; set; } 

        public string DepartureAirport { get; set; } 

        public string DepartureAirportName { get; set; }

        public string DepartureDate { get; set; } = string.Empty;

        public string FlightNumber { get; set; } = string.Empty;
    }
}
