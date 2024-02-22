using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable()]
    public class MOBVBQPremierActivity
    {
        private string vBQPremierActivityTitle;
        private string vBQPpremierActivityYear;
        private string vBQPremierActivityStatus;
        private MOBPremierQualifierTracker pQF;
        private MOBPremierQualifierTracker pQP;
        private MOBPremierQualifierTracker outrightPQP;
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
        public MOBPremierQualifierTracker PQF
        {
            get { return this.pQF; }
            set { this.pQF = value; }
        }
        public MOBPremierQualifierTracker PQP
        {
            get { return this.pQP; }
            set { this.pQP = value; }
        }
        public MOBPremierQualifierTracker OutrightPQP
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
