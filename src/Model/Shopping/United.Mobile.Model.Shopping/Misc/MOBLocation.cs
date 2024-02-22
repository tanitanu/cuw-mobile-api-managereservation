using System;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class Location
    {
        private Double longitude;
        private Double latitude;

        public Location()
        {
        }

        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public Double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        public Double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }
    }
}
