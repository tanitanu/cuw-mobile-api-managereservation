using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.SeatMapEngine
{
    [Serializable()]
    public class MOBSHOPTax
    {
        private decimal amount;
        private string displayAmount = string.Empty;
        private string currencyCode = string.Empty;
        private decimal newAmount;
        private string displayNewAmount = string.Empty;
        private string taxCode = string.Empty;
        private string taxCodeDescription = string.Empty;

        public decimal Amount
        {
            get
            {
                return this.amount;
            }
            set
            {
                this.amount = value;
            }
        }

        public string DisplayAmount
        {
            get
            {
                return this.displayAmount;
            }
            set
            {
                this.displayAmount = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string CurrencyCode
        {
            get
            {
                return this.currencyCode;
            }
            set
            {
                this.currencyCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public decimal NewAmount
        {
            get
            {
                return this.newAmount;
            }
            set
            {
                this.newAmount = value;
            }
        }

        public string DisplayNewAmount
        {
            get { return this.displayNewAmount; }
            set { this.displayNewAmount = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        public string TaxCode
        {
            get { return this.taxCode; }
            set { this.taxCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string TaxCodeDescription
        {
            get { return this.taxCodeDescription; }
            set { this.taxCodeDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
    }
}
