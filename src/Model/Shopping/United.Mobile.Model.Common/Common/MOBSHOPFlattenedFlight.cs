using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBSHOPFlattenedFlight
    {

        private string tripId = string.Empty;
        private string flightId = string.Empty;
        private string productId = string.Empty;
        private string tripDays = string.Empty;
        private string cabinMessage = string.Empty;
        private List<MOBSHOPFlight> flights;
        private bool isUADiscount;
        private string addCollectProductId;
        private string flightHash = string.Empty;
        private bool isIBELite;
        private bool isIBE;
        private bool isElf;
        private bool isCovidTestFlight;
        private bool isChangeFeeWaiver;
        private List<string> flightLabelTextList;
        private string msgFlightCarrier;
        private string carrierMessage;

        public string CarrierMessage
        {
            get { return carrierMessage; }
            set { carrierMessage = value; }
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

        public string CabinMessage
        {
            get
            {
                return this.cabinMessage;
            }
            set
            {
                this.cabinMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public List<MOBSHOPFlight> Flights
        {
            get
            {
                return this.flights;
            }
            set
            {
                this.flights = value;
            }
        }

        public string TripDays
        {
            get
            {
                return this.tripDays;
            }
            set
            {
                this.tripDays = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool IsUADiscount
        {
            get
            {
                return this.isUADiscount;
            }
            set
            {
                this.isUADiscount = value;
            }
        }

        public bool isAddCollectWaived { get; set; }

        public string AddCollectProductId
        {
            get
            {
                return this.addCollectProductId;
            }
            set
            {
                this.addCollectProductId = value;
            }
        }

        /// <example>
        /// "16-31|1180-UA"
        /// </example>
        /// <hint>
        /// The flight's hash of the flight. If a flight has connection, this is the flight hash of the first segment 
        /// </hint>
        public string FlightHash
        {
            get
            {
                return flightHash;
            }
            set
            {
                flightHash = string.IsNullOrEmpty(value) ? "US" : value.Trim().ToUpper();
            }
        }

        private string airPortChange = string.Empty;

        [JsonProperty(PropertyName = "airportChange")]
        public string AirportChange
        {
            get { return this.airPortChange; }
            set
            {
                this.airPortChange = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        
        /// <summary>
        /// True if the product is IBELite amd is available
        /// </summary>
        public bool IsIBELite
        {
            get { return isIBELite; }
            set { isIBELite = value; }
        }

        public bool IsIBE
        {
            set { isIBE = value; }
            get { return isIBE; }
        }

        public bool IsElf
        {
            set { isElf = value; }
            get { return isElf; }
        }
        public bool IsChangeFeeWaiver
        {
            get { return isChangeFeeWaiver; }
            set { isChangeFeeWaiver = value; }
        }
        public bool IsCovidTestFlight
        {
            get { return isCovidTestFlight; }
            set { isCovidTestFlight = value; }
        }
        public List<string> FlightLabelTextList
        {
            get { return flightLabelTextList; }
            set { flightLabelTextList = value; }
        }

        private List<MOBStyledText> flightBadges;
        public List<MOBStyledText> FlightBadges
        {
            get { return flightBadges; }
            set { flightBadges = value; }
        }

        public string MsgFlightCarrier
        {
            get { return msgFlightCarrier; }
            set { msgFlightCarrier = value; }
        }

    }
}
