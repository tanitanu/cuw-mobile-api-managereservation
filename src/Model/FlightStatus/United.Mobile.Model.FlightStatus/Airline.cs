using System;

namespace United.Mobile.Model.FlightStatus
{
    [Serializable]
    public class Airline
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string FlightNumber { get; set; } = string.Empty;
    }
}
