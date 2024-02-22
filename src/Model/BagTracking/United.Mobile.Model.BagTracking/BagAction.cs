using System;

namespace United.Mobile.Model.BagTracking
{
    [Serializable]
    public class BagAction
    {
        private string actionCode = string.Empty;
        private string actionDateTime = string.Empty;
        private string actionDescription = string.Empty;
        private string actionStationCode = string.Empty;
        private string actionUTCDateTime = string.Empty;
        private string agentId = string.Empty;
        private string arrivalAirport = string.Empty;
        private string bagLoadSqnbr = string.Empty;
        private string bagTagHistorySqnbr = string.Empty;
        private string bagTagStatusCode = string.Empty;
        private string departureAirport = string.Empty;
        private string flightLegSqnbr = string.Empty;
        private string processBagMessage = string.Empty;
        private string scanDateTime = string.Empty;
        private string stageNumber = string.Empty;

        public string FlightLegSqnbr
        {
            get
            {
                return flightLegSqnbr;
            }
            set
            {
                this.flightLegSqnbr = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string ActionCode
        {
            get
            {
                return actionCode;
            }
            set
            {
                this.actionCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string ActionDateTime
        {
            get
            {
                return actionDateTime;
            }
            set
            {
                this.actionDateTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ActionDescription
        {
            get
            {
                return actionDescription;
            }
            set
            {
                this.actionDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ActionStationCode
        {
            get
            {
                return actionStationCode;
            }
            set
            {
                this.actionStationCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string ActionUTCDateTime
        {
            get
            {
                return actionUTCDateTime;
            }
            set
            {
                this.actionUTCDateTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string AgentId
        {
            get
            {
                return agentId;
            }
            set
            {
                this.agentId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ArrivalAirport
        {
            get
            {
                return arrivalAirport;
            }
            set
            {
                this.arrivalAirport = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string BagLoadSqnbr
        {
            get
            {
                return bagLoadSqnbr;
            }
            set
            {
                this.bagLoadSqnbr = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string BagTagHistorySqnbr
        {
            get
            {
                return bagTagHistorySqnbr;
            }
            set
            {
                this.bagTagHistorySqnbr = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string BagTagStatusCode
        {
            get
            {
                return bagTagStatusCode;
            }
            set
            {
                this.bagTagStatusCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string DepartureAirport
        {
            get
            {
                return departureAirport;
            }
            set
            {
                this.departureAirport = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string ProcessBagMessage
        {
            get
            {
                return processBagMessage;
            }
            set
            {
                this.processBagMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ScanDateTime
        {
            get
            {
                return scanDateTime;
            }
            set
            {
                this.scanDateTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string StageNumber
        {
            get
            {
                return stageNumber;
            }
            set
            {
                this.stageNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
    }
}
