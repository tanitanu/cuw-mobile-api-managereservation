using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class SegFlightLeg
    {
        public TMAAirport ArrivalAirport { get; set; }
        public int CabinCount { get; set; }
        public TMAAirport DepartureAirport { get; set; }
        public MOBComAircraft Equipment { get; set; }
    }
}
