using System;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class BagFeesPerSegment
    {
        public string FlightTravelDate { get; set; } = string.Empty;

        public string OriginAirportCode { get; set; } = string.Empty;

        public string OriginAirportName { get; set; } = string.Empty;

        public string DestinationAirportCode { get; set; } = string.Empty;

        public string DestinationAirportName { get; set; } = string.Empty;

        public string FirstBagFee { get; set; } = string.Empty;

        public string RegularFirstBagFee { get; set; } = string.Empty;

        public string SecondBagFee { get; set; } = string.Empty;

        public string RegularSecondBagFee { get; set; } = string.Empty;

        public string WeightPerBag { get; set; } = string.Empty;

        public bool IsFirstBagFree { get; set; }
        public bool IsSecondBagFree { get; set; }
        public string CabinName { get; set; } = string.Empty;
    }
}
