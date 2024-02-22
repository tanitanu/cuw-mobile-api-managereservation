using System;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class ShopPricesCommon
    {
        public decimal BrokenOutYQSurcharges { get; set; }
        public decimal BusinessAirfare { get; set; }
        public decimal CheapestAirfare { get; set; }
        public decimal CheapestAirfareNoConnections { get; set; }
        public decimal CheapestAirfareWithConnections { get; set; }
        public decimal CheapestAirfareWithConnNonPartner { get; set; }
        public decimal CheapestAirfareWithConnPartner { get; set; }
        public decimal CheapestAltDate { get; set; }
        public decimal CheapestNearbyAirport { get; set; }
        public decimal CheapestRefundable { get; set; }
        public decimal CheapestWithoutOffer { get; set; }
        public decimal FirstClassAirfare { get; set; }
        public string FirstClassAirfareNotShownReason { get; set; }
        public decimal FullYAirfare { get; set; }
        public decimal MostExpensiveAirfare { get; set; }
        public decimal RefundableAirfare { get; set; }
        public decimal RefundableAverageAirfarePerPax { get; set; }
    }
}
