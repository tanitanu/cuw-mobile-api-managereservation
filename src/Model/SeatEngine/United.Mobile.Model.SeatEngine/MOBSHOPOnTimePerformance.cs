using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.SeatMapEngine
{
    [Serializable()]
    public class MOBSHOPOnTimePerformance
    {
        private string effectiveDate = string.Empty;
        private string pctOnTimeCancelled = string.Empty;
        private string pctOnTimeDelayed = string.Empty;
        private string pctOnTimeMax = string.Empty;
        private string pctOnTimeMin = string.Empty;
        private string source = string.Empty;
        private List<string> onTimeNotAvailableMessage;
        private MOBSHOPOnTimeDOTMessages dotMessages;

        public string EffectiveDate
        {
            get
            {
                return this.effectiveDate;
            }
            set
            {
                this.effectiveDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string PctOnTimeCancelled
        {
            get
            {
                return this.pctOnTimeCancelled;
            }
            set
            {
                this.pctOnTimeCancelled = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string PctOnTimeDelayed
        {
            get
            {
                return this.pctOnTimeDelayed;
            }
            set
            {
                this.pctOnTimeDelayed = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string PctOnTimeMax
        {
            get
            {
                return this.pctOnTimeMax;
            }
            set
            {
                this.pctOnTimeMax = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string PctOnTimeMin
        {
            get
            {
                return this.pctOnTimeMin;
            }
            set
            {
                this.pctOnTimeMin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Source
        {
            get
            {
                return this.source;
            }
            set
            {
                this.source = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<string> OnTimeNotAvailableMessage
        {
            get
            {
                return this.onTimeNotAvailableMessage;
            }
            set
            {
                this.onTimeNotAvailableMessage = value;
            }
        }

        public MOBSHOPOnTimeDOTMessages DOTMessages
        {
            get
            {
                return this.dotMessages;
            }
            set
            {
                this.dotMessages = value;
            }
        }
    }
}
