using System;
using System.Collections.Generic;

namespace United.Mobile.Model.BagTracking
{
    [Serializable]
    public class BagItinerary
    {
        private string currentItineraryRoute = string.Empty;
        private string originalItineraryRoute = string.Empty;
        private string currentPaxItineraryRoute = string.Empty;

        public string CurrentItineraryRoute
        {
            get
            {
                return currentItineraryRoute;
            }
            set
            {
                this.currentItineraryRoute = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string OriginalItineraryRoute
        {
            get
            {
                return originalItineraryRoute;
            }
            set
            {
                this.originalItineraryRoute = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public List<BagFlightSegment> CurrentItinerary { get; set; }
        public string CurrentPaxItineraryRoute
        {
            get
            {
                return currentPaxItineraryRoute;
            }
            set
            {
                this.currentPaxItineraryRoute = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public BagItinerary()
        {
            CurrentItinerary = new List<BagFlightSegment>();
        }
    }
}
