using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class SeatMapResponse : MOBResponse
    {
        private string marketingCarrierCode;
        private string operatingCarrierCode;
        private string flightNumber = string.Empty;
        private string flightDate = string.Empty;
        private string departureAirportCode = string.Empty;
        private string arrivalAirportCode = string.Empty;
        private MOBSeatMap seatMap;
        private SHOPOnTimePerformance onTimePerformance;
        private string sessionId = string.Empty;
        private SeatMapRequest seatMapRequest;

        public SeatMapResponse()
            : base()
        {
        }

        public SeatMapRequest SeatMapRequest
        {
            get
            {
                return this.seatMapRequest;
            }
            set
            {
                this.seatMapRequest = value;
            }
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

        public string MarketingCarrierCode
        {
            get
            {
                return this.marketingCarrierCode;
            }
            set
            {
                this.marketingCarrierCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
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

        public string FlightNumber
        {
            get
            {
                return this.flightNumber;
            }
            set
            {
                this.flightNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string FlightDate
        {
            get
            {
                return this.flightDate;
            }
            set
            {
                this.flightDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DepartureAirportCode
        {
            get
            {
                return this.departureAirportCode;
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
                return this.arrivalAirportCode;
            }
            set
            {
                this.arrivalAirportCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public MOBSeatMap SeatMap
        {
            get
            {
                return this.seatMap;
            }
            set
            {
                this.seatMap = value;
            }
        }

        public SHOPOnTimePerformance OnTimePerformance
        {
            get
            {
                return this.onTimePerformance;
            }
            set
            {
                this.onTimePerformance = value;
            }
        }
    }
}
