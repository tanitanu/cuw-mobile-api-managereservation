using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class Segment
    {
        public Airline MarketingCarrier { get; set; }
        public string FlightNumber { get; set; } = string.Empty;
        public MOBAirport Departure { get; set; }
        public MOBAirport Arrival { get; set; }
        public string ScheduledDepartureDateTime { get; set; } = string.Empty;
        public string ScheduledArrivalDateTime { get; set; } = string.Empty;
        public string ScheduledDepartureTimeGMT { get; set; } = string.Empty;
        public string ScheduledArrivalTimeGMT { get; set; } = string.Empty;
        public string FormattedScheduledDepartureDateTime { get; set; } = string.Empty;
        public string FormattedScheduledArrivalDateTime { get; set; } = string.Empty;
        public string FormattedScheduledDepartureDate { get; set; } = string.Empty;
        public string FormattedScheduledArrivalDate { get; set; } = string.Empty;
        public string FormattedScheduledDepartureTime { get; set; } = string.Empty;
        public string FormattedScheduledArrivalTime { get; set; } = string.Empty;
        public Segment()
        {
        }
    }
}
