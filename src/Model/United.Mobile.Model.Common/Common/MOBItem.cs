using System;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBItem
    {
        private string id = string.Empty;
        private string currentValue = string.Empty;
        private bool saveToPersist = false;

        public string Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string CurrentValue
        {
            get
            {
                return this.currentValue;
            }
            set
            {
                this.currentValue = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool SaveToPersist
        {
            get
            {
                return this.saveToPersist;
            }
            set
            {
                this.saveToPersist = value;
            }
        }
    }
}
