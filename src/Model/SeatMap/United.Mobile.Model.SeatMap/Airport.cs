using System;

namespace United.Mobile.Model.SeatMap
{
    [Serializable]
    public class Airport
    {
        // Sample response from Couchbase Airport:
        // {"AirportNameMobile":"San Francisco, CA (SFO)","CityName":"San Francisco","AirportCode":"SFO","AirportName":"San Francisco, CA (SFO)"}
        public string Code { get; set; }
        public string NameShort { get; set; }
        public string NameLong { get; set; }
        public string City { get; set; }
        public string CityName { get; set; }
        public string AirportCode { get; set; }
    }
}
