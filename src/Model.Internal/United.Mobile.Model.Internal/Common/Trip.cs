namespace United.Mobile.Model.Internal.Common
{
    public class Trip
    {
        public int TripID { get; set; }
        public string DepartDate { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Time { get; set; }
        public string[] DependantID { get; set; }
        public int OriginMiles { get; set; }
        public int DestinationMiles { get; set; }
    }

}
