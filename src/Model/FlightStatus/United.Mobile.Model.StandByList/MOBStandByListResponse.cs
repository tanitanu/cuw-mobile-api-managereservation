using System;

namespace United.Mobile.Model.StandByList
{
    [Serializable]
    public class MOBStandByListResponse : MOBResponse
    {
        private string flightNumber = string.Empty;
        private string flightDate = string.Empty;
        private string departureAirportCode = string.Empty;
        private string carrierCode = string.Empty;
        private MOBStandbyList standbyList;

        public MOBStandByListResponse()
            : base()
        {
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

        public MOBStandbyList StandbyList
        {
            get
            {
                return standbyList;
            }
            set
            {
                this.standbyList = value;
            }
        }

        public string CarrierCode
        {
            get
            {
                return carrierCode;
            }
            set
            {
                this.carrierCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
    }
}
