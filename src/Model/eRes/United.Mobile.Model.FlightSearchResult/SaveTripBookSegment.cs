namespace United.Mobile.Model.FlightSearchResult
{
    public class SaveTripBookSegment
    {
        public string DepartureDate { get; set; }
        public string DepartTime { get; set; }
        public string DepartureCityCode { get; set; }
        public string ArrivalDate { get; set; }
        public string ArrivalTime { get; set; }
        public string ArrivalCityCode { get; set; }
        public string Duration { get; set; }
        public int NumOfStops { get; set; }
        public int DepartureGMTOfffsetMinutes { get; set; }
        public int ArrivalGMTOfffsetMinutes { get; set; }
        public string MarketingCarrier { get; set; }
        public string FlightNumber { get; set; }
        public string DepartureDateTimeGMT { get; set; }
        public string ArrivalDateTimeGMT { get; set; }
        public string OriginDescription { get; set; }
        public string DestinationDescription { get; set; }

    }
}
