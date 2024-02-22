using System.Collections.Generic;
using United.Mobile.Model.FlightSearchResult;

namespace United.Mobile.Model.SaveTripContent
{
    public class SaveTripResponse : FlightSearchResultResponse
    {       
        public string FlightsSelectedHeader { get; set; }
        public string SelectionsByTravelersHeader { get; set; }
        public string SelectionsByTravelersContent { get; set; }
    }
}
