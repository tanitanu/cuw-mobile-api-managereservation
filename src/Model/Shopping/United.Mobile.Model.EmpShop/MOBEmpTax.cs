using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBEmpTax
    {
        private string type;
        private string description;
        private string amount;
        private decimal raw;

        public string Type
        {
            get { return type; }
            set { type = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string Description
        {
            get { return description; }
            set { description = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string Amount
        {
            get { return amount; }
            set { amount = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public decimal Raw
        {
            get { return raw; }
            set { raw = value; }
        }
    }
}
