using System;

namespace United.Mobile.Model.FligtStatus.Internal
{
    [Serializable()]
    public class FlightStatusRequestParameter
    {
        public string ArrivalAirport { get; set; }

        public string DepartureAirport { get; set; }

        public string DepartureDate { get; set; }

        public string FlightNumber { get; set; }

    }
}
