using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBTier
    {
        private int id;
        private List<MOBPricing> pricing;

        public MOBTier()
        {
            Pricing = new List<MOBPricing>();
        }

        public MOBTier(int id, decimal basePrice, string currencyCode, decimal numberOfDecimals, List<MOBPricing> pricing)
        {
            Id = id;
            Pricing = pricing;
        }

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public List<MOBPricing> Pricing
        {
            get { return pricing; }
            set { pricing = value; }
        }
    }

    [Serializable()]
    public class MOBPricing
    {
        private int travelerId;
        private decimal totalPrice;
        private string originalPrice;
        private string couponCode;
        private string eligibility;
        private string currencyCode;

        public MOBPricing()
        {

        }

        public MOBPricing(int travelerId, decimal totalPrice, string originalPrice, string couponCode, string eligibility, string currencyCode)
        {
            TravelerId = travelerId;
            TotalPrice = totalPrice;
            OriginalPrice = originalPrice;
            CouponCode = couponCode;
            Eligibility = eligibility;
            CurrencyCode = currencyCode;
        }

        public int TravelerId
        {
            get { return travelerId; }
            set { travelerId = value; }
        }

        public decimal TotalPrice
        {
            get { return totalPrice; }
            set { totalPrice = value; }
        }

        public string OriginalPrice
        {
            get { return originalPrice; }
            set { originalPrice = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        public string CouponCode
        {
            get { return couponCode; }
            set { couponCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        public string Eligibility
        {
            get { return eligibility; }
            set { eligibility = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        public string CurrencyCode
        {
            get { return currencyCode; }
            set { currencyCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }
    }
}
