using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable]
    public class AdditionalBagDetails
    {
        private string title = string.Empty;
        private List<OverSizeWeightBagFeeDetails> bagFeeDetails;

        public string Title
        {
            get { return this.title; }
            set { this.title = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public List<OverSizeWeightBagFeeDetails> BagFeeDetails
        {
            get { return this.bagFeeDetails; }
            set { this.bagFeeDetails = value; }
        }
    }
}
