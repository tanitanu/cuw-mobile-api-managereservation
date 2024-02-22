using System.Collections.Generic;

namespace United.Mobile.Model.Internal.Common
{
    public class BookingTrip
    {
        public List<BookingSegment> BookingSegment { get; set; }
        public int Days { get; set; }
        public string TotalJourneyDuration { get; set; } 
        public int TripID { get; set; }
        public BookingTrip()
        {
            BookingSegment = new List<BookingSegment>();
        }
    }
}
