using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Internal.SaveTrip
{
    public class EmpBookingSegment
    {
        public string FlightNumber { get; set; }
        public Airport Origin { get; set; }
        public Airport Destination { get; set; }
        public string DepartTime { get; set; }
        public string ArrivalTime { get; set; }
        public string ArrivalDate { get; set; }
        public string DepartDate { get; set; }
        public TravelTime TravelTime { get; set; }
        public string FirstClassBucket { get; set; }
        public string MarketingCarrier { get; set; }
        public string OperatingCarrier { get; set; }
        public int SegmentIndex { get; set; }
        public bool IsBusinessFirstEligible { get; set; }
        public bool IsBusinessCoachEligible { get; set; }
        public DEI DEI { get; set; }
        public TypeOption Cabin { get; set; }
        public string CurrentViewDate { get; set; }
        public int CabinCount { get; set; }
        public PositiveSpaceCost PSCost { get; set; }
        public EResPBTType Capacity { get; set; }
    }

}
