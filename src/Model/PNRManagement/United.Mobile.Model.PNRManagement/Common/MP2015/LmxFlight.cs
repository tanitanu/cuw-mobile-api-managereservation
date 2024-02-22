using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Common.MP2015
{
    [Serializable()]
    public class LmxFlight
    {
        private MOBAirline marketingCarrier;
        private string flightNumber = string.Empty;
        private MOBAirport departure;
        private MOBAirport arrival;
        private string scheduledDepartureDateTime = string.Empty;
        private List<LmxProduct> products;
        private bool nonPartnerFlight;

        public MOBAirline MarketingCarrier
        {
            get
            {
                return this.marketingCarrier;
            }
            set
            {
                this.marketingCarrier = value;
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
                this.flightNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public MOBAirport Departure
        {
            get
            {
                return this.departure;
            }
            set
            {
                this.departure = value;
            }
        }

        public MOBAirport Arrival
        {
            get
            {
                return this.arrival;
            }
            set
            {
                this.arrival = value;
            }
        }

        public string ScheduledDepartureDateTime
        {
            get
            {
                return this.scheduledDepartureDateTime;
            }
            set
            {
                this.scheduledDepartureDateTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<LmxProduct> Products
        {
            get
            {
                return this.products;
            }
            set
            {
                this.products = value;
            }
        }

        public bool NonPartnerFlight
        {
            get
            {
                return this.nonPartnerFlight;
            }
            set
            {
                this.nonPartnerFlight = value;
            }
        }
    }
}