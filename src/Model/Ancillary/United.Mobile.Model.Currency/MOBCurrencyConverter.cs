using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Currency
{
    [Serializable]
    public class MOBCurrencyConverter
    {

        public string FromCurrencyCode { get; set; } = string.Empty;

        public string ToCurrencyCode { get; set; } = string.Empty;

        public double FromAmount { get; set; }


        public double ToAmount { get; set; }


        public double ConversionRate { get; set; }

        public string DateValid { get; set; }

        public string FromCurrencyCodeDescription { get; set; }

        public string ToCurrencyCodeDescription { get; set; }
    }
}
