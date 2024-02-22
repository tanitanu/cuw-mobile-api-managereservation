using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class FareRules
    {
        public string Origin { get; set; } = string.Empty;

        public string Destination { get; set; } = string.Empty;

        public string FareBasisCode { get; set; } = string.Empty;

        public string ServiceClass { get; set; } = string.Empty;

        public List<FareRuleList> FareRuleTextList { get; set; }
        public FareRules()
        {
            FareRuleTextList = new List<FareRuleList>();
        }
    }
}
