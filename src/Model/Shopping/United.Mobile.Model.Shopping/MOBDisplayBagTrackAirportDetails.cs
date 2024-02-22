using System;

namespace United.Mobile.Model.Shopping
{
    public class MOBDisplayBagTrackAirportDetails
    {
        public string AirportNameMobile { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public string AirportCode { get; set; } = string.Empty;
        private string airportInfo; //St. Louis,MO(STL)

        public string AirportInfo
        {
            get { return airportInfo; }
            set { airportInfo = value; }
        }

        private string airportCityName; //St.Louis

        public string AirportCityName
        {
            get { return airportCityName; }
            set { airportCityName = value; }
        }

    }
}
