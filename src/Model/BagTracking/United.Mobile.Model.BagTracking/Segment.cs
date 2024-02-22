using System;

namespace United.Mobile.Model.BagTracking
{
    [Serializable]
    public class Segment
    {
        private string flightNumber = string.Empty;
        private string departureAirportCode = string.Empty;
        private string arrivalAirportCode = string.Empty;
        private string departureDateTime = string.Empty;
        private string departureDateTimeGMT = string.Empty;

        private string estimatedArrivalTime = string.Empty;
        private string estimatedDepartureTime = string.Empty;

        private string estimatedArrivalTimeGMT = string.Empty;
        private string estimatedDepartureTimeGMT = string.Empty;

        private string bagStatus = string.Empty;
        private string flightStatus = string.Empty;

        private string operatingCarrierCode = string.Empty;
        private bool isIn;
        private bool isOut;
        private string bagCarousel = string.Empty;
        private bool bagAtStation;
        private bool bagAtClaimBelt;

        private string inTime = string.Empty;
        private string outTime = string.Empty;

        public string FlightNumber
        {
            get
            {
                return flightNumber;
            }
            set
            {
                this.flightNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DepartureAirportCode
        {
            get
            {
                return departureAirportCode;
            }
            set
            {
                this.departureAirportCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string ArrivalAirportCode
        {
            get
            {
                return arrivalAirportCode;
            }
            set
            {
                this.arrivalAirportCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string DepartureDateTime
        {
            get
            {
                return departureDateTime;
            }
            set
            {
                this.departureDateTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DepartureDateTimeGMT
        {
            get
            {
                return departureDateTimeGMT;
            }
            set
            {
                this.departureDateTimeGMT = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string EstimatedArrivalTime
        {
            get
            {
                return this.estimatedArrivalTime;
            }
            set
            {
                this.estimatedArrivalTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string EstimatedDepartureTime
        {
            get
            {
                return this.estimatedDepartureTime;
            }
            set
            {
                this.estimatedDepartureTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string EstimatedArrivalTimeGMT
        {
            get
            {
                return this.estimatedArrivalTimeGMT;
            }
            set
            {
                this.estimatedArrivalTimeGMT = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string EstimatedDepartureTimeGMT
        {
            get
            {
                return this.estimatedDepartureTimeGMT;
            }
            set
            {
                this.estimatedDepartureTimeGMT = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string BagStatus
        {
            get
            {
                return this.bagStatus;
            }
            set
            {
                this.bagStatus = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string FlightStatus
        {
            get
            {
                return this.flightStatus;
            }
            set
            {
                this.flightStatus = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public bool IsIn
        {
            get
            {
                return this.isIn;
            }
            set
            {
                this.isIn = value;
            }
        }

        public bool IsOut
        {
            get
            {
                return this.isOut;
            }
            set
            {
                this.isOut = value;
            }
        }

        public string BagCarousel
        {
            get
            {
                return this.bagCarousel;
            }
            set
            {
                this.bagCarousel = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool BagAtStation
        {
            get
            {
                return this.bagAtStation;
            }
            set
            {
                this.bagAtStation = value;
            }
        }

        public string InTime
        {
            get
            {
                return this.inTime;
            }
            set
            {
                this.inTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string OutTime
        {
            get
            {
                return this.outTime;
            }
            set
            {
                this.outTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool BagAtClaimBelt
        {
            get
            {
                return this.bagAtClaimBelt;
            }
            set
            {
                this.bagAtClaimBelt = value;
            }
        }

    }
}
