using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBBKFlattenedFlight
    {
        private string tripId = string.Empty;
        private string flightId = string.Empty;
        private List<MOBBKFlight> flights;

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

        public List<MOBBKFlight> Flights
        {
            get
            {
                return this.flights;
            }
            set
            {
                this.flights = value;
            }
        }
    }
}
