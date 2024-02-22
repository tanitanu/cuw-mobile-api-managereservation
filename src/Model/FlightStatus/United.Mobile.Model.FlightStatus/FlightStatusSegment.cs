using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.FlightStatus
{
    [Serializable]
    public class FlightStatusSegment : Segment
    {
        private string departureTerminal = string.Empty;

        private string arrivalTerminal = string.Empty;

        private string departureGate = string.Empty;

        private string arrivalGate = string.Empty;

        public string ScheduledFlightTime { get; set; } = string.Empty;

        public string ActualFlightTime { get; set; } = string.Empty;

        public string EstimatedDepartureDateTime { get; set; } = string.Empty;

        public string EstimatedArrivalDateTime { get; set; } = string.Empty;

        public string ActualDepartureDateTime { get; set; } = string.Empty;

        public string ActualArrivalDateTime { get; set; } = string.Empty;

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

        private List<MOBPNRAdvisory> alerts  = new List<MOBPNRAdvisory>();

        public List<MOBPNRAdvisory> Alerts
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

        public Equipment Ship { get; set; }

        public Airline OperatingCarrier { get; set; }

        public Airline CodeShareCarrier { get; set; }

        public string Status { get; set; } = string.Empty;

        public bool EnableSeatMap { get; set; }

        public bool EnableStandbyList { get; set; }

        public bool EnableUpgradeList { get; set; }

        public bool EnableAmenity { get; set; }

        public string CodeShareflightNumber { get; set; } = string.Empty;

        public bool CanPushNotification { get; set; }

        public bool IsSegmentCancelled { get; set; }

        public bool GetInBoundSegment { get; set; }

        public FlightSegment InBoundSegment { get; set; }

        private bool isWiFiAvailable ;

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

        public string LastUpdatedGMT { get; set; } = string.Empty;

        public string PushNotificationRegId { get; set; } = string.Empty;

        public string StatusShort { get; set; } = string.Empty;

        public bool AddToComplications { get; set; }

        public Baggage Baggage { get; set; }

        public string ArrivalAirport { get; set; } = string.Empty;

        public string DepartureAirport { get; set; } = string.Empty;

        public bool EnableAllFlifoTopTabs { get; set; } = true;

        public bool EnableFlifoPushNotification { get; set; } = true;

        public bool EnableShareMyFlight { get; set; } = true;

        public bool EnableWhereAirCraftCurrently { get; set; } = true;
        public bool EnableWhereAircraftCurrently { get; set; } = true;

        public FlightStatusSegmentDetails FlightStatusSegmentDetails { get; set; }

        public bool IsShuttleService { get; set; } = false;
        public PreBoardActionItem TrackerActionItem { get; set; }

        public string PlaneMapUrl { get; set; } = string.Empty;

        public FlightStatusShareOption ShareOption { get; set; }

        private bool showMoreFlightHistoryButton = false;
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

    }
}