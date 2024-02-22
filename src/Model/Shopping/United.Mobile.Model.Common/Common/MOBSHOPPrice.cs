using System;
using System.Globalization;
using United.Mobile.Model.Common;
using United.Utility.Helper;

namespace United.Mobile.Model.Shopping
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
        // private string status = string.Empty;
        private bool waived;
        private string billedSeperateText;
        private string productId = string.Empty;
        private string paxTypeCode;
        private string strikeThroughDescription = string.Empty;
        private string strikeThroughDisplayValue = string.Empty;

        public string StrikeThroughDescription
        {
            get { return strikeThroughDescription; }
            set { strikeThroughDescription = value; }
        }

        public string StrikeThroughDisplayValue
        {
            get { return strikeThroughDisplayValue; }
            set { strikeThroughDisplayValue = value; }
        }

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

        public string Status { get; set; } = string.Empty;
        //{
        //    get { return status; }
        //    set { status = value; }
        //}


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

        public string ProductId
        {
            get { return productId; }
            set { this.productId = string.IsNullOrEmpty(value) ? string.Empty : value; }
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
