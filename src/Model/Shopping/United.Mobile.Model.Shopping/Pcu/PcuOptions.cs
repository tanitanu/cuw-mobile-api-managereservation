using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace United.Mobile.Model.Shopping.Pcu
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

        [XmlArrayItem("MOBPcuSegment")]
        public List<PcuSegment> EligibleSegments
        {
            get { return eligibleSegments; }
            set { eligibleSegments = value; }
        }

        [XmlArrayItem("MOBPcuUpgradeOptionInfo")]
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
