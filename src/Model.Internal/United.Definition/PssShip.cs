namespace United.Mobile.Model.Internal.Common
{
    public class PssShip
    {
        public string Carrier { get; set; } = string.Empty;

        public int Ship { get; set; }

        public string AircraftType { get; set; } = string.Empty;

        public string IFEType { get; set; } = string.Empty;

        public string Powertype { get; set; } = string.Empty;

        public string RegistrationId { get; set; } = string.Empty;

        public int NumberOfCabins { get; set; }

        public string WifiVendor { get; set; } = string.Empty;
    }
}