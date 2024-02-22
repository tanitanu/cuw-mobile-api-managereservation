using System;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class TravelOptionSubItem
    {
        public double Amount { get; set; }

        public string DisplayAmount { get; set; } = string.Empty;

        public string CurrencyCode { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Key { get; set; } = string.Empty;

        public string ProductId { get; set; } = string.Empty;

        public object Value { get; set; }

    }

}
