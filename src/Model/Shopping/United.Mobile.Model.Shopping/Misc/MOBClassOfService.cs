using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class ClassOfService
    {
        private string code = string.Empty;
        private string description = string.Empty;

        public ClassOfService()
        {
        }

        public ClassOfService(string classOfServiceCode, string desc)
        {
            code = classOfServiceCode;
            description = desc;
        }

        public string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
