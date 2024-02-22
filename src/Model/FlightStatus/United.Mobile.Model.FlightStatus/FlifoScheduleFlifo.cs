using System;

namespace United.Mobile.Model.FlightStatus
{
    [Serializable()]
    public class FlifoScheduleFlifo
    {
        public string ActualDepartureTime { get; set; }

        public string EstimatedArrivalTime { get; set; }

        public string EstimatedDepartureTime { get; set; }

        public string MinutesDelayed { get; set; }

        public string OutTime { get; set; }

        public string ScheduledArrivalTime { get; set; }

        public string ScheduledDepartureTime { get; set; }

        public string StatusCode { get; set; }

        public string StatusMessage { get; set; }

    }
}
