using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBLmxLoyaltyTier
    {
        public string Description { get; set; } = string.Empty;

        public string Key { get; set; } = string.Empty;

        public int Level { get; set; }

        public List<MOBLmxQuote> LmxQuotes { get; set; }
    }
}
