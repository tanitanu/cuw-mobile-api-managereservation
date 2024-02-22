using System;

namespace United.Mobile.Model.FlightStatus
{
    [Serializable]
    public class FlightStatusSegmentDetails
    {
        public string StatusShort { get; set; }

        public string StatusDescription { get; set; }

        public string StatusTitle { get; set; }

        public string StatusSubTitle { get; set; }

    }
}