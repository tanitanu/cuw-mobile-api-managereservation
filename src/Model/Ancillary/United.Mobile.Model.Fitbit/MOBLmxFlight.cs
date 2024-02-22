using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Fitbit
{
    [Serializable()]
    public class MOBLmxFlight
    {
        public MOBAirline MarketingCarrier { get; set; }

        public string FlightNumber { get; set; } = string.Empty;

        public MOBAirport Departure { get; set; }

        public MOBAirport Arrival { get; set; }

        public string ScheduledDepartureDateTime { get; set; } = string.Empty;

        public List<MOBLmxProduct> Products { get; set; }

        public bool NonPartnerFlight { get; set; }

        public MOBLmxFlight()
        {
            Products = new List<MOBLmxProduct>();
        }
    }
}
