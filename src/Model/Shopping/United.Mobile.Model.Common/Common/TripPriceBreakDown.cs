using System;
using United.Mobile.Model.Shopping.PriceBreakDown;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class TripPriceBreakDown
    {
        public PriceBreakDownSummary PriceBreakDownSummary { get; set; }

        public PriceBreakDownDetails PriceBreakDownDetails { get; set; }
    }
}
