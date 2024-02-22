using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class Country
    {
        private string countryCode = string.Empty;
        private string name = string.Empty;
        private string shortName = string.Empty;

        public string CountryCode
        {
            get
            {
                return this.countryCode;
            }
            set
            {
                this.countryCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
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

        public string ShortName
        {
            get
            {
                return this.shortName;
            }
            set
            {
                this.shortName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
