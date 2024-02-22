using System;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBKVP
    {
        private string key = string.Empty;
        private string value = string.Empty;

        public MOBKVP() { }

        public MOBKVP(string key, string value)
        {
            this.key = key;
            this.value = value;
        }

        public string Key
        {
            get { return key; }
            set
            {
                this.key = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Value
        {
            get { return value; }
            set
            {
                this.value = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
