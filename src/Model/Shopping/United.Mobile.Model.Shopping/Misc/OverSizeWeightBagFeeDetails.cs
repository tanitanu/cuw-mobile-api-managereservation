using System;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable]
    public class OverSizeWeightBagFeeDetails
    {
        private string bagInfo = string.Empty;
        private string priceInfo = string.Empty;

        public string BagInfo
        {
            get { return this.bagInfo; }
            set { this.bagInfo = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string PriceInfo
        {
            get { return this.priceInfo; }
            set { this.priceInfo = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
    }
}
