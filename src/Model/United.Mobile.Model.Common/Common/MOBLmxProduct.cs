using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBLmxProduct
    {
        public string BookingCode { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public List<MOBLmxLoyaltyTier> LmxLoyaltyTiers { get; set; }

        public string ProductType { get; set; } = string.Empty;

        public MOBLmxProduct()
        {
            LmxLoyaltyTiers = new List<MOBLmxLoyaltyTier>();
        }
    }
}
