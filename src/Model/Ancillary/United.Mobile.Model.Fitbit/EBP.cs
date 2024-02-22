using System;

namespace United.Mobile.Model.Fitbit
{
    [Serializable]
    public class EBP
    {
        public string FlightNumber { get; set; } = string.Empty;

        public string FlightDate { get; set; } = string.Empty;

        public Airport Departure { get; set; }

        public Airport Arrival { get; set; }

        public string DepartureDate { get; set; } = string.Empty;

        public string ArrivalDate { get; set; } = string.Empty;

        public string DepartureTime { get; set; } = string.Empty;

        public string ArrivalTime { get; set; } = string.Empty;

        public string BoardGate { get; set; } = string.Empty;

        public string BoardStartTime { get; set; } = string.Empty;

        public string BoardEndTime { get; set; } = string.Empty;

        public string BoardGroup { get; set; } = string.Empty;

        public string PassengerName { get; set; } = string.Empty;

        public string Seat { get; set; } = string.Empty;

        public string SequenceNumber { get; set; } = string.Empty;

        public string COS { get; set; } = string.Empty;

        public string MileagePlusNumber { get; set; } = string.Empty;

        public string MileagePlusAccountStatus { get; set; } = string.Empty;

        public Barcode Barcode { get; set; }
    }
}