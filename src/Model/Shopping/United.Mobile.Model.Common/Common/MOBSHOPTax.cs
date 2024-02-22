using System;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBSHOPTax
    {
        public decimal Amount { get; set; }

        public string DisplayAmount { get; set; } = string.Empty;

        public string CurrencyCode { get; set; } = string.Empty;

        public decimal NewAmount { get; set; }

        public string DisplayNewAmount { get; set; } = string.Empty;

        public string TaxCode { get; set; } = string.Empty;

        public string TaxCodeDescription { get; set; } = string.Empty;
    }
}
