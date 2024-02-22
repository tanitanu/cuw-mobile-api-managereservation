using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBSHOPTrip : MOBSHOPTripBase
    {
        private string tripId = string.Empty;
        private string originDecoded = string.Empty;
        private string destinationDecoded = string.Empty;
        private int flightCount;
        private int totalFlightCount;
        private List<MOBSHOPFlattenedFlight> flattenedFlights;
        private List<FlightSection> flightSections;
        private int lastTripIndexRequested;
        private List<MOBSHOPShoppingProduct> columns;
        private string yqyrMessage = string.Empty;
        private int pageCount;
        private string originDecodedWithCountry;
        private string destinationDecodedWithCountry;
        private bool showOriginDestinationForFlights = false;
        private bool disableEplus;

        public bool DisableEplus
        {
            get { return disableEplus; }
            set { disableEplus = value; }
        }

        public string DestinationDecodedWithCountry
        {
            get
            {
                return this.destinationDecodedWithCountry;
            }
            set
            {
                this.destinationDecodedWithCountry = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string OriginDecodedWithCountry
        {
            get
            {
                return this.originDecodedWithCountry;
            }
            set
            {
                this.originDecodedWithCountry = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }


        public string YqyrMessage
        {
            get { return yqyrMessage; }
            set { yqyrMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
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

        public string OriginDecoded
        {
            get
            {
                return this.originDecoded;
            }
            set
            {
                this.originDecoded = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DestinationDecoded
        {
            get
            {
                return this.destinationDecoded;
            }
            set
            {
                this.destinationDecoded = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool ShowOriginDestinationForFlights
        {
            get
            {
                return this.showOriginDestinationForFlights;
            }
            set
            {
                this.showOriginDestinationForFlights = value;
            }
        }

        public List<MOBSHOPFlattenedFlight> FlattenedFlights
        {
            get
            {
                return this.flattenedFlights;
            }
            set
            {
                this.flattenedFlights = value;
            }
        }

        public List<FlightSection> FlightSections
        {
            get
            {
                return this.flightSections;
            }
            set
            {
                this.flightSections = value;
            }
        }

        public int FlightCount
        {
            get
            {
                return this.flightCount;
            }
            set
            {
                this.flightCount = value;
            }
        }

        public int TotalFlightCount
        {
            get
            {
                return this.totalFlightCount;
            }
            set
            {
                this.totalFlightCount = value;
            }
        }

        public int LastTripIndexRequested
        {
            get
            {
                return this.lastTripIndexRequested;
            }
            set
            {
                this.lastTripIndexRequested = value;
            }
        }

        public List<MOBSHOPShoppingProduct> Columns
        {
            get
            {
                return this.columns;
            }
            set
            {
                this.columns = value;
            }
        }

        private string callDurationText = string.Empty;

        public string CallDurationText
        {
            get { return callDurationText; }
            set { callDurationText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public int PageCount
        {
            get
            {
                return this.pageCount;
            }
            set
            {
                this.pageCount = value;
            }
        }

        //private string flightDateChangeMessage = ConfigurationManager.AppSettings["FlightDateChangeMessage"].ToString();
        public string FlightDateChangeMessage { get; set; } = "Please note this flight involves a date change";

        private bool tripHasNonStopflightsOnly;

        public bool TripHasNonStopflightsOnly
        {
            get { return tripHasNonStopflightsOnly; }
            set { tripHasNonStopflightsOnly = value; }
        }
        public MOBSHOPTrip()
        {
            FlattenedFlights = new List<MOBSHOPFlattenedFlight>();
        }
    }
}