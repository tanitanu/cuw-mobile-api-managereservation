using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class FlightStatusRequest : MOBRequest
    {
        private int flightNumber;
        private string flightDate = string.Empty;
        private string origin = string.Empty;
        private string carrierCode = string.Empty;
        private string destination = string.Empty;
        private string currentFlightShipId;
        private string viewedFlightScheduledDepartureTimeGMT;

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

        public string Origin
        {
            get
            {
                return this.origin;
            }
            set
            {
                this.origin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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
                this.carrierCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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
                this.destination = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string CurrentFlightShipId
        {
            get
            {
                return this.currentFlightShipId;
            }
            set
            {
                this.currentFlightShipId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ViewedFlightScheduledDepartureTimeGMT
        {
            get
            {
                return this.viewedFlightScheduledDepartureTimeGMT;
            }
            set
            {
                this.viewedFlightScheduledDepartureTimeGMT = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
