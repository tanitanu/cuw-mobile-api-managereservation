using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.FlightSearchResult
{
   public class SaveBookingTrip
   {
        public List<SaveTripBookSegment> BookingSegment { get; set; }
        public int Days { get; set; }
        public string TotalJourneyDuration { get; set; }
        public int TripID { get; set; }      
    }
}
