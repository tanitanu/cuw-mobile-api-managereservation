using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.Internal.Common;
using EResAlert = United.Mobile.Model.Internal.Common.EResAlert;

namespace United.Mobile.Model.FlightSearchResult
{
    public class FlightSearchResultResponse: ResponseBase
    {
        public List<string> Disclaimer { get; set; }
        public int TripIndex { get; set; }
        public string CartId { get; set; } = string.Empty;
        public string EResTransactionId { get; set; }
        public string FlightMessage { get; set; }
        public bool LastTrip { get; set; }
        public string RefreshResultsData { get; set; } = string.Empty;
        public string EResSessionId { get; set; }
        public string SessionId { get; set; }
        public string BookingSessionId { get; set; }
        public Availability Availability { get; set; }
        public List<EResAlert> Alerts{ get; set; }
        public FlightSearchResultRequest FlightSearchResultRequest { get; set; }
        public List<SaveBookingTrip> BookingTrips { get; set; }
        public string EmployeeId { get; set; }
        public string[] BoardingLegends { get; set; }
    }
}
