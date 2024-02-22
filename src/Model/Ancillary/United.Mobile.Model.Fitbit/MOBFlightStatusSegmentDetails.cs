using System;

namespace United.Mobile.Model.Fitbit
{
    [Serializable]
    public class MOBFlightStatusSegmentDetails
    {
        public string StatusShort { get; set; } = string.Empty;


        public string StatusDescription { get; set; } = string.Empty;


        public string StatusTitle { get; set; } = string.Empty;


        public string StatusSubTitle { get; set; } = string.Empty;
    }
}
