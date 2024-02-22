using System;

namespace United.Mobile.Model.Catalog
{
    [Serializable]
    public class MOBOptimizelyQMData
    {
        private string experiment = string.Empty;
        private string variation = string.Empty;
        private string dataFileVersion = string.Empty;

        public string Experiment
        {
            get
            {
                return this.experiment;
            }
            set
            {
                this.experiment = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Variation
        {
            get
            {
                return this.variation;
            }
            set
            {
                this.variation = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DataFileVersion
        {
            get
            {
                return this.dataFileVersion;
            }
            set
            {
                this.dataFileVersion = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
