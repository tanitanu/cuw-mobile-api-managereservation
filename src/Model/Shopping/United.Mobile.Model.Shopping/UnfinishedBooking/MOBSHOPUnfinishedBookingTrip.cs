using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping.UnfinishedBooking
{
    [Serializable]
    public class MOBSHOPUnfinishedBookingTrip
    {
        private string departDate;
        private string departTime;
        private string destination;
        private string origin;
        private List<MOBSHOPUnfinishedBookingFlight> flights = new List<MOBSHOPUnfinishedBookingFlight>();
        private string arrivalTime;
        private string arrivalDate;
        private string departDateTimeGMT;

        /// <example>
        /// "3/21/2018"
        /// </example>
        public string DepartDate
        {
            get { return departDate; }
            set { departDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        /// <example>
        /// "05:30:00 AM"
        /// </example>
        public string DepartTime
        {
            get { return departTime; }
            set { departTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        /// <example>
        /// "DEN"
        /// </example>
        public string Destination
        {
            get { return destination; }
            set { destination = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        /// <example>
        /// "07:30:00 AM"
        /// </example>
        public string ArrivalTime
        {
            get { return arrivalTime; }
            set { arrivalTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        /// <example>
        /// "8/21/2018"
        /// </example>
        public string ArrivalDate
        {
            get { return arrivalDate; }
            set { arrivalDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        /// <example>
        /// "ORD"
        /// </example>
        public string Origin
        {
            get { return origin; }
            set { origin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public List<MOBSHOPUnfinishedBookingFlight> Flights
        {
            get { return flights; }
            set { flights = value; }
        }

        /// <example>
        /// "2018-05-31T13:10+02:00"
        /// </example>
        public string DepartDateTimeGMT
        {
            get { return departDateTimeGMT; }
            set { departDateTimeGMT = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
    }
}
