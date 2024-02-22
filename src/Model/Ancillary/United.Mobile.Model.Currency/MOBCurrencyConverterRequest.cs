using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Currency
{
    [Serializable]
    public class MOBCurrencyConverterRequest : MOBRequest
    {
        public long SessionID { get; set; }
        public string FromCurrencyCode { get; set; } = string.Empty;
        public string ToCurrencyCode { get; set; } = string.Empty;
        public double Amount { get; set; }
        public string MessageFormat { get; set; } = string.Empty;
    }
}
