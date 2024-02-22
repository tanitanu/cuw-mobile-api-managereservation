using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.SeatMapEngine
{
    [Serializable()]
    public class MOBSHOPMessage
    {
        private string tripId = string.Empty;
        private string flightId = string.Empty;
        private string connectionIndex = string.Empty;
        private string flightNumberField = string.Empty;
        private string messageCode = string.Empty;
        private List<object> messageParameters;

        public string TripId
        {
            get
            {
                return this.tripId;
            }
            set
            {
                this.tripId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string FlightId
        {
            get
            {
                return this.flightId;
            }
            set
            {
                this.flightId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ConnectionIndex
        {
            get
            {
                return this.connectionIndex;
            }
            set
            {
                this.connectionIndex = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string FlightNumberField
        {
            get
            {
                return this.flightNumberField;
            }
            set
            {
                this.flightNumberField = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string MessageCode
        {
            get
            {
                return this.messageCode;
            }
            set
            {
                this.messageCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<object> MessageParameters
        {
            get
            {
                return this.messageParameters;
            }
            set
            {
                this.messageParameters = value;
            }
        }
    }
}
