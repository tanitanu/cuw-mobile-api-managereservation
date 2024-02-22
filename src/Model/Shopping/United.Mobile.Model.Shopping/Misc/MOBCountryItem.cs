using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class CountryItem
    {
        private string countryName = string.Empty;
        private string shortCountryCode = string.Empty;
        private string characteristics = string.Empty;

        public string CountryName
        {
            get
            {
                return this.countryName;
            }
            set
            {
                this.countryName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ShortCountryCode
        {
            get
            {
                return this.shortCountryCode;
            }
            set
            {
                this.shortCountryCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string Characteristics
        {
            get
            {
                return this.characteristics;
            }
            set
            {
                this.characteristics = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
    }
}
