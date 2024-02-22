using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class SHOPRewardPriceSummary
    {
        public string Cabin { get; set; } = string.Empty;

        public List<string> PriceSummaries { get; set; }

        public SHOPRewardPriceSummary()
        {
            PriceSummaries = new List<string>();
        }
    }
}
