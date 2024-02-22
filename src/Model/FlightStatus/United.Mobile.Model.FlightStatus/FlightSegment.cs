using System;

namespace United.Mobile.Model.FlightStatus
{
    [Serializable]
    public class FlightSegment
    {
        public string ArrivalAirport { get; set; }
        public string ArrivalAirportName { get; set; }
        public string CarrierCode { get; set; }
        public string DepartureAirport { get; set; }
        public string DepartureAirportName { get; set; }
        public string DepartureDate { get; set; }
        public string FlightNumber { get; set; }

    }
}