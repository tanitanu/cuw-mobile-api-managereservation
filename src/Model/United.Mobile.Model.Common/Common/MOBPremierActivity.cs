using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class PremierActivity
    {
        private string premierActivityTitle;
        private string premierActivityYear;
        private string premierActivityStatus;
        private PremierQualifierTracker pqm;
        private PremierQualifierTracker pqs;
        private PremierQualifierTracker pqd;
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
        public PremierQualifierTracker PQM
        {
            get { return this.pqm; }
            set { this.pqm = value; }
        }
        public PremierQualifierTracker PQS
        {
            get { return this.pqs; }
            set { this.pqs = value; }
        }
        public PremierQualifierTracker PQD
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
