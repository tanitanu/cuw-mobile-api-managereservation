using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    [XmlRoot("MOBSHOPSelectTripRequest")]
    public class SelectTripRequest : MOBRequest
    {
        public string cartId { get; set; } = string.Empty;

        public int LengthOfCalendar { get; set; }

        public string SessionId { get; set; } = string.Empty;

        public string TripId { get; set; } = string.Empty;

        public string FlightId { get; set; } = string.Empty;

        public string ProductId { get; set; } = string.Empty;

        public string RewardId { get; set; } = string.Empty;

        public bool UseFilters { get; set; }

        public MOBSearchFilters Filters { get; set; }

        public string ResultSortType { get; set; } 
        

        public string CalendarDateChange { get; set; } 

        public bool BackButtonClick { get; set; }

        public bool ISProductSelected { get; set; }

        public bool GetNonStopFlightsOnly { get; set; }

        public bool GetFlightsWithStops { get; set; }
        private List<MOBItem> catalogItems;
        public List<MOBItem> CatalogItems
        {
            get { return catalogItems; }
            set { catalogItems = value; }
        }

    }
}
