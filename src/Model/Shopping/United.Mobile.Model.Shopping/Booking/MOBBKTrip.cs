using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.Booking
{
    [Serializable()]
    public class MOBBKTrip : MOBBKTripBase
    {
        private string tripId = string.Empty;
        private string originDecoded = string.Empty;
        private string destinationDecoded = string.Empty;
        private List<MOBBKFlattenedFlight> flattenedFlights;
        private List<MOBBKFlightSection> flightSections;

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

        public string OriginDecoded
        {
            get
            {
                return this.originDecoded;
            }
            set
            {
                this.originDecoded = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DestinationDecoded
        {
            get
            {
                return this.destinationDecoded;
            }
            set
            {
                this.destinationDecoded = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<MOBBKFlattenedFlight> FlattenedFlights
        {
            get
            {
                return this.flattenedFlights;
            }
            set
            {
                this.flattenedFlights = value;
            }
        }

        public List<MOBBKFlightSection> FlightSections
        {
            get
            {
                return this.flightSections;
            }
            set
            {
                this.flightSections = value;
            }
        }
    }
}
