using System.Collections.Generic;
using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Internal.FlightSearchResult
{
    public class EResFsrResponse : EResBaseResponse
    {
        public List<EResAlert> Alerts { get; set; }
        public AvailableRoute AvailableRoutes { get; set; }
        public string FlightMessage { get; set; }
        public EResFsrRequest FlightSearchRequest { get; set; }
        public bool LastTrip { get; set; }
        public int CurrentTrip { get; set; }
        public string NearbyOriginAirports { get; set; }
        public string NearbyDestinationAirports { get; set; }
        public bool ShowUADiscountFareWheel { get; set; }
    }
}
