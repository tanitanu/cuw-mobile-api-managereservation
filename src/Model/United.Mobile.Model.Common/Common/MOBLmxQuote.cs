using System;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBLmxQuote
    {
        public string Amount { get; set; } = string.Empty;

        public string Currency { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public double DblAmount { get; set; }

        public string CurrencySymbol { get; set; } = string.Empty;
    }
}
