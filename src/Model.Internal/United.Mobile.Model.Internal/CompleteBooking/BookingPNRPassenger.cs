using System.Collections.Generic;
using United.Mobile.Model.Internal.Booking;
using United.Mobile.Model.Internal.Common;
using United.Mobile.Model.Internal.PassRiders;
namespace United.Mobile.Model.Internal.CompleteBooking
{
    public class BookingPNRPassenger
    {
        public DayOfContactInformation DayofContact { get; set; }
        public TypeOption Cabin { get; set; }
        public List<SpecialService> SpecialService { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string NameSuffix { get; set; }
        public PassType TravelerType { get; set; }
        public string PassClass { get; set; }
        public RelationShip Relationship { get; set; }
        public List< SSRInfo > SSRInfo { get; set; }
        public PassengerPrice PassengerPrice { get; set; }
        public BookingLapChild LapChild { get; set; }
        public string PassengerId { get; set; }
        public PassType PassType { get; set; }
    }  
}