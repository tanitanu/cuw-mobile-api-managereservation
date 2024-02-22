namespace United.Mobile.Model.Airport
{
    public class AirportGeoInfo
    {
        public string AirportCode { get; set; }
        public string AirportDescription { get; set; }
        public string CountryCode { get; set; } = string.Empty;
        public string Distance { get; set; } = string.Empty;
        public string City { get; set; }
        public string Country { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string StateCode { get; set; } = string.Empty;
    }
}
