namespace United.Mobile.Model.Common
{
    public class PassengerListRequest : EResBaseRequest
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string FlightDate { get; set; }
        public string FlightTime { get; set; }
        public string FlightNumber { get; set; }
        public string CarrierCode { get; set; }
        public string EquipmentCode { get; set; }
        public string EquipmentDesc { get; set; }
        public string TailNumber { get; set; }
        public bool ShowPosition { get; set; }
        public bool IsMobile { get; set; }
        public bool ShowFlightStatus { get; set; }
        public string TravelerListIndicator { get; set; }
        public string TravelerListOption { get; set; }
        public bool IsConnectingFlight { get; set; }
        public string OperatingCarrier { get; set; }
    }
 }
