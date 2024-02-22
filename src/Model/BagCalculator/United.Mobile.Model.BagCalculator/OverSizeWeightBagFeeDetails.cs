using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.BagCalculator
{
    [Serializable]
    public class OverSizeWeightBagFeeDetails
    {
        public string BagInfo { get; set; } = string.Empty;
        public string PriceInfo { get; set; } = string.Empty;
    }
}
