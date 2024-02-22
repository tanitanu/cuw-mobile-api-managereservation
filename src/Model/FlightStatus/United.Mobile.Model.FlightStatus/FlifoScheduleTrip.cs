using System;

namespace United.Mobile.Model.FlightStatus
{
    [Serializable()]
    public class FlifoScheduleTrip
    {
        public string DepartureDate { get; set; }

        public string DepartureTime { get; set; }

        public string Destination { get; set; }

        public FlifoScheduleFlight[] Flights;

        public string GroundTime { get; set; }

        public string JourneyTime { get; set; }

        public string OperationDays { get; set; }

        public string Origin { get; set; }

        public string Stops { get; set; }

        public string TripNumber { get; set; }

    }
}