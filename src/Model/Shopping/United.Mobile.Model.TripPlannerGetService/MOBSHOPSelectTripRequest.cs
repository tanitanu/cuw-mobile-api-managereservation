using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.TripPlannerGetService
{
    [Serializable()]
    public class MOBSHOPSelectTripRequest : MOBRequest
    {
        private string sessionId = string.Empty;
        private string tripId = string.Empty;
        private string flightId = string.Empty;
        private string productId = string.Empty;
        private string rewardId = string.Empty;
        private bool useFilters = false;
        private MOBSHOPSearchFilters filters;
        private string resultSortType = string.Empty;
        private string calendarDateChange = string.Empty;
        private bool backButtonClick = false;
        private bool isProductSelected = false;
        private bool getNonStopFlightsOnly;
        private bool getFlightsWithStops;
        private int lengthOfCalendar;
        private string cartId;
        private List<MOBItem> catalogItems;

        public string CartId
        {
            get
            {
                return cartId;
            }
            set
            {
                cartId = value;
            }
        }
        public int LengthOfCalendar
        {
            get { return lengthOfCalendar; }
            set { lengthOfCalendar = value; }
        }

        public string SessionId
        {
            get
            {
                return this.sessionId;
            }
            set
            {
                this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string TripId
        {
            get
            {
                return this.tripId;
            }
            set
            {
                this.tripId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string FlightId
        {
            get
            {
                return this.flightId;
            }
            set
            {
                this.flightId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ProductId
        {
            get
            {
                return this.productId;
            }
            set
            {
                this.productId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string RewardId
        {
            get
            {
                return this.rewardId;
            }
            set
            {
                this.rewardId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool UseFilters
        {
            get
            {
                return this.useFilters;
            }
            set
            {
                this.useFilters = value;
            }
        }

        public MOBSHOPSearchFilters Filters
        {
            get
            {
                return this.filters;
            }
            set
            {
                this.filters = value;
            }
        }

        public string ResultSortType
        {
            get;set;
        }

        public string CalendarDateChange
        {
            get;set;
        }

        public bool BackButtonClick
        {
            get
            {
                return this.backButtonClick;
            }
            set
            {
                this.backButtonClick = value;
            }
        }

        public bool ISProductSelected
        {
            get
            {
                return this.isProductSelected;
            }
            set
            {
                this.isProductSelected = value;
            }
        }

        public bool GetNonStopFlightsOnly
        {
            get { return getNonStopFlightsOnly; }
            set { getNonStopFlightsOnly = value; }
        }

        public bool GetFlightsWithStops
        {
            get { return getFlightsWithStops; }
            set { getFlightsWithStops = value; }
        }

        public List<MOBItem> CatalogItems
        {
            get { return catalogItems; }
            set { catalogItems = value; }
        }

    }
}
