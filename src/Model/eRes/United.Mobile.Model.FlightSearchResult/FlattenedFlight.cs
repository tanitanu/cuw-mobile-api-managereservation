using System.Collections.Generic;

namespace United.Mobile.Model.FlightSearchResult
{
    public class FlattenedFlight
    {
        public string TripId { get; set; }
        public string FlightId { get; set; }
        public string CabinMessage { get; set; }
        public string ProductId { get; set; }
        public List<Flight> Flights { get; set; }
        public string TripDays { get; set; }
        public bool CovidVaccineIndicator { get; set; }
    }
}
