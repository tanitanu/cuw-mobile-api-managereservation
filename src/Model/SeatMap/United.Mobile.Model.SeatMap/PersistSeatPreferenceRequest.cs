using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Services.FlightShopping.Common.FlightReservation;

namespace United.Mobile.Model.SeatMap
{
    public class PersistSeatPreferenceRequest
    {
        public string RecordLocator { get; set; }
        public string LastName { get; set; }
        public bool IsBE { get; set; } = false;
        public bool IsLegRoom { get; set; } = false;
        public bool IsStandby { get; set; } = false;
        public bool IsMultiPax { get; set; } = false;
        public bool RequestSeatsTogether { get; set; } = false;
        public bool IsRemembered { get; set; }
        public bool IsSemiSignedIn { get; set; }
        public string MileagePlusId { get; set; }
        public string LangCode { get; set; }
        public string SessionId { get; set; }
        //public string CarrierCode { get; set; }
        //public string BookingCode { get; set; }

        public string MarketingCarrierCode { get; set; }
        public string BookClassOfServiceCode { get; set; }
        public string EntryPoint { get; set; }
        public List<FlightDetails> FlightDetails { get; set; }

    }
    public class FlightDetails
    {
        public string MarketingCarrierCode { get; set; }
        public string FlightNumber { get; set; } = string.Empty;
        public string FlightDate { get; set; } = string.Empty;
        public string EstimatedArrivalTime { get; set; } = string.Empty;
        public string EstimatedDepartureTime { get; set; } = string.Empty;
        public string scheduledArrivalTime { get; set; } = string.Empty;
        public string scheduledDepartureTime { get; set; } = string.Empty;
    }

}
