
using System.Collections.Generic;

namespace United.Mobile.Model.Internal.Common
{
    public class Segment
    {
        public EResPBTType Available { get; set; }
        public string ArrivalDate { get; set; }
        public string ArrivalTime { get; set; }
        public EResPBTType Authorized { get; set; }        
        public bool BookBusinessCoach { get; set; }
        public bool BookBusinessFirst { get; set; }
        public EResPBTType Booked { get; set; }
        public EResPBTType Capacity { get; set; }
        public string CarrierMessage { get; set; }
        public string ConnectionTime { get; set; }
        public List<CodeValuePair> DEIs { get; set; }
        public string DepartureDate { get; set; }
        public string DepartureTime { get; set; }
        public Airport Destination { get; set; }
        public string Equipment { get; set; }
        public string EquipmentDescription { get; set; }
        public string FirstClassBucket { get; set; }
        public string FlightNumber { get; set; }
        public string FlightSubfleetInfo { get; set; }
        public int FlightWatchId { get; set; }
        public EResPBTType Group { get; set; }
        public EResPBTType Held { get; set; }
        public bool IsWatchedFlight { get; set; }
        public bool IsRSB { get; set; }
        public string HighestAvailabilityClass { get; set; }
        public string MarketingCarrier { get; set; }
        public TypeOption Meal { get; set; }
        public string Miles { get; set; }
        public List<string> Notes { get; set; }
        public string OperatingCarrier { get; set; }
        public Airport Origin { get; set; }
        public EResPBTType PS { get; set; }
        public PositiveSpaceCost PSCost { get; set; }
        public EResPBTType RSB { get; set; }
        public EResPBTType SA { get; set; }
        public string TailNumber { get; set; }
        public TravelTime TravelTime { get; set; }
        public string UpgradableStandBy { get; set; }
        public string ActualDepatureTime { get; set; }
        public string FareClasAvailability { get; set; }
        public int DepartureGMTOfffsetMinutes { get; set; }
        public int ArrivalGMTOfffsetMinutes { get; set; }
        public Umnrcount UMNRcount { get; set; }
    }
}
