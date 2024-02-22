using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBEmpCountry
    {
        private string code = string.Empty;
        private string name = string.Empty;
        private string phoneCode = string.Empty;

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

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string PhoneCode
        {
            get
            {
                return this.phoneCode;
            }
            set
            {
                this.phoneCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
