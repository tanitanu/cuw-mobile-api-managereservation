using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBEmpPBTType
    {
        private string first;
        private string business;
        private string coach;
        private string total;
        private string displayText;

        public string First
        {
            get { return this.first; }
            set { this.first = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string Business
        {
            get { return this.business; }
            set { this.business = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string Coach
        {
            get { return this.coach; }
            set { this.coach = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string Total
        {
            get { return this.total; }
            set { this.total = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
        public string DisplayText
        {
            get { return this.displayText; }
            set { this.displayText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
    }
}
