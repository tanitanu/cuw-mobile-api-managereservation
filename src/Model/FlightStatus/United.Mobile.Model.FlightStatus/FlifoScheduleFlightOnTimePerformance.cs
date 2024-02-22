using System;

namespace United.Mobile.Model.FlightStatus
{
    [Serializable()]
    public class FlifoScheduleFlightOnTimePerformance
    {
        public string EffectiveDate { get; set; }

        public string PctOnTimeCancelled { get; set; }

        public string PctOnTimeDelayed { get; set; }

        public string PctOnTimeMax { get; set; }

        public string PctOnTimeMin { get; set; }
    }
}