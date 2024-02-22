using System.Collections.Generic;

namespace United.Mobile.Model.FlightSearchResult
{
    public class ShopTrip
    {
        public string Origin { get; set; }
        public string OriginDecoded { get; set; }
        public string DepartDate { get; set; }
        public string Destination { get; set; }
        public string DestinationDecoded { get; set; }
        public string ArrivalDate { get; set; }
        public string TripId { get; set; }
        public List<FlattenedFlight> FlattenedFlights { get; set; }
        public string FlightDateChangeMessage { get; set; }
        public string DepartTime { get; set; }
        public string DestinationTime { get; set; }
        public string TotalTravelTime { get; set; }

    }
}