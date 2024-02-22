using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class SHOPReward
    {
        public string TripId { get; set; } = string.Empty;

        public string FlightId { get; set; } = string.Empty;

        public string RewardId { get; set; } = string.Empty;

        public bool Available { get; set; }

        public string Cabin { get; set; } = string.Empty;

        public decimal ChangeFee { get; set; }

        public decimal ChangeFeeTotal { get; set; }

        public string CrossCabinMessaging { get; set; } = string.Empty;

        public string FareBasis { get; set; } = string.Empty;

        public decimal Mileage { get; set; }

        public decimal MileageCollect { get; set; }

        public decimal MileageCollectTotal { get; set; }

        public decimal MileageTotal { get; set; }

        public string RewardCode { get; set; } = string.Empty;

        public string RewardType { get; set; } = string.Empty;

        public bool Selected { get; set; }

        public string Status { get; set; } = string.Empty;

        public List<MOBSHOPTax> Taxes { get; set; }

        public decimal TaxTotal { get; set; }

        public decimal TaxAndFeeTotal { get; set; }

        public List<string> Descriptions { get; set; }

        public SHOPReward()
        {
            Taxes = new List<MOBSHOPTax>();
            Descriptions = new List<string>();
        }
    }
}
