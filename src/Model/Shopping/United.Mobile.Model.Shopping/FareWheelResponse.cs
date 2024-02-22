using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class FareWheelResponse : MOBResponse
    {
        public SelectTripRequest FareWheelRequest { get; set; }
        public string CartId { get; set; } = string.Empty;

        public List<MOBSHOPFareWheelItem> FareWheel { get; set; }
        public string CallDurationText { get; set; }

        public bool DisablePricingBySlice { get; set; }
        public List<MOBFSRAlertMessage> FSRAlertMessages { get; set; }
        public int NumberPeopleViewingFlights { get; set; }
        
    }
}
