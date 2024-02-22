using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.SeatMapEngine
{
    [Serializable]
    public class MOBSHOPRewardPriceSummary
    {
        private string cabin;
        private List<string> priceSummaries;

        public string Cabin
        {
            get
            {
                return this.cabin;
            }
            set
            {
                this.cabin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<string> PriceSummaries
        {
            get
            {
                return this.priceSummaries;
            }
            set
            {
                this.priceSummaries = value;
            }
        }
    }
}
