using System;

namespace United.Mobile.Model.HomePageContent
{
    public class EmpTravelTypeItem
    {
        public  bool IsEligible { get; set; }
        public bool IsAuthorizationRequired { get; set; }
        public int NumberOfTravelers { get; set; }
        public string TravelType { get; set; }
        public string TravelTypeDescription { get; set; }
        public string Advisory { get; set; }
        public int AdvanceBookingDays { get; set; }
    }
}
