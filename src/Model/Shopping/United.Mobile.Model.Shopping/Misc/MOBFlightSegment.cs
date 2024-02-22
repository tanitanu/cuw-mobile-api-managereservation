using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class FlightSegment
    {
        private string arrivalAirport;

        private string arrivalAirportName;

        private string carrierCode;

        private string departureAirport;

        private string departureAirportName;

        private string departureDate;

        private string flightNumber;



        public string ArrivalAirport
        {
            get
            {
                return this.arrivalAirport;
            }
            set
            {
                this.arrivalAirport= value;
            }
        }



        public string ArrivalAirportName
        {
            get
            {
                return this.arrivalAirportName;
            }
            set
            {
                this.arrivalAirportName= value;
            }
        }



        public string CarrierCode
        {
            get
            {
                return this.carrierCode;
            }
            set
            {
                this.carrierCode= value;
            }
        }



        public string DepartureAirport
        {
            get
            {
                return this.departureAirport;
            }
            set
            {
                this.departureAirport= value;
            }
        }



        public string DepartureAirportName
        {
            get
            {
                return this.departureAirportName;
            }
            set
            {
                this.departureAirportName= value;
            }
        }



        public string DepartureDate
        {
            get
            {
                return this.departureDate;
            }
            set
            {
                this.departureDate= value;
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
                this.flightNumber= value;
            }
        }
    }
}
