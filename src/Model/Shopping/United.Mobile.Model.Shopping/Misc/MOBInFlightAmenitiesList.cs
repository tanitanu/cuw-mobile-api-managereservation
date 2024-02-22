using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class InFlightAmenitiesList
    {
        private Airline airline;
        private Segment segment;
        private List<string> complimentaryFood;
        private List<string> beverages;
        private List<string> inflightEntertainment;
        private List<string> inSeatPower;
        private List<string> seating;
        private List<string> aircraft;
        private string departureAirportTimeStamp = string.Empty;
        private string flightTravelTime = string.Empty;
        private bool departed;

        public Airline Airline
        {
            get
            {
                return this.airline;
            }
            set
            {
                this.airline = value;
            }
        }

        public Segment Segment
        {
            get
            {
                return this.segment;
            }
            set
            {
                this.segment = value;
            }
        }

        public List<string> ComplimentaryFood
        {
            get
            {
                return this.complimentaryFood;
            }
            set
            {
                this.complimentaryFood = value;
            }
        }

        public List<string> Beverages
        {
            get
            {
                return this.beverages;
            }
            set
            {
                this.beverages = value;
            }
        }

        public List<string> InflightEntertainment
        {
            get
            {
                return this.inflightEntertainment;
            }
            set
            {
                this.inflightEntertainment = value;
            }
        }

        public List<string> InSeatPower
        {
            get
            {
                return this.inSeatPower;
            }
            set
            {
                this.inSeatPower = value;
            }
        }

        public List<string> Seating
        {
            get
            {
                return this.seating;
            }
            set
            {
                this.seating = value;
            }
        }

        public List<string> Aircraft
        {
            get
            {
                return this.aircraft;
            }
            set
            {
                this.aircraft = value;
            }
        }

        public string DepartureAirportTimeStamp
        {
            get
            {
                return this.departureAirportTimeStamp;
            }
            set
            {
                this.departureAirportTimeStamp = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string FlightTravelTime
        {
            get
            {
                return this.flightTravelTime;
            }
            set
            {
                this.flightTravelTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool Departed
        {
            get
            {
                return departed;
            }
            set
            {
                this.departed = value;
            }
        }
    }
}
