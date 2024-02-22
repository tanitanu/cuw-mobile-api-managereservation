using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Internal.CompleteBooking
{
    public class BookingPNRSegment
    {
        public Airport DepartureAirport { get; set; }
        public FlightDateTime DepartureDateTime { get; set; }
        public Airport ArrivalAirport { get; set; }
        public FlightDateTime ArrivalDateTime { get; set; }
        public TypeOption Cabin { get; set; }
        public string FlightNumber { get; set; }
        public string CarrierCode { get; set; }
        public string PassClass { get; set; }
        public Aircraft Aircraft { get; set; }
        public TravelTime ConnectTime { get; set; }
        public TravelTime TravelTime { get; set; }
        public PositiveSpaceCost PSCost { get; set; }
        public int TripNumber { get; set; }
        public bool ShowCheckIn { get; set; }
    }
}