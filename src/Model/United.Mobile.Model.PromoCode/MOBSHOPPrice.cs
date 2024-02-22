using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.PromoCode
{
    [Serializable()]
    public class MOBSHOPPrice 
    {
        //add new double property amount
        private string priceIndex = string.Empty;
        private string currencyCode = string.Empty;
        private string priceType = string.Empty;
        private string displayType = string.Empty;
        private string displayValue = string.Empty;
        private double value;
        private string totalBaseFare = string.Empty;
        private string totalOtherTaxes = string.Empty;
        private string formattedDisplayValue = string.Empty;
        private string status = string.Empty;
        private bool waived;
        private string billedSeperateText;
        private string paxTypeCode;

        public string PaxTypeCode
        {
            get { return paxTypeCode; }
            set { paxTypeCode = value; }
        }

        public bool Waived
        {
            get { return waived; }
            set { waived = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }


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

        private string priceTypeDescription;

        public string PriceTypeDescription
        {
            get { return priceTypeDescription; }
            set { priceTypeDescription = value; }
        }
        public string BilledSeperateText
        {
            get { return billedSeperateText; }
            set { billedSeperateText = value; }
        }

        private MOBPromoCode promoDetails;

        public MOBPromoCode PromoDetails
        {
            get { return promoDetails; }
            set { promoDetails = value; }
        }

        private string paxTypeDescription;
        public string PaxTypeDescription
        {
            get { return paxTypeDescription; }
            set { paxTypeDescription = value; }
        }

    }
}

