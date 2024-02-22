using System.Collections.Generic;
using United.Mobile.Model.Internal.Common;
using United.Mobile.Model.Internal.EmployeeProfile;

namespace United.Mobile.Model.Internal.SaveTrip
{
    public class EmpBookingPassenger
    {
        public EmpTCDInfo TcdInfo { get; set; }
        public EmpBookingLapChild LapChild { get; set; }
        public string PassengerId { get; set; }
        public string Index { get; set; }
        public int Age { get; set; }
        public EmpName Name { get; set; }
        public TypeOption Cabin { get; set; }
        public PassType PassType { get; set; }
        public List<int> SegmentIndexes { get; set; }
        public List<SpecialService> SpecialService { get; set; }
        public Common.RelationShip Relationship { get; set; }
        public EmpPaxPrice PaxPrice { get; set; }
        public bool IsPrimaryFriend { get; set; }
        public int SortOrder { get; set; }
        public int TravelerNumber { get; set; }
    }

}
