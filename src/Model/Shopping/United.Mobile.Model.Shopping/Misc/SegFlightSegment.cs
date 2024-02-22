using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class SegFlightSegment
    {
        public MOBTMAAirport ArrivalAirport { get; set; }
        public string ArrivalDateTime { get; set; } = string.Empty;
        public List<ComBookingClass> BookingClasses { get; set; }
        public MOBTMAAirport DepartureAirport { get; set; }
        public string DepartureDateTime { get; set; } = string.Empty;
        public int Distance { get; set; }
        public MOBComAircraft Equipment { get; set; }
        public string FlightNumber { get; set; } = string.Empty;
        public int GroundTime { get; set; }
        public string IsChangeOfGauge { get; set; } = string.Empty;
        public string IsInternational { get; set; } = string.Empty;
        public int JourneyDuration { get; set; }
        public List<SegFlightLeg> Legs { get; set; }
        public List<ComMessage> Messages { get; set; }
        public int NumberOfStops { get; set; }
        public double OnTimeRate { get; set; }
        public string OperatingAirlineCode { get; set; } = string.Empty;
        public string OperatingAirlineName { get; set; } = string.Empty;
        public int ScheduledFlightDuration { get; set; }
        public int SegmentNumber { get; set; }
        public List<MOBTMAAirport> StopLocations { get; set; }
        public SegFlightSegment()
        {
            BookingClasses = new List<ComBookingClass>();
            Legs = new List<SegFlightLeg>();
            Messages = new List<ComMessage>();
            StopLocations = new List<MOBTMAAirport>();

        }
    }
}
