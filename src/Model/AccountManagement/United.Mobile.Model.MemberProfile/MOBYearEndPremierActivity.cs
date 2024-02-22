using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBYearEndPremierActivity
    {
        private string yearEndPremierActivityTitle;
        private string yearEndPremierActivityYear;
        private string yearEndPremierActivityStatus;
        private string yearEndPQMTitle;
        private string yearEndPQMText;
        private string yearEndPQSTitle;
        private string yearEndPQSText;
        private string yearEndPQDTitle;
        private string yearEndPQDText;
        private string yearEnd4FlightSegmentMinimumText;
        private string yearEnd4FlightSegmentMinimumValue;

        public string YearEndPremierActivityTitle
        {
            get { return this.yearEndPremierActivityTitle; }
            set { this.yearEndPremierActivityTitle = value; }
        }
        public string YearEndPremierActivityYear
        {
            get { return this.yearEndPremierActivityYear; }
            set { this.yearEndPremierActivityYear = value; }
        }
        public string YearEndPremierActivityStatus
        {
            get { return this.yearEndPremierActivityStatus; }
            set { this.yearEndPremierActivityStatus = value; }
        }
        public string YearEndPQMTitle
        {
            get { return this.yearEndPQMTitle; }
            set { this.yearEndPQMTitle = value; }
        }
        public string YearEndPQMText
        {
            get { return this.yearEndPQMText; }
            set { this.yearEndPQMText = value; }
        }
        public string YearEndPQSTitle
        {
            get { return this.yearEndPQSTitle; }
            set { this.yearEndPQSTitle = value; }
        }
        public string YearEndPQSText
        {
            get { return this.yearEndPQSText; }
            set { this.yearEndPQSText = value; }
        }
        public string YearEndPQDTitle
        {
            get { return this.yearEndPQDTitle; }
            set { this.yearEndPQDTitle = value; }
        }
        public string YearEndPQDText
        {
            get { return this.yearEndPQDText; }
            set { this.yearEndPQDText = value; }
        }
        public string YearEnd4FlightSegmentMinimumText
        {
            get { return this.yearEnd4FlightSegmentMinimumText; }
            set { this.yearEnd4FlightSegmentMinimumText = value; }
        }
        public string YearEnd4FlightSegmentMinimumValue
        {
            get { return this.yearEnd4FlightSegmentMinimumValue; }
            set { this.yearEnd4FlightSegmentMinimumValue = value; }
        }

    }
}
