using System;

namespace United.Mobile.Model.Currency
{
    [Serializable]
    public class MOBCurrencyConverterResponse : MOBResponse
    {
        public MOBCurrencyConverter CurrencyConverter { get; set; }
    }
}
