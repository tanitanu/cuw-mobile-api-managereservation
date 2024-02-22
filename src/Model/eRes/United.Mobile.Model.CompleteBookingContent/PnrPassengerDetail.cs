using System.Collections.Generic;
using United.Mobile.Model.BookingTrips;

namespace United.Mobile.Model.CompleteBookingContent
{
    public class PnrPassengerDetail 
    {
        public string ConfirmationPnrText { get; set; }
        public string RecordLocator { get; set; }
        public List<PassengerDetail> Passengers { get; set; }
    }
   
}
