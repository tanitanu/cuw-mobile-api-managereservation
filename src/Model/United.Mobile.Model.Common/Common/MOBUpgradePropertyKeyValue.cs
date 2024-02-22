using System;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBUpgradePropertyKeyValue
    {
        private MOBUpgradeProperty key;
        private string value = string.Empty;

        public MOBUpgradeProperty Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = value;
            }
        }

        public string Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
