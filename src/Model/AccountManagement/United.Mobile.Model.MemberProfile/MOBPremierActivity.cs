using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBPremierActivity
    {
        private string premierActivityTitle;
        private string premierActivityYear;
        private string premierActivityStatus;
        private MOBPremierQualifierTracker pqm;
        private MOBPremierQualifierTracker pqs;
        private MOBPremierQualifierTracker pqd;
        private List<MOBKVP> keyValueList;
        public string PremierActivityTitle
        {
            get { return this.premierActivityTitle; }
            set { this.premierActivityTitle = value; }
        }
        public string PremierActivityYear
        {
            get { return this.premierActivityYear; }
            set { this.premierActivityYear = value; }
        }
        public string PremierActivityStatus
        {
            get { return this.premierActivityStatus; }
            set { this.premierActivityStatus = value; }
        }
        public MOBPremierQualifierTracker PQM
        {
            get { return this.pqm; }
            set { this.pqm = value; }
        }
        public MOBPremierQualifierTracker PQS
        {
            get { return this.pqs; }
            set { this.pqs = value; }
        }
        public MOBPremierQualifierTracker PQD
        {
            get { return this.pqd; }
            set { this.pqd = value; }
        }
        public List<MOBKVP> KeyValueList
        {
            get { return this.keyValueList; }
            set { this.keyValueList = value; }
        }
    }
}
