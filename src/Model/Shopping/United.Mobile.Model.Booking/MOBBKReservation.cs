using System;
using System.Collections.Generic;
using United.Mobile.Model.Shopping.Booking;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBBKReservation
    {
        private string recordLocator = string.Empty;
        private string searchType = string.Empty;
        private List<MOBBKTrip> trips;
        private List<MOBBKPrice> prices;
        private List<MOBBKTax> taxes;
        private int numberOfTravelers;
        private List<MOBBKTraveler> travelers;
        private List<MOBSeatPrice> seatPrices;
        private string warning;


        public string RecordLocator
        {
            get
            {
                return this.recordLocator;
            }
            set
            {
                this.recordLocator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string SearchType
        {
            get
            {
                return this.searchType;
            }
            set
            {
                this.searchType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public List<MOBBKTrip> Trips
        {
            get
            {
                return this.trips;
            }
            set
            {
                this.trips = value;
            }
        }

        public List<MOBBKPrice> Prices
        {
            get
            {
                return this.prices;
            }
            set
            {
                this.prices = value;
            }
        }

        public List<MOBBKTax> Taxes
        {
            get
            {
                return this.taxes;
            }
            set
            {
                this.taxes = value;
            }
        }

        public int NumberOfTravelers
        {
            get
            {
                return this.numberOfTravelers;
            }
            set
            {
                this.numberOfTravelers = value;
            }
        }

        public List<MOBBKTraveler> Travelers
        {
            get
            {
                return this.travelers;
            }
            set
            {
                this.travelers = value;
            }
        }

        public List<MOBSeatPrice> SeatPrices
        {
            get
            {
                return this.seatPrices;
            }
            set
            {
                this.seatPrices = value;
            }
        }

        public string Warning
        {
            get { return warning; }
            set { warning = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
    }
}
