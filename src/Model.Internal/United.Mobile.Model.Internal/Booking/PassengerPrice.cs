using System.Collections.Generic;
using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Internal.Booking
{
    public class PassengerPrice
    {
        public string BaseFare { get; set; }
        public string Destination { get; set; }
        public string DisplacementCost { get; set; }
        public bool IsEligibleForImputedTax { get; set; }
        public string Origin { get; set; }
        public string PassengerName { get; set; }
        public string PricingTravelerType { get; set; }
        public decimal RawBaseFare { get; set; }
        public RelationShip RelationShip { get; set; }
        public object SegmentImputedTaxDetails { get; set; }
        public int SegMile { get; set; }
        public decimal SegMileFare { get; set; }
        public List<Tax> Taxes { get; set; }
        public string TotalFare { get; set; }
        public decimal TotalFareRaw { get; set; }
        public decimal TotalImputedTax { get; set; }

    }
}
