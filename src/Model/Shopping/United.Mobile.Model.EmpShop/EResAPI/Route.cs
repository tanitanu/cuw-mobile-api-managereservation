using System.Collections.Generic;

namespace United.Mobile.Model.Common
{
    public class Route
    {
        public List<Segment> Segments { get; set; }
        public string TripNumber { get; set; }
        public int Days { get; set; }
        public string TotalJourneyDuration { get; set; }
    }
}
