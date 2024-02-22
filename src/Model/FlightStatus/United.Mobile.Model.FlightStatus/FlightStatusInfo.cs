using System;
using System.Collections.Generic;

namespace United.Mobile.Model.FlightStatus
{
    [Serializable]
    public class FlightStatusInfo
    {
        public string CarrierCode { get; set; } = string.Empty;

        public string FlightNumber { get; set; } = string.Empty;

        public string CodeShareflightNumber { get; set; } = string.Empty;

        public string FlightDate { get; set; } = string.Empty;

        public List<FlightStatusSegment> Segments { get; set; }

        public AirportAdvisoryMessage AirportAdvisoryMessage { get; set; }
        public FlightStatusInfo()
        {
            Segments = new List<FlightStatusSegment>();
        }
    }
}
