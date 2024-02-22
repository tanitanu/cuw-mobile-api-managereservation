using System.Collections.Generic;
using United.Mobile.Model.Internal.PassRiders;

namespace United.Mobile.Model.Internal.Common
{
    public class BookingSegment
    {
        public EResPBTType Available { get; set; }
        public string ArrivalDate { get; set; }
        public string ArrivalTime { get; set; }
        public EResPBTType Authorized { get; set; }
        public bool BookBusinessCoach { get; set; }
        public List<PassengerCOS> ClassOfService { get; set; }
        public EResPBTType Booked { get; set; }
        public bool BookBusinessFirst { get; set; }
        public TypeOption Cabin { get; set; }
        public int CabinCount { get; set; }
        public EResPBTType Capacity { get; set; }
        public string ConnectionTime { get; set; }
        public string CurrentViewDate { get; set; }
        public List<CodeValuePair> DEIs { get; set; }
        public DEI DEI { get; set; }
        public string DepartDate { get; set; }
        public string DepartTime { get; set; }
        public Airport Destination { get; set; }
        public string Equipment { get; set; }
        public string EquipmentDescription { get; set; }
        public string FirstClassBucket { get; set; }
        public string FlightNumber { get; set; }
        public string FlightSubfleetInfo { get; set; }
        public string MarketingCarrier { get; set; }
        public int Miles { get; set; }
        public string OperatingCarrier { get; set; }
        public Airport Origin { get; set; }
        public PositiveSpaceCost PSCost { get; set; }
        public int SegmentIndex { get; set; }
        public EResPBTType SAListed { get; set; }
        public string TailNumber { get; set; }
        public TravelTime TravelTime { get; set; }
        public string TotalJourneyDuration { get; set; }
        public EResPBTType PS { get; set; }
        public EResPBTType SA { get; set; }
        public EResPBTType Held { get; set; }
        public EResPBTType Group { get; set; }
        public EResPBTType RSB { get; set; }
        public List<PassengerCabinInfo> PassengerCabinInfo { get; set; }
        public int DepartureGMTOfffsetMinutes { get; set; }
        public int ArrivalGMTOfffsetMinutes { get; set; }
        public BookingSegment()
        {
            ClassOfService = new List<PassengerCOS>();
            DEIs = new List<CodeValuePair>();
            PassengerCabinInfo = new List<PassengerCabinInfo>();

        }
    }
}
