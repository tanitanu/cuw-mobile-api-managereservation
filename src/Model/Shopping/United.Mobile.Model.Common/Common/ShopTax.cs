using System;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class ShopTax
    {
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
        public decimal NewAmount { get; set; }
        public string TaxCode { get; set; } = string.Empty;
        public string TaxCodeDescription { get; set; } = string.Empty;
    }
}
