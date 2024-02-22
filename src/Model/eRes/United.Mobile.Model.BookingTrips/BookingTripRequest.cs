using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using United.Mobile.Model.FlightSearchResult;
using United.Mobile.Model.Internal.Booking;

namespace United.Mobile.Model.BookingTrips
{
    public class BookingTripRequest:RequestBase
    {
        public string BookingTravelType { get; set; }
        public bool IsAgentToolLogOn { get; set; }
        public bool IsCrewTravelling { get; set; }
        public bool IsPassRiderLoggedIn { get; set; }
        [Required]
        public string SearchType { get; set; }
        public List<SelectedPassRider> SelectedPassRider { get; set; }
        public string ClientIpAddress { get; set; }
        public BookingTripRequest()
        {
            SelectedPassRider = new List<SelectedPassRider>();
        }
    }

}
