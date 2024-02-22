
namespace United.Mobile.Model.SeatMap
{
    public class Segment : CouchbaseDocument
    {
        public Airline MarketingCarrier { get; set; }
        public Airline OperatingCarrier { get; set; }
        public string FlightNumber { get; set; }
        public string FlightDate { get; set; }
        public Airport Departure { get; set; }
        public Airport Arrival { get; set; }
        public string ScheduledDepartureDateTime { get; set; }
        public string ScheduledDepartureDateTimeUtc { get; set; }
        public string ScheduledArrivalDateTime { get; set; }
        public string ScheduledArrivalDateTimeUtc { get; set; }
        public string EstimatedDepartureDateTime { get; set; }
        public string EstimatedArrivalDateTime { get; set; }
    }
}
