using System.Collections.Generic;
using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Internal.PassRiders
{
    public class EmpPassRidersResponse : EResBaseResponse
    {       
        public PassRidersRequest BookingPassRiderListRequest { get; set; }        
        public List<EmpBookingPassRider> BookingPassRiders { get; set; }
        public List<BookingTrip> BookingTrips { get; set; }
        public bool BuddiesAllowed { get; set; }
        public bool OnlyFamilyBuddies { get; set; }
        public List<PassRider> SuspendedPassRider { get; set; }        
        public bool WorkingCrewMember { get; set; }        
        public string TravelDocToolTip { get; set; }
        public string UnaccompaniedPickDropMessage { get; set; }
        public EmpPassRidersResponse()
        {
            BookingPassRiders = new List<EmpBookingPassRider>();
            BookingTrips = new List<BookingTrip>();
            SuspendedPassRider = new List<PassRider>();
        }
    }
}
