using System;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBSHOPFareFamily
    {
        private string fareFamily;
        private int maxMileage = -1;
        private string maxPrice;
        private int minMileage = -1;
        private string minPrice;
        private bool minPriceInSummary = false;

        public string FareFamily { get; set; }
        public int MaxMileage { get; set; }
        public string MaxPrice { get; set; } = string.Empty;
        public int MinMileage { get; set; }
        public string MinPrice { get; set; }
        public bool MinPriceInSummary { get; set; }
    }
}
