using System;
using System.Collections.Generic;
using United.Mobile.Model.Shopping.Misc;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class FlightStatusSegment : Segment
    {
        private string scheduledFlightTime = string.Empty;
        private string actualFlightTime = string.Empty;
        private string estimatedDepartureDateTime = string.Empty;
        private string estimatedArrivalDateTime = string.Empty;
        private string actualDepartureDateTime = string.Empty;
        private string actualArrivalDateTime = string.Empty;
        private string departureTerminal = string.Empty;
        private string arrivalTerminal = string.Empty;
        private string departureGate = string.Empty;
        private string arrivalGate = string.Empty;
        private Equipment ship;
        private Airline operatingCarrier;
        private Airline codeShareCarrier;
        private string status = string.Empty;
        private bool enableSeatMap;
        private bool enableStandbyList;
        private bool enableUpgradeList;
        private bool enableAmenity;
        private string codeShareflightNumber = string.Empty;
        private bool canPushNotification;
        private bool isSegmentCancelled;
        private bool getInBoundSegment;
        private FlightSegment inBoundSegment;
        private bool isWiFiAvailable;
        private string lastUpdatedGMT = string.Empty;
        private string pushNotificationRegId = string.Empty;
        private string statusShort = string.Empty;
        private Baggage baggage;
        //User Story - 160153 - Added below DepartureAirport and ArrivalAirport variables to get the airportname from database
        private string arrivalAirport = string.Empty;
        private string departureAirport = string.Empty;
        private bool enableAllFlifoTopTabs = true;
        private bool enableFlifoPushNotification = true;
        private bool enableShareMyFlight = true;
        private bool enableWhereAircraftCurrently = true;
        private bool enableWhereAirCraftCurrently = true;
        private string planeMapUrl = string.Empty;
        private FlightStatusSegmentDetails flightStatusSegmentDetails;
        private bool isShuttleService = false;
        private MOBPreBoardActionItem trackerActionItem;
        private MOBFlightStatusShareOption shareOption;
        private bool showMoreFlightHistoryButton = false;
        private List<Advisory> alerts;
        public FlightStatusSegment()
            : base()
        {
            //this.addToComplications = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["EnableAddToComplications"].ToString());
        }

        public string ScheduledFlightTime
        {
            get
            {
                return this.scheduledFlightTime;
            }
            set
            {
                this.scheduledFlightTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public MOBPreBoardActionItem TrackerActionItem
        {
            get
            {
                return this.trackerActionItem;
            }
            set
            {
                this.trackerActionItem = value;
            }
        }

        public MOBFlightStatusShareOption ShareOption
        {
            get
            {
                return this.shareOption;
            }
            set
            {
                this.shareOption = value;
            }
        }
        public string ActualFlightTime
        {
            get
            {
                return this.actualFlightTime;
            }
            set
            {
                this.actualFlightTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string EstimatedDepartureDateTime
        {
            get
            {
                return this.estimatedDepartureDateTime;
            }
            set
            {
                this.estimatedDepartureDateTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string EstimatedArrivalDateTime
        {
            get
            {
                return this.estimatedArrivalDateTime;
            }
            set
            {
                this.estimatedArrivalDateTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ActualDepartureDateTime
        {
            get
            {
                return this.actualDepartureDateTime;
            }
            set
            {
                this.actualDepartureDateTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ActualArrivalDateTime
        {
            get
            {
                return this.actualArrivalDateTime;
            }
            set
            {
                this.actualArrivalDateTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public bool ShowMoreFlightHistoryButton
        {
            get
            {
                return this.showMoreFlightHistoryButton;
            }
            set
            {
                this.showMoreFlightHistoryButton = value;
            }
        }

        public List<Advisory> Alerts
        {
            get
            {
                return this.alerts;
            }
            set
            {
                this.alerts = value;
            }
        }
        public string DepartureTerminal
        {
            get
            {
                return this.departureTerminal;
            }
            set
            {
                this.departureTerminal = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ArrivalTerminal
        {
            get
            {
                return this.arrivalTerminal;
            }
            set
            {
                this.arrivalTerminal = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DepartureGate
        {
            get
            {
                return this.departureGate;
            }
            set
            {
                this.departureGate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string ArrivalGate
        {
            get
            {
                return this.arrivalGate;
            }
            set
            {
                this.arrivalGate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public Equipment Ship
        {
            get
            {
                return this.ship;
            }
            set
            {
                this.ship = value;
            }
        }

        public Airline OperatingCarrier
        {
            get
            {
                return this.operatingCarrier;
            }
            set
            {
                this.operatingCarrier = value;
            }
        }

        public Airline CodeShareCarrier
        {
            get
            {
                return this.codeShareCarrier;
            }
            set
            {
                this.codeShareCarrier = value;
            }
        }

        public string Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool EnableSeatMap
        {
            get
            {
                return enableSeatMap;
            }
            set
            {
                this.enableSeatMap = value;
            }
        }

        public bool EnableStandbyList
        {
            get
            {
                return enableStandbyList;
            }
            set
            {
                this.enableStandbyList = value;
            }
        }

        public bool EnableUpgradeList
        {
            get
            {
                return enableUpgradeList;
            }
            set
            {
                this.enableUpgradeList = value;
            }
        }

        public bool EnableAmenity
        {
            get
            {
                return enableAmenity;
            }
            set
            {
                this.enableAmenity = value;
            }
        }

        public string CodeShareflightNumber
        {
            get
            {
                return this.codeShareflightNumber;
            }
            set
            {
                this.codeShareflightNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool CanPushNotification
        {
            get
            {
                return canPushNotification;
            }
            set
            {
                canPushNotification = value;
            }
        }

        public bool IsSegmentCancelled
        {
            get
            {
                return isSegmentCancelled;
            }
            set
            {
                this.isSegmentCancelled = value;
            }
        }

        public bool GetInBoundSegment
        {
            get
            {
                return getInBoundSegment;
            }
            set
            {
                this.getInBoundSegment = value;
            }
        }

        public FlightSegment InBoundSegment
        {
            get
            {
                return this.inBoundSegment;
            }
            set
            {
                this.inBoundSegment = value;
            }
        }
        public bool ISWiFiAvailable
        {
            get
            {
                return isWiFiAvailable;
            }
            set
            {
                this.isWiFiAvailable = value;
            }
        }

        public string LastUpdatedGMT
        {
            get
            {
                return this.lastUpdatedGMT;
            }
            set
            {
                this.lastUpdatedGMT = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string PushNotificationRegId
        {
            get
            {
                return this.pushNotificationRegId;
            }
            set
            {
                this.pushNotificationRegId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string StatusShort
        {
            get
            {
                return this.statusShort;
            }
            set
            {
                this.statusShort = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        private bool addToComplications;
        public bool AddToComplications
        {
            get
            {
                return this.addToComplications;
            }
            set
            {
                this.addToComplications = value;
            }
        }
        public Baggage Baggage
        {
            get { return this.baggage; }
            set { this.baggage = value; }
        }
        //User Story - 160153 - Added below DepartureAirport and ArrivalAirport property to get the airportname from database
        public string ArrivalAirport
        {
            get { return arrivalAirport; }
            set { this.arrivalAirport = value; }
        }
        public string DepartureAirport
        {
            get { return departureAirport; }
            set { this.departureAirport = value; }
        }

        public bool EnableAllFlifoTopTabs
        {
            get { return enableAllFlifoTopTabs; }
            set { this.enableAllFlifoTopTabs = value; }
        }

        public bool EnableFlifoPushNotification
        {
            get { return enableFlifoPushNotification; }
            set { this.enableFlifoPushNotification = value; }
        }

        public bool EnableShareMyFlight
        {
            get { return enableShareMyFlight; }
            set { this.enableShareMyFlight = value; }
        }

        public bool EnableWhereAircraftCurrently
        {
            get { return enableWhereAircraftCurrently; }
            set { enableWhereAircraftCurrently = value; }
        }

        public bool EnableWhereAirCraftCurrently
        {
            get { return enableWhereAirCraftCurrently; }
            set { enableWhereAirCraftCurrently = value; }
        }
        public FlightStatusSegmentDetails FlightStatusSegmentDetails
        {
            get { return flightStatusSegmentDetails; }

            set { flightStatusSegmentDetails = value; }
        }

        public bool IsShuttleService
        {
            get
            {
                return isShuttleService;
            }
            set
            {
                this.isShuttleService = value;
            }
        }
    }
}
