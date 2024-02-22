using System.Collections.Generic;
using United.Mobile.Model.Internal.Common;
using United.Mobile.Model.Internal.PassRiders;

namespace United.Mobile.Model.Internal.Booking
{
    public class BookingPassenger
    {
        public int Age { get; set; }
        public TypeOption Cabin { get; set; }
        public string DateOfBrith { get; set; }
        public DayOfContactInformation DayofContact { get; set; }
        public string FirstName { get; set; }
        public bool IsInDomesticSegment { get; set; }
        public bool IsInMexicoTrip { get; set; }
        public BookingLapChild LapChild { get; set; }
        public string LastName { get; set; }
        public string Level1 { get; set; }
        public string MiddleName { get; set; }
        public string NameSuffix { get; set; }
        public string PassClass { get; set; }
        public string PassengerId { get; set; }
        public PassengerPrice PassengerPrice { get; set; }
        public PassType PassType { get; set; }
        public object Price { get; set; }
        public string PricingTravelerType { get; set; }
        public bool PrimaryFriend { get; set; }
        public RelationShip RelationShip { get; set; }
        public object Seats { get; set; }
        public List<int> SegmentIndexes { get; set; }
        public string SeniorityDate { get; set; }
        public int SortOrder { get; set; }
        public List<SpecialService> SpecialService { get; set; }
       //public List<SpecialService> SpecialService { get; set; }
        public List<SSRInfo> SSRInfo { get; set; }
        public string Status { get; set; }
        public string TravelerKeyIndex { get; set; }
        public List<int> TripIds { get; set; }
    }
}
