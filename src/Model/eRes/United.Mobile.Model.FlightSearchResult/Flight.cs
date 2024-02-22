namespace United.Mobile.Model.FlightSearchResult
{
    public class Flight
    {
        public decimal Airfare { get; set; }
        public string AirfareDisplayValue { get; set; }
        public string ArrivalDateTime { get; set; }
        public string ArrivalDateTimeGMT { get; set; }
        public string Cabin { get; set; }
        public string ConnectTimeMinutes { get; set; }
        public string DepartDate { get; set; }
        public string DepartureDateTime { get; set; }
        public string DepartTime { get; set; }
        public string DepartureDateTimeGMT { get; set; }
        public string Destination { get; set; }
        public string DestinationDescription { get; set; }
        public string DestinationCity { get; set; }
        public string DestinationDate { get; set; }
        public string DestinationTime { get; set; }
        public EquipmentDisclosure EquipmentDisclosures { get; set; }
        public string FlightId { get; set; }
        public string FlightNumber { get; set; }
        public bool IsConnection { get; set; }
        public bool IsStopOver { get; set; }
        public string MarketingCarrier { get; set; }
        public string MarketingCarrierDescription { get; set; }
        public string Meal { get; set; }
        public string OperatingCarrier { get; set; }
        public string OperatingCarrierDescription { get; set; }
        public string Origin { get; set; }
        public string OriginDescription { get; set; }
        public string OriginCity { get; set; }
        public string OvernightConnection { get; set; }
        public string PSCost { get; set; }
        public int SeatsRemaining { get; set; }
        public SegmentPBT SegmentPBT { get; set; }
        public string TravelTime { get; set; }
        public string TotalTravelTime { get; set; }
        public bool ShowFlightStatus { get; set; }

    }
}