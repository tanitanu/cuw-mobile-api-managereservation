using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.Shopping;

namespace United.Mobile.Model.PromoCode
{
    [Serializable]
    public class MOBSHOPTravelOptionSubItem
    {
        private double amount;
        private string displayAmount = string.Empty;
        private string currencyCode = string.Empty;
        private string description = string.Empty;
        private string key = string.Empty;
        private string productId = string.Empty;
        private object value = string.Empty;

        public double Amount
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

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ProductId
        {
            get
            {
                return this.productId;
            }
            set
            {
                this.productId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public object Value
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
