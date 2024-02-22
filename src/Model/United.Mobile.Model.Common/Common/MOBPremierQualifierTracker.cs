using System;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class PremierQualifierTracker
    {
        private string premierQualifierTrackerTitle;
        private string premierQualifierTrackerCurrentValue;
        private string premierQualifierTrackerCurrentText;
        private string premierQualifierTrackerCurrentFlexValue;
        private string premierQualifierTrackerCurrentFlexTitle;
        private string premierQualifierTrackerThresholdValue;
        private string premierQualifierTrackerThresholdText;
        private string premierQualifierTrackerThresholdPrefix;
        private string premierQualifierTrackerCurrentChaseFlexText;
        private string premierQualifierTrackerCurrentChaseFlexValue;
        private bool isWaived;
        private string separator;
        public string PremierQualifierTrackerTitle
        {
            get { return this.premierQualifierTrackerTitle; }
            set { this.premierQualifierTrackerTitle = value; }
        }
        public string PremierQualifierTrackerCurrentValue
        {
            get { return this.premierQualifierTrackerCurrentValue; }
            set { this.premierQualifierTrackerCurrentValue = value; }
        }
        public string PremierQualifierTrackerCurrentChaseFlexValue
        {
            get { return this.premierQualifierTrackerCurrentChaseFlexValue; }
            set { this.premierQualifierTrackerCurrentChaseFlexValue = value; }
        }
        public string PremierQualifierTrackerCurrentChaseFlexText
        {
            get { return this.premierQualifierTrackerCurrentChaseFlexText; }
            set { this.premierQualifierTrackerCurrentChaseFlexText = value; }
        }
        public string PremierQualifierTrackerCurrentText
        {
            get { return this.premierQualifierTrackerCurrentText; }
            set { this.premierQualifierTrackerCurrentText = value; }
        }
        public string PremierQualifierTrackerCurrentFlexValue
        {
            get { return this.premierQualifierTrackerCurrentFlexValue; }
            set { this.premierQualifierTrackerCurrentFlexValue = value; }
        }
        public string PremierQualifierTrackerCurrentFlexTitle
        {
            get { return this.premierQualifierTrackerCurrentFlexTitle; }
            set { this.premierQualifierTrackerCurrentFlexTitle = value; }
        }
        public string PremierQualifierTrackerThresholdValue
        {
            get { return this.premierQualifierTrackerThresholdValue; }
            set { this.premierQualifierTrackerThresholdValue = value; }
        }
        public string PremierQualifierTrackerThresholdText
        {
            get { return this.premierQualifierTrackerThresholdText; }
            set { this.premierQualifierTrackerThresholdText = value; }
        }
        public bool IsWaived
        {
            get { return this.isWaived; }
            set { this.isWaived = value; }
        }
        public string Separator
        {
            get { return this.separator; }
            set { this.separator = value; }
        }
        public string PremierQualifierTrackerThresholdPrefix
        {
            get { return this.premierQualifierTrackerThresholdPrefix; }
            set { this.premierQualifierTrackerThresholdPrefix = value; }
        }
    }
}
