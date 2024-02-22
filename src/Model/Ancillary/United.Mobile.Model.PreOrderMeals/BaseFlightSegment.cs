using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.PreOrderMeals
{
    [Serializable]
    public class BaseFlightSegment
    {
        private string flightNumber = string.Empty;
        private string tripNumber = string.Empty;
        private int segmentNumber;

        public string FlightNumber
        {
            get { return this.flightNumber; }
            set { this.flightNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string TripNumber
        {
            get { return this.tripNumber; }
            set { this.tripNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public int SegmentNumber
        {
            get { return this.segmentNumber; }
            set { this.segmentNumber = value; }
        }

        private FlightTransit departure;

        public FlightTransit Departure
        {
            get
            {
                if (departure == null)
                {
                    departure = new FlightTransit();
                }

                return departure;
            }
            set { departure = value; }
        }

        private FlightTransit arrival;

        public FlightTransit Arrival
        {
            get
            {
                if (arrival == null)
                {
                    arrival = new FlightTransit();
                }

                return arrival;
            }
            set { arrival = value; }
        }

        private string operatingAirlineCode;

        public string OperatingAirlineCode
        {
            get { return operatingAirlineCode; }
            set { operatingAirlineCode = value; }
        }

        private string aircraftModelCode;

        public string AircraftModelCode
        {
            get { return aircraftModelCode; }
            set { aircraftModelCode = value; }
        }

        private string meal = string.Empty;
        public string MealType
        {
            get { return this.meal; }
            set { this.meal = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
    }
}
