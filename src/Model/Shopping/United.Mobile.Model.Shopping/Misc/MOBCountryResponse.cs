using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class CountryResponse : MOBResponse
    {
        public CountryResponse()
            : base()
        {
        }

        private List<CountryItem> countries;
        private string version = string.Empty;

        public List<CountryItem> Countries
        {
            get { return this.countries; }
            set
            {
                this.countries = value;
            }
        }

        public string Version
        {
            get
            {
                return this.version;
            }
            set
            {
                this.version = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
    }
}
