using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping.UnfinishedBooking
{ 
    [Serializable]
    public class MOBSHOPUnfinishedBookingFlight
    {
        private string bookingCode;
        private string productType;
        private List<MOBSHOPUnfinishedBookingFlight> connections;
        private string departDateTime;
        private string origin;
        private string destination;
        private string flightNumber;
        private string marketingCarrier;
        private List<MOBSHOPUnfinishedBookingFlightProduct> products;

        /// <example>
        /// "S"
        /// </example>
        public string BookingCode
        {
            get { return bookingCode; }
            set { bookingCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        /// <example>
        /// "3/21/2018 7:45:00 AM"
        /// </example>
        public string DepartDateTime
        {
            get { return departDateTime; }
            set { departDateTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        /// <example>
        /// "ORD"
        /// </example>
        public string Origin
        {
            get { return origin; }
            set { origin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
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
        /// "675"
        /// </example>
        public string FlightNumber
        {
            get { return flightNumber; }
            set { flightNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        /// <example>
        /// "UA"
        /// </example>
        public string MarketingCarrier
        {
            get { return marketingCarrier; }
            set { marketingCarrier = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string ProductType
        {
            get { return productType; }
            set { productType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public List<MOBSHOPUnfinishedBookingFlight> Connections
        {
            get { return connections; }
            set { connections = value; }
        }
        public List<MOBSHOPUnfinishedBookingFlightProduct> Products
        {
            get { return products; }
            set { products = value; }
        }

    }
}
