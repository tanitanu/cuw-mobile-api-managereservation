using System.Collections.Generic;
using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Internal.PassRiders
{
    public class EmpBookingPassRider
    {        
        public List<DropOption> ClassOfService { get; set; }
        public List<CustomizedRoute> CustomizedRoutes { get; set; }
        public string DisplayName { get; set; }
        public List<DropOption> JumpSeatType { get; set; }
        public List<DropOption> PassType { get; set; }        
        public string PaxId { get; set; }        
        public bool PrimaryFriend { get; set; }
        public RelationShip Relationship { get; set; }        
        public int SortOrder { get; set; }
        public List<SSRInfo> SSRs { get; set; }
        public DayOfContactInformation DayOfContactInformation { get; set; }
        public int Age { get; set; } 
        public List<int> TripIds { get; set; }
        public bool isSameMetalTravel{ get; set; }

        public string Gender { get; set; } = string.Empty;
        public string Nationality { get; set; }
        public string Country { get; set; }
    }
}
