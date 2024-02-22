using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class CurrencyConverterResponse : MOBResponse
    {
        private CurrencyConverter currencyConverter;

        public CurrencyConverter CurrencyConverter
        {
            get
            {
                return this.currencyConverter;
            }
            set
            {
                this.currencyConverter = value;
            }
        }
    }
}
