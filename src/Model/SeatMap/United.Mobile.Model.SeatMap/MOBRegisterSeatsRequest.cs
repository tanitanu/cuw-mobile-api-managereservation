using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.SeatMap
{
    [Serializable()]
    public class MOBRegisterSeatsRequest : MOBRequest
    {
        private string sessionId;
        private string cartId = string.Empty;
        private string flow = string.Empty;
        private string origin = string.Empty;
        private string destination = string.Empty;
        private string flightNumber = string.Empty;
        private string flightDate = string.Empty;
        private string paxIndex = string.Empty;
        private string seatAssignment = string.Empty;
        private string mileagePlusNumber = string.Empty;

        public string SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }

        public string CartId
        {
            get { return cartId; }
            set { cartId = value; }
        }
        public string Flow
        {
            get { return flow; }
            set { flow = value; }
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

        public string PaxIndex
        {
            get
            {
                return this.paxIndex;
            }
            set
            {
                this.paxIndex = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string SeatAssignment
        {
            get
            {
                return this.seatAssignment;
            }
            set
            {
                this.seatAssignment = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string MileagePlusNumber
        {
            get
            {
                return this.mileagePlusNumber;
            }
            set
            {
                this.mileagePlusNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        private bool continueToRegisterAncillary;
        public bool ContinueToRegisterAncillary
        {
            get { return continueToRegisterAncillary; }
            set { continueToRegisterAncillary = value; }
        }
    }
}
