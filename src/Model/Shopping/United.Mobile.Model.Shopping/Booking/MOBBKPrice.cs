using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping.Booking
{
    [Serializable()]
    public class MOBBKPrice
    {
        private string priceIndex = string.Empty;
        private string currencyCode = string.Empty;
        private string priceType = string.Empty;
        private string displayType = string.Empty;
        private string displayValue = string.Empty;
        private string totalBaseFare = string.Empty;
        private string totalOtherTaxes = string.Empty;
        private string formattedDisplayValue = string.Empty;

        public string PriceIndex
        {
            get
            {
                return this.priceIndex;
            }
            set
            {
                this.priceIndex = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
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

        public string PriceType
        {
            get
            {
                return this.priceType;
            }
            set
            {
                this.priceType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string DisplayType
        {
            get
            {
                return this.displayType;
            }
            set
            {
                this.displayType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string DisplayValue
        {
            get
            {
                return this.displayValue;
            }
            set
            {
                this.displayValue = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string TotalBaseFare
        {
            get
            {
                return this.totalBaseFare;
            }
            set
            {
                this.totalBaseFare = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string TotalOtherTaxes
        {
            get
            {
                return this.totalOtherTaxes;
            }
            set
            {
                this.totalOtherTaxes = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }


        public string FormattedDisplayValue
        {
            get
            {
                return this.formattedDisplayValue;
            }
            set
            {
                this.formattedDisplayValue = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
