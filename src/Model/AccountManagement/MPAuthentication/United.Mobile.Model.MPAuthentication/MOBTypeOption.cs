using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable]
    public class MOBTypeOption
    {
        public MOBTypeOption()
            : base()
        {
        }

        public MOBTypeOption(string key, string value)
        {
            Key = key;
            Value = value;
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

        private string value;
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
