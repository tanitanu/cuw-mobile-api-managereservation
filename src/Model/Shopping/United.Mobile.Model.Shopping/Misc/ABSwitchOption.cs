using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable]
    public class ABSwitchOption
    {
        public ABSwitchOption()
            : base()
        {
        }

        public ABSwitchOption(string key, string mpValue, string defaultValue)
        {
            Key = key;
            MPValue = mpValue;
            DefaultValue  = defaultValue;
        }

        private string key;
        public string Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        private string mpValue;
        public string MPValue
        {
            get
            {
                return this.mpValue;
            }
            set
            {
                this.mpValue = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string defaultValue;
        public string DefaultValue
        {
            get
            {
                return this.defaultValue;
            }
            set
            {
                this.defaultValue = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
