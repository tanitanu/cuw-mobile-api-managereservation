using System.Collections.Generic;

namespace United.Mobile.Model.Internal.PassRiders
{
    public class CustomizedRoute
    {        
        public List<DropOption> ClassOfService { get; set; }
        public string Destination { get; set; } 
        public string FlightDate { get; set; } 
        public string FlightNumber { get; set; }
        public string MarketingCarrier { get; set; } 
        public string Origin { get; set; } 
        public List<DropOption> PassType { get; set; }
        public int SegmentIndex { get; set; }

    }
}
