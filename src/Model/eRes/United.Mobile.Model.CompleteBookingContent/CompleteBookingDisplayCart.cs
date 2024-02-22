using System.Collections.Generic;
using United.Mobile.Model.BookingTrips;
using United.Mobile.Model.FlightSearchResult;

namespace United.Mobile.Model.CompleteBookingContent
{
    public class CompleteBookingDisplayCart
    {
        public PriceBreakdownDetails PriceBreakdownDetails { get; set; }
        public List<SaveBookingTrip> BookingTrips { get; set; }
        public List<PnrPassengerDetail> BookingPassengers { get; set; }
        public CardInfo CardInfo { get; set; }
        public string TotalCost { get; set; }
        public bool ShowPriceBreakdown { get; set; }
        public string FromAndToDate { get; set; }
        public string DepartAndArrival { get; set; }

    }
   
}
