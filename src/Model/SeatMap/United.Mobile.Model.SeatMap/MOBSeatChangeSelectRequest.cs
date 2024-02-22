using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.SeatMap
{
    [Serializable]
    public class MOBSeatChangeSelectRequest : MOBRequest
    {
        private string sessionId = string.Empty;
        private string origin = string.Empty;
        private string destination = string.Empty;
        private string flightNumber = string.Empty;
        private string flightDate = string.Empty;
        private string paxIndex = string.Empty;
        private string seatAssignment = string.Empty;
        private string nextOrigin = string.Empty;
        private string nextDestination = string.Empty;
        private string catalogValues = string.Empty;
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

        public string NextOrigin
        {
            get
            {
                return this.nextOrigin;
            }
            set
            {
                this.nextOrigin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string NextDestination
        {
            get
            {
                return this.nextDestination;
            }
            set
            {
                this.nextDestination = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string CatalogValues
        {
            get
            {
                return this.catalogValues;
            }
            set
            {
                this.catalogValues = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
