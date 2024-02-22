using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class CurrencyConverter
    {
        private string fromCurrencyCode = string.Empty;
        private string toCurrencyCode = string.Empty;
        private double fromAmount;
        private double toAmount;
        private double conversionRate;
        private string dateValid = string.Empty;
        private string fromCurrencyDescription = string.Empty;
        private string toCurrencyDescription = string.Empty;

        public string FromCurrencyCode
        {
            get { return this.fromCurrencyCode; }
            set { this.fromCurrencyCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string ToCurrencyCode
        {
            get { return this.toCurrencyCode; }
            set { this.toCurrencyCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public double FromAmount
        {
            get { return this.fromAmount; }
            set { this.fromAmount = value; }
        }

        public double ToAmount
        {
            get { return this.toAmount; }
            set { this.toAmount = value; }
        }

        public double ConversionRate
        {
            get { return this.conversionRate; }
            set { this.conversionRate = value; }
        }

        public string DateValid
        {
            get { return this.dateValid; }
            set { this.dateValid = value; }
        }

        public string FromCurrencyCodeDescription
        {
            get { return this.fromCurrencyDescription; }
            set { this.fromCurrencyDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string ToCurrencyCodeDescription
        {
            get { return this.toCurrencyDescription; }
            set { this.toCurrencyDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
    }
}
