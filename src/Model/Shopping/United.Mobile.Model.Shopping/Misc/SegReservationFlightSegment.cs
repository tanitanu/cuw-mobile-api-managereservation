using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class SegReservationFlightSegment
    {
        public ComBookingClass BookingClass { get; set; }
        public SegFlightSegment FlightSegment { get; set; }
        public string IsConnection { get; set; } = string.Empty;
        public string OtherAirlineRecordLocator { get; set; } = string.Empty;
    }
}
