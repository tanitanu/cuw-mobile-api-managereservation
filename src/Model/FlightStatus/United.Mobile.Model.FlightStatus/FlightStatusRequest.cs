using United.Mobile.Model.Common;

namespace United.Mobile.Model.FlightStatus
{
    public class FlightStatusRequest : MOBRequest
    {
        public int FlightNumber { get; set; }

        public string FlightDate { get; set; }

        public string Origin { get; set; }

        public string CarrierCode { get; set; }

        public string Destination { get; set; }

        public string CurrentFlightShipId { get; set; }
        public string ViewedFlightScheduledDepartureTimeGMT { get; set; }
    }
}