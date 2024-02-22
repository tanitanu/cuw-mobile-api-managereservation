using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.BagCalculator
{
    [Serializable]
    public class AdditionalBagDetails
    {
        public string Title { get; set; } = string.Empty;
        public List<OverSizeWeightBagFeeDetails> BagFeeDetails { get; set; }
        public string ImageUrl { get; set; }
        public AdditionalBagDetails()
        {
            BagFeeDetails = new List<OverSizeWeightBagFeeDetails>();
        }
    }
}
