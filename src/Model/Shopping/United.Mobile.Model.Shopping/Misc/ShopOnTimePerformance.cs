using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class ShopOnTimePerformance
    {
        public string EffectiveDate { get; set; } = string.Empty;
        public string PctOnTimeCancelled { get; set; } = string.Empty;
        public string PctOnTimeDelayed { get; set; } = string.Empty;
        public string PctOnTimeMax { get; set; } = string.Empty;
        public string PctOnTimeMin { get; set; } = string.Empty;
    }
}
