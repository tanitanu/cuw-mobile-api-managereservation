using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Internal.Common
{
    public class PassRider
    {
        public PassRiderExtended PRExtended { get; set; }        
        public RelationShip Relationship { get; set; }
        public string FirstName { get; set; }        
        public string MiddleName { get; set; }
        public string LastName { get; set; }        
        public string NameSuffix { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public bool UnaccompaniedFirst { get; set; }
        public bool MustUseCurrentYearPasses { get; set; } 
        public string DependantID { get; set; }
        public string FirstBookingBuckets { get; set; } 
        public bool PrimaryFriend { get; set; } 
        public List<SSRInfo> SSRs { get; set; }       
        public DayOfContactInformation DayOfContactInformation { get; set; }
        public DayOfTravelNotification DayOfTravelNotification { get; set; }
        public int SortOrder { get; set; }
    }
}
