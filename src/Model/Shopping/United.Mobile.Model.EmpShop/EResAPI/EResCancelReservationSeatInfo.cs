using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    public class EResCancelReservationSeatInfo
    {
        public string Attribute { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public string FlightDate { get; set; } = string.Empty;
        public string FlightNumber { get; set; } = string.Empty;
        public int SegmentNumber { get; set; } = 0;
        public string Origin { get; set; } = string.Empty;
        public string SeatAssignment { get; set; } = string.Empty;
        public string TravelerSharesIndex { get; set; } = string.Empty;
        public string SeatNumber { get; set; } = string.Empty;

    }
}
