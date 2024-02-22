namespace United.Mobile.Model.FlightSearchResult
{
    public class ChangeSearchDateRequest:RequestBase
    {
        public string TripIndex { get; set; }
        public string ChangeDays { get; set; }        
        public string BookingTravelType { get; set; }
        public bool IncludeSSRUMNR { get; set; }
    }
}
