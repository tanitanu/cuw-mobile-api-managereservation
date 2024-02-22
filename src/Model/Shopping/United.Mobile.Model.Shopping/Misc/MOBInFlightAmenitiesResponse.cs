using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class InFlightAmenitiesResponse : MOBResponse
    {
        private string flightNumber = string.Empty;
        private string flightDate = string.Empty;
        private string departureAirportCode = string.Empty;
        private string arrivalAirportCode = string.Empty;
        private InFlightAmenitiesList inFlightAmenitiesList;
        private string carrierCode = string.Empty;

        public InFlightAmenitiesResponse()
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

        
        public InFlightAmenitiesList InFlightAmenitiesList
        {
            get
            {
                return inFlightAmenitiesList;
            }
            set
            {
                this.inFlightAmenitiesList = value;
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
