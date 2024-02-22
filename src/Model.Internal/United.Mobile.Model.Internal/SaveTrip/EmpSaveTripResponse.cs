using System.Collections.Generic;
using United.Mobile.Model.Internal.Common;
using United.Mobile.Model.Internal.FlightSearchResult;

namespace United.Mobile.Model.Internal.SaveTrip
{
    public class EmpSaveTripResponse
    {
        public bool IsAllowedToSelectFlight { get; set; }
        public BookingSaveTripRequest BookingSaveTripRequest { get; set; }
        public bool LastTrip { get; set; }
        public int CurrentTrip { get; set; }
        public AvailableRoute AvailableRoutes { get; set; }
        public EmpBookingInfo BookingInfo { get; set; }
        public bool IsChangeSegment { get; set; }
        public string DisplayMessage { get; set; }
        public List<EResAlert> Alerts { get; set; }
        public EResFsrRequest FlightSearchRequest { get; set; }
        public string NearbyOriginAirports { get; set; }
        public string NearbyDestinationAirports { get; set; }
        public bool ShowUADiscountFareWheel { get; set; }
        public ErrorInfo Error { get; set; }
        public string LastCallDateTime { get; set; }
        public string ServerName { get; set; }
        public string Status { get; set; }
        public string TransactionID { get; set; }
        public EResAlert BaseAlert { get; set; }
        public string TransferMessage { get; set; }
    }

}
