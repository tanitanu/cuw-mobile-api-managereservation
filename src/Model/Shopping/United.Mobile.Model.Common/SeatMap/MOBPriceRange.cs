using System;

namespace United.Mobile.Model.Shopping
{
    public class MOBPriceRange
    {
        public string SeatLegend { get; set; }
        public string SeatFeature { get; set; }
        public decimal SeatPrice { get; set; } = 0;
        public decimal PcuOfferPrice { get; set; }
        public bool IsPcuOfferEligible { get; set; }
        public bool HasAvailableSeats { get; set; }
        public bool HasEnoughPcuSeats { get; set; }
    }
}
