using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBEmpSeatPrice
    {
        private string origin = string.Empty;
        private string destination = string.Empty;
        private string seatMessage = string.Empty;
        private int numberOftravelers;
        private decimal totalPrice;
        private string totalPriceDisplayValue;
        private decimal discountedTotalPrice;
        private string discountedTotalPriceDisplayValue;
        private string currencyCode = string.Empty;

        public MOBEmpSeatPrice()
        {
        }

        public string Origin
        {
            get
            {
                return this.origin;
            }
            set
            {
                this.origin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string Destination
        {
            get
            {
                return this.destination;
            }
            set
            {
                this.destination = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string SeatMessage
        {
            get
            {
                return this.seatMessage;
            }
            set
            {
                this.seatMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public int NumberOftravelers
        {
            get
            {
                return this.numberOftravelers;
            }
            set
            {
                this.numberOftravelers = value;
            }
        }

        public decimal TotalPrice
        {
            get
            {
                return this.totalPrice;
            }
            set
            {
                this.totalPrice = value;
            }
        }

        public decimal DiscountedTotalPrice
        {
            get
            {
                return this.discountedTotalPrice;
            }
            set
            {
                this.discountedTotalPrice = value;
            }
        }

        public string TotalPriceDisplayValue
        {
            get
            {
                return this.totalPriceDisplayValue;
            }
            set
            {
                this.totalPriceDisplayValue = string.IsNullOrEmpty(value) ? "" : value.Trim().ToUpper();
            }
        }

        public string DiscountedTotalPriceDisplayValue
        {
            get
            {
                return this.discountedTotalPriceDisplayValue;
            }
            set
            {
                this.discountedTotalPriceDisplayValue = string.IsNullOrEmpty(value) ? "" : value.Trim().ToUpper();
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
                this.currencyCode = string.IsNullOrEmpty(value) ? "USD" : value.Trim().ToUpper();
            }
        }
    }
}
