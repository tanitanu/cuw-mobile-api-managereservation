using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBEmpSHOPPrice
    {
        //add new double property amount
        private string priceIndex = string.Empty;
        private string currencyCode = string.Empty;
        private string priceType = string.Empty;
        private string displayType = string.Empty; //total
        private string displayValue = string.Empty; //desc
        private string displayValueLine2 = string.Empty;
        private double value;
        private string totalBaseFare = string.Empty;
        private string totalOtherTaxes = string.Empty;     
        private string formattedDisplayValue = string.Empty; //$xx.xx

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
                this.displayValue = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string DisplayValueLine2
        {
            get
            {
                return this.displayValueLine2;
            }
            set
            {
                this.displayValueLine2 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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


        public double Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }
    }
}
