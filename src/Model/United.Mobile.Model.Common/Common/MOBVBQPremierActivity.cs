using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class VBQPremierActivity
    {
        private string vBQPremierActivityTitle;
        private string vBQPpremierActivityYear;
        private string vBQPremierActivityStatus;
        private PremierQualifierTracker pQF;
        private PremierQualifierTracker pQP;
        private PremierQualifierTracker outrightPQP;
        private List<MOBKVP> keyValueList;
        public string VBQPremierActivityTitle
        {
            get { return this.vBQPremierActivityTitle; }
            set { this.vBQPremierActivityTitle = value; }
        }
        public string VBQPremierActivityYear
        {
            get { return this.vBQPpremierActivityYear; }
            set { this.vBQPpremierActivityYear = value; }
        }
        public string VBQPremierActivityStatus
        {
            get { return this.vBQPremierActivityStatus; }
            set { this.vBQPremierActivityStatus = value; }
        }
        public PremierQualifierTracker PQF
        {
            get { return this.pQF; }
            set { this.pQF = value; }
        }
        public PremierQualifierTracker PQP
        {
            get { return this.pQP; }
            set { this.pQP = value; }
        }
        public PremierQualifierTracker OutrightPQP
        {
            get { return this.outrightPQP; }
            set { this.outrightPQP = value; }
        }
        public List<MOBKVP> KeyValueList
        {
            get { return this.keyValueList; }
            set { this.keyValueList = value; }
        }
    }
}
