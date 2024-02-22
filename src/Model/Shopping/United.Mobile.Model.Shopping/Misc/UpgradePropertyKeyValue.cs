using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable]
    public class UpgradePropertyKeyValue
    {
        private UpgradeProperty key;
        private string value = string.Empty;

        public UpgradeProperty Key
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
