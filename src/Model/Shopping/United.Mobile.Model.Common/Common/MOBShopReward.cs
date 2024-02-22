using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBShopReward
    {
        public bool Available { get; set; }
        public string BBXSolutionSet { get; set; } = string.Empty;
        public List<ShopBookingInfo> BookingInfos { get; set; }
        public string Bucket { get; set; } = string.Empty;
        public int BucketCount { get; set; }
        public string BucketString { get; set; } = string.Empty;
        public string CabinType { get; set; } = string.Empty;
        public decimal ChangeFee { get; set; }
        public decimal ChangeFeeTotal { get; set; }
        public string CrossCabinMessaging { get; set; } = string.Empty;
        public string FareBasis { get; set; } = string.Empty;
        public decimal Mileage { get; set; }
        public decimal MileageCollect { get; set; }
        public decimal MileageCollectTotal { get; set; }
        public decimal MileageTotal { get; set; }
        public string RewardCode { get; set; } = string.Empty;
        public List<MOBShopReward> Rewards { get; set; }
        public string RewardType { get; set; } = string.Empty;
        public int SegmentIndex { get; set; }
        public bool Selected { get; set; }

        public int Soultion { get; set; }
        public string SolutionId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public bool SuppressMileage { get; set; }
        public List<ShopTax> Taxes { get; set; }
        public decimal TaxTotal { get; set; }
        public string TravelArea { get; set; } = string.Empty;
        public int TripIndex { get; set; }
        public MOBShopReward()
        {
            BookingInfos = new List<ShopBookingInfo>();
            Rewards = new List<MOBShopReward>();
            Taxes = new List<ShopTax>();
        }
    }
}
