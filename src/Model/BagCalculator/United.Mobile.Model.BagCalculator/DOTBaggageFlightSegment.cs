using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.BagCalculator
{
    [Serializable]
    public class DOTBaggageFlightSegment
    {
        public string DepartureAirportCode { get; set; } = string.Empty;
        public string ArrivalAirportCode { get; set; } = string.Empty;
        public DateTime DepartureDate { get; set; }
        public string DepartureDateString { get; set; } = string.Empty;
        public string OperatingAirline { get; set; } = string.Empty;
        public string MarketingAirline { get; set; } = string.Empty;
        public string SegmentNumber { get; set; } = string.Empty;
        public string ClassOfService { get; set; } = string.Empty;
    }
}
