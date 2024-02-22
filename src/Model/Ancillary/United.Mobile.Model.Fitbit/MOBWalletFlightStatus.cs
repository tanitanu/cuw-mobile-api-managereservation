using System;

namespace United.Mobile.Model.Fitbit
{
    [Serializable()]
    public class MOBWalletFlightStatus
    {
        private string carrierCode = string.Empty;
        private string carrierName = string.Empty;
        private string operatingCarrierCode = string.Empty;
        private string operatingCarrierName = string.Empty;
        private string flightNumber = string.Empty;
        private string origin = string.Empty;
        private string originName = string.Empty;
        private string destination = string.Empty;
        private string destinationName = string.Empty;
        private string scheduledDepartureDateTime = string.Empty;
        private string scheduledArrivalDateTime = string.Empty;
        private string scheduledDepartureDateTimeGMT = string.Empty;
        private string scheduledArrivalDateTimeGMT = string.Empty;
        private string estimatedDepartureDateTime = string.Empty;
        private string estimatedArrivalDateTime = string.Empty;
        private string actualDepartureDateTime = string.Empty;
        private string actualArrivalDateTime = string.Empty;
        private string departureTerminal = string.Empty;
        private string arrivalTerminal = string.Empty;
        private string departureGate = string.Empty;
        private string arrivalGate = string.Empty;
        private string baggageClaim = string.Empty;
        private string ship = string.Empty;
        private string equipment = string.Empty;
        private string status = string.Empty;
        private bool isWiFiAvailable;
        private string lastUpdated = string.Empty;

        public MOBWalletFlightStatus()
        {
           // this.addToComplications = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["EnableAddToComplications"].ToString());
        }

        public string CarrierCode
        {
            get
            {
                return this.carrierCode;
            }
            set
            {
                this.carrierCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string CarrierName
        {
            get
            {
                return this.carrierName;
            }
            set
            {
                this.carrierName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string OperatingCarrierCode
        {
            get
            {
                return this.operatingCarrierCode;
            }
            set
            {
                this.operatingCarrierCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string OperatingCarrierName
        {
            get
            {
                return this.operatingCarrierName;
            }
            set
            {
                this.operatingCarrierName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string FlightNumber
        {
            get
            {
                return this.flightNumber;
            }
            set
            {
                this.flightNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string Origin
        {
            get
            {
                return this.origin;
            }
            set
            {
                this.origin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string OriginName
        {
            get
            {
                return this.originName;
            }
            set
            {
                this.originName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Destination
        {
            get
            {
                return this.destination;
            }
            set
            {
                this.destination = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string DestinationName
        {
            get
            {
                return this.destinationName;
            }
            set
            {
                this.destinationName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ScheduledDepartureDateTime
        {
            get
            {
                return this.scheduledDepartureDateTime;
            }
            set
            {
                this.scheduledDepartureDateTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ScheduledArrivalDateTime
        {
            get
            {
                return this.scheduledArrivalDateTime;
            }
            set
            {
                this.scheduledArrivalDateTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ScheduledDepartureDateTimeGMT
        {
            get
            {
                return this.scheduledDepartureDateTimeGMT;
            }
            set
            {
                this.scheduledDepartureDateTimeGMT = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ScheduledArrivalDateTimeGMT
        {
            get
            {
                return this.scheduledArrivalDateTimeGMT;
            }
            set
            {
                this.scheduledArrivalDateTimeGMT = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public string BaggageClaim
        {
            get
            {
                return this.baggageClaim;
            }
            set
            {
                this.baggageClaim = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Ship
        {
            get
            {
                return this.ship;
            }
            set
            {
                this.ship = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Equipment
        {
            get
            {
                return this.equipment;
            }
            set
            {
                this.equipment = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public bool IsWiFiAvailable
        {
            get
            {
                return this.isWiFiAvailable;
            }
            set
            {
                this.isWiFiAvailable = value;
            }
        }

        public string LastUpdated
        {
            get
            {
                return this.lastUpdated;
            }
            set
            {
                this.lastUpdated = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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
    }
}
