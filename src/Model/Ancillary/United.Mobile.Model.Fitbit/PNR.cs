using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Fitbit
{
    [Serializable]
    public class PNR
    {

        public string RecordLocator { get; set; } = string.Empty;
        public string DateCreated { get; set; } = string.Empty;
        public string FlightDate { get; set; } = string.Empty;
        public Airport Origin { get; set; }
        public Airport Destination { get; set; }
        public string CheckInStatus { get; set; } = string.Empty;
        public string NumberOfPassengers { get; set; } = string.Empty;
        public string LastSegmentArrivalDate { get; set; } = string.Empty;
        public string ExpirationDate { get; set; } = string.Empty;
        public string LastUpdated { get; set; } = string.Empty;
        public List<PNRSegment> Segments { get; set; }
        public string FarelockExpirationDate { get; set; } = string.Empty;
        public PNR()
        {
            Segments = new List<PNRSegment>();
        }
    }
}
