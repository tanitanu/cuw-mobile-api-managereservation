using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBBKRewardPriceSummary
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
