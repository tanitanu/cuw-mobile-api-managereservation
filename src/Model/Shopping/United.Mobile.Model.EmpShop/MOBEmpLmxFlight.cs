using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBEmpLmxFlight
    {
        private MOBAirline marketingCarrier;
        private string flightNumber = string.Empty;
        private MOBAirport departure;
        private MOBAirport arrival;
        private string scheduledDepartureDateTime = string.Empty;
        private List<MOBEmpLmxProduct> products;
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

        public List<MOBEmpLmxProduct> Products
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
