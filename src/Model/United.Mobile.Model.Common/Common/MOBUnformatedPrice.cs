using System;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBReservationPrice
    {
        private string currencyShortCode;
        private string currencyCode;
        private double amount;
        private string priceTypeDescription;
        public string CurrencyCode
        {
            get { return this.currencyCode; }
            set { this.currencyCode = value; }
        }

        public string CurrencyShortCode
        {
            get { return this.currencyShortCode; }
            set { this.currencyShortCode = value; }
        }
        public double Amount
        {
            get { return this.amount; }
            set { this.amount = value; }
        }
        public string PriceTypeDescription
        {
            get { return this.priceTypeDescription; }
            set { this.priceTypeDescription = value; }
        }
    }
}
