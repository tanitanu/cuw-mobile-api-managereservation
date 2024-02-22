using System;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class ShopBookingInfo
    {
        public string BookingCode { get; set; } = string.Empty;
        public decimal COCTotal { get; set; }
        public string ExtendedFareCode { get; set; } = string.Empty;
        public string FareBasisCode { get; set; } = string.Empty;
        public string FareIndex { get; set; } = string.Empty;
        public string FareInfoHash { get; set; } = string.Empty;
        public string PaxIds { get; set; } = string.Empty;
        public string PaxPricingIndex { get; set; } = string.Empty;
        public decimal SaleFareTotal { get; set; }
        public decimal SaleTaxTotal { get; set; }
        public int Segmentindex { get; set; }
        public string SliceIndex { get; set; } = string.Empty;
        public string TicketDesignator { get; set; } = string.Empty;
    }
}
