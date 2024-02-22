using System.Collections.Generic;

namespace United.Mobile.Model.Internal.HomePageContent
{
    public class MOBTravelType
    {
        public string TravelCode { get; set; }
        public string TravelDescription { get; set; }
        public EmergencyInvolve[] EmergencyInvolves { get; set; }
        public List<NatureOfEmergency> NatureOfEmergency { get; set; }
    }
}
