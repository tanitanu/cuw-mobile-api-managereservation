using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class SHOPOnTimePerformance
    {

        public string EffectiveDate { get; set; } = string.Empty;

        public string PctOnTimeCancelled { get; set; } = string.Empty;

        public string PctOnTimeDelayed { get; set; } = string.Empty;

        public string PctOnTimeMax { get; set; } = string.Empty;

        public string PctOnTimeMin { get; set; } = string.Empty;

        public string Source { get; set; } = string.Empty;

        public List<string> OnTimeNotAvailableMessage { get; set; }

        public SHOPOnTimeDOTMessages DOTMessages { get; set; }
        public SHOPOnTimePerformance()
        {
            OnTimeNotAvailableMessage = new List<string>();
        }
    }
}
