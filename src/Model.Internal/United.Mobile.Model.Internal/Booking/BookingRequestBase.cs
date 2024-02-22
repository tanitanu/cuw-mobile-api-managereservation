using System.Collections.Generic;

namespace United.Mobile.Model.Internal.Booking
{
    public class BookingRequestBase
    {
        public string BookingSessionId { get; set; }
        public string BookingTravelType { get; set; }
        public string ClientIpAddress { get; set; }
        public bool IsAgentToolLogOn { get; set; }
        public bool IsCrewTravelling { get; set; }
        public bool IsPassRiderLoggedIn { get; set; }
        public string SearchType { get; set; }
        public List<SelectedPassRider> SelectedPassRider { get; set; }
        public string TransactionId { get; set; }
        public BookingRequestBase()
        {
            SelectedPassRider = new List<SelectedPassRider>();
        }
    }
}
