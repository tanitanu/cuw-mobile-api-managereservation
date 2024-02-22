using System;
using System.Collections.Generic;
namespace United.Mobile.Model.MPAuthentication
{
    [Serializable]
    public class MOBSeatPrice
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
        private List<string> seatNumbers;
        private MOBItem seatPricesHeaderandTotal;
        private Int32 totalMiles;
        private string totalMilesDisplayValue;
        private Int32 discountedTotalMiles;
        private string discountedTotalMilesDisplayValue;
        private MOBPromoCode seatPromoDetails;

        public MOBPromoCode SeatPromoDetails
        {
            get { return seatPromoDetails; }
            set { seatPromoDetails = value; }
        }

        public MOBSeatPrice()
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

        public List<string> SeatNumbers
        {
            get { return seatNumbers; }
            set { seatNumbers = value; }
        }

        public MOBItem SeatPricesHeaderandTotal
        {
            get { return seatPricesHeaderandTotal; }
            set { seatPricesHeaderandTotal = value; }
        }
        public Int32 TotalMiles
        {
            get
            {
                return this.totalMiles;
            }
            set
            {
                this.totalMiles = value;
            }
        }
        public string TotalMilesDisplayValue
        {
            get
            {
                return this.totalMilesDisplayValue;
            }
            set
            {
                this.totalMilesDisplayValue = value;
            }
        }
        public Int32 DiscountedTotalMiles
        {
            get
            {
                return this.discountedTotalMiles;
            }
            set
            {
                this.discountedTotalMiles = value;
            }
        }
        public string DiscountedTotalMilesDisplayValue
        {
            get
            {
                return this.discountedTotalMilesDisplayValue;
            }
            set
            {
                this.discountedTotalMilesDisplayValue = value;
            }
        }

    }
}
