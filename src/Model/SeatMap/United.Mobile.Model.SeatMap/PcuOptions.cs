using System;
using System.Collections.Generic;

namespace United.Mobile.Model.SeatMap
{
    [Serializable()]
    public class PcuOptions
    {
        private List<string> eligibleTravelers;
        private List<PcuSegment> eligibleSegments;
        private List<PcuUpgradeOptionInfo> compareOptions;
        private string currencyCode;

        public List<string> EligibleTravelers
        {
            get { return eligibleTravelers; }
            set { eligibleTravelers = value; }
        }

        public List<PcuSegment> EligibleSegments
        {
            get { return eligibleSegments; }
            set { eligibleSegments = value; }
        }

        public List<PcuUpgradeOptionInfo> CompareOptions
        {
            get { return compareOptions; }
            set { compareOptions = value; }
        }

        public string CurrencyCode
        {
            get { return currencyCode; }
            set { currencyCode = value; }
        }
    }
}
