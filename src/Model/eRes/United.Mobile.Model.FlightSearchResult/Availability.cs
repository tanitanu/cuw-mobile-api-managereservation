using System.Collections.Generic;

namespace United.Mobile.Model.FlightSearchResult
{
    public class Availability
    {
        public bool AwardTravel { get; set; }
        public string SessionId { get; set; }
        public string CartId { get; set; }
        public decimal CloseInBookingFee { get; set; }
        public List<SearchDateItem> SearchDates { get; set; }
        public ShopTrip Trip { get; set; }
        public bool ShowNext { get; set; }
        public bool ShowPrevious { get; set; }
        public bool LoadMoreFlights { get; set; }
    }
}