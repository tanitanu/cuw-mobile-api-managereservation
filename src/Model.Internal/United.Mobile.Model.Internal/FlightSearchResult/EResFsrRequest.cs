using System.Collections.Generic;
using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Internal.FlightSearchResult
{

    public class EResFsrRequest:EResBaseRequest
    {
        public List<Trip> Trips { get; set; }
        public string ReturnDate { get; set; } = null;
        public string EmergencyNature { get; set; } = null;
        public string MaxConnections { get; set; }
        public string MaxNumberOfTrips { get; set; }
        public string TravelTypeCode { get; set; } = null;
        public string QualifiedEmergency { get; set; } = null;        
        public string BookingSessionID { get; set; } = null;
        public int NumberOfTrips { get; set; }
        public int TripIndex { get; set; }
        public bool ShowBuckets { get; set; }
        public long BookingSessionDetailID { get; set; }
        public string PassClass { get; set; }
        public bool IsAuthorizedTravelTypePNR { get; set; } = false;
        public string CurrentDepartureDate { get; set; }
        public bool RequestAdditionalFlights { get; set; } = false;
        public bool IncludeSSRUMNR { get; set; } = false;
        public bool InclusiveStops { get; set; } = false;
    }
}


