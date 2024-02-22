namespace United.Mobile.Model.Common
{
    public class FlightStatusSegment
    {
        public Airport DepartureAirport { get; set; } = null;
        public Airport ArrivalAirport { get; set; } = null;
        public string DepartureGate { get; set; } = string.Empty;
        public string ArrivalGate { get; set; } = string.Empty;
        public string DepartureTerminal { get; set; } = string.Empty;
        public string ArrivalTerminal { get; set; } = string.Empty;
        public string DepartureConcourse { get; set; } = string.Empty;
        public string ArrivalConcourse { get; set; } = string.Empty;
        public string ScheduledDepartureDate { get; set; } = string.Empty;
        public string ScheduledDepartureTime { get; set; } = string.Empty;
        public string EstimatedDepartureDate { get; set; } = string.Empty;
        public string EstimatedDepartureTime { get; set; } = string.Empty;
        public string ActualDepartureDate { get; set; } = string.Empty;
        public string ActualDepartureTime { get; set; } = string.Empty;
        public string ScheduledArrivalDate { get; set; } = string.Empty;
        public string ScheduledArrivalTime { get; set; } = string.Empty;
        public string EstimatedArrivalDate { get; set; } = string.Empty;
        public string EstimatedArrivalTime { get; set; } = string.Empty;
        public string ActualArrivalDate { get; set; } = string.Empty;
        public string ActualArrivalTime { get; set; } = string.Empty;
        public string Aircraft { get; set; } = string.Empty;
        public bool IsInBoundSegment { get; set; } = false;
        public string WeatherInfoUrl { get; set; } = string.Empty;
        public string FlightStatus { get; set; } = string.Empty;
        public string NoseNumber { get; set; } = string.Empty;
        public string TailNumber { get; set; } = string.Empty;
        public string ShipNumber { get; set; } = string.Empty;
        public string FlightNumber { get; set; } = string.Empty;
        public string EquipmentKey { get; set; } = string.Empty;
        public string ArrivalBagClaim { get; set; } = string.Empty;
        public string DepartureBagclaim { get; set; } = string.Empty;
    }
}