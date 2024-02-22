using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBSeatMap
    {
        private int flightNumber;
        private string flightDateTime = string.Empty;
        private MOBAirport departure;
        private MOBAirport arrival;
        private string codeshareFlightNumber = string.Empty;
        private string seatLegendId = string.Empty;
        private List<MOBCabin> cabins = new List<MOBCabin>();
        private string legId = string.Empty;
        private string fleetType = string.Empty;
        private bool seatMapAvailabe;
        private string eplusPromotionMessage = string.Empty;
        private bool suppressLMX;
        private string showInfoMessageOnSeatMap;
        private bool isOaSeatMap;
        private string operatedByText;
        private bool isReadOnlySeatMap = false;
        private string OASeatMapBannerMessage = string.Empty;
        private List<MOBItem> captions;
        private bool hasNoComplimentarySeatsAvailableForOA;
        private string showInfoTitleForOA;
        private bool isAdjacentPreferredSeatsModified;
        private int numberOfPreferredSeatsOpened;
        private string adjacentSeatsList;
        private List<MOBLegend> cabinLegends = new List<MOBLegend>();
        private List<MOBLegend> monumentLegends = new List<MOBLegend>();
        private List<MOBTier> tiers;
        private List<InfoWarningMessages> alertMessages = new List<InfoWarningMessages>();
        private string seatPurchasedText;

        public string SeatPurchasedText
        {
            get { return seatPurchasedText; }
            set { seatPurchasedText = value; }
        }

        public string ShowInfoMessageOnSeatMap
        {
            get { return showInfoMessageOnSeatMap; }
            set { showInfoMessageOnSeatMap = value; }
        }

        public bool SuppressLMX
        {
            get { return suppressLMX; }
            set { suppressLMX = value; }
        }

        public string EplusPromotionMessage
        {
            get
            {
                return this.eplusPromotionMessage;
            }
            set
            {
                this.eplusPromotionMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public int FlightNumber
        {
            get
            {
                return this.flightNumber;
            }
            set
            {
                this.flightNumber = value;
            }
        }

        public string FlightDateTime
        {
            get
            {
                return this.flightDateTime;
            }
            set
            {
                this.flightDateTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public MOBAirport Departure
        {
            get { return departure; }
            set { departure = value; }
        }

        public MOBAirport Arrival
        {
            get { return arrival; }
            set { arrival = value; }
        }

        public string CodeshareFlightNumber
        {
            get
            {
                return this.codeshareFlightNumber;
            }
            set
            {
                this.codeshareFlightNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string SeatLegendId
        {
            get
            {
                return this.seatLegendId;
            }
            set
            {
                this.seatLegendId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<MOBCabin> Cabins
        {
            get { return cabins; }
            set { cabins = value; }
        }


        public string LegId
        {
            get
            {
                return this.legId;
            }
            set
            {
                this.legId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string FleetType
        {
            get
            {
                return this.fleetType;
            }
            set
            {
                this.fleetType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool SeatMapAvailabe
        {
            get { return seatMapAvailabe; }
            set { seatMapAvailabe = value; }
        }

        public bool IsOaSeatMap
        {
            get { return isOaSeatMap; }
            set { isOaSeatMap = value; }
        }

        public string OperatedByText
        {
            get { return operatedByText; }
            set { operatedByText = value; }
        }
        public bool IsReadOnlySeatMap
        {
            get { return isReadOnlySeatMap; }
            set { isReadOnlySeatMap = value; }
        }
        public string oASeatMapBannerMessage
        {
            get { return OASeatMapBannerMessage; }
            set { OASeatMapBannerMessage = value; }
        }

        public List<MOBItem> Captions
        {
            get { return captions; }
            set { captions = value; }
        }

        public bool HasNoComplimentarySeatsAvailableForOA
        {
            get { return hasNoComplimentarySeatsAvailableForOA; }
            set { hasNoComplimentarySeatsAvailableForOA = value; }
        }
        public MOBSeatMap()
        {
            Cabins = new List<MOBCabin>();
            cabinLegends = new List<MOBLegend>();
            monumentLegends = new List<MOBLegend>();
            tiers = new List<MOBTier>();
        }
        public string ShowInfoTitleForOA
        {
            get { return showInfoTitleForOA; }
            set { showInfoTitleForOA = value; }
        }

        public bool IsAdjacentPreferredSeatsModified
        {
            get { return isAdjacentPreferredSeatsModified; }
            set { isAdjacentPreferredSeatsModified = value; }
        }

        public int NumberOfPreferredSeatsOpened
        {
            get { return numberOfPreferredSeatsOpened; }
            set { numberOfPreferredSeatsOpened = value; }
        }

        public string AdjacentSeatsList
        {
            get { return adjacentSeatsList; }
            set { adjacentSeatsList = value; }
        }

        public List<MOBLegend> CabinLegends
        {
            get { return cabinLegends; }
            set { cabinLegends = value; }
        }

        public List<MOBLegend> MonumentLegends
        {
            get { return monumentLegends; }
            set { monumentLegends = value; }
        }

        public List<MOBTier> Tiers
        {
            get { return this.tiers; }
            set { tiers = value; }
        }

        public List<InfoWarningMessages> AlertMessages
        {
            get { return this.alertMessages; }
            set { alertMessages = value; }
        }
    }
}
