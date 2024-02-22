using System;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class SHOPFareFamily
    {
        private string fareFamily = string.Empty;
        private int maxMileage = -1;
        private string maxPrice = string.Empty;
        private int minMileage = -1;
        private string minPrice = string.Empty;
        private bool minPriceInSummary = false;

        public string FareFamily { get { return this.fareFamily; } set { this.fareFamily = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public int MaxMileage { get { return this.maxMileage; } set { this.maxMileage = value; } }
        public string MaxPrice { get { return this.maxPrice; } set { this.maxPrice = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public int MinMileage { get { return this.minMileage; } set { this.minMileage = value; } }
        public string MinPrice { get { return this.minPrice; } set { this.minPrice = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public bool MinPriceInSummary { get { return this.minPriceInSummary; } set { this.minPriceInSummary = value; } }
    }
}
