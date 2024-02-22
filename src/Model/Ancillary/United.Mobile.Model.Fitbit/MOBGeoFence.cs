using System;

namespace United.Mobile.Model.Fitbit
{
    [Serializable()]
    public class MOBGeoFence
    {
        public int Id { get; set; } 

        public string Identifier { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        //public MOBLocation Location
        //{
        //    get { return location; }
        //    set { location = value; }
        //}

        public Double Longitude { get; set; } 


        public Double Latitude { get; set; } 


        public double Radius { get; set; } 


        public string AirportCode { get; set; } = string.Empty;
    }
}
