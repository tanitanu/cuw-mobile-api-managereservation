using System;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class GeoFence
    {
        private int id;
        private string identifier;
        private string name;
         private Double longitude;
        private Double latitude;
        //private Location location;
        private double radius;
        private string airportCode;
        
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        
        public string Identifier
        {
            get { return identifier; }
            set { identifier = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
       
        //public Location Location
        //{
        //    get { return location; }
        //    set { location = value; }
        //}

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

        public double Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        public string AirportCode
        {
            get { return airportCode; }
            set { airportCode = value; }
        }
    }
}
