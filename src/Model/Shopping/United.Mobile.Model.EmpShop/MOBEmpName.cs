using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBEmpName
    {
        private string title = string.Empty;
        private string first = string.Empty;
        private string middle = string.Empty;
        private string last = string.Empty;
        private string suffix = string.Empty;

        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string First
        {
            get
            {
                return this.first;
            }
            set
            {
                this.first = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Middle
        {
            get
            {
                return this.middle;
            }
            set
            {
                this.middle = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Last
        {
            get
            {
                return this.last;
            }
            set
            {
                this.last = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Suffix
        {
            get
            {
                return this.suffix;
            }
            set
            {
                this.suffix = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
