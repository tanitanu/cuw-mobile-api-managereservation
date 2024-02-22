using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class TMAAirport
    {
        private string iataCityCode = string.Empty;
        private string iataCode = string.Empty;
        private string iataCountryCode = string.Empty;
        private string name = string.Empty;
        private string shortName = string.Empty;

        public string IATACityCode
        {
            get
            {
                return this.iataCityCode;
            }
            set
            {
                this.iataCityCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string IATACode
        {
            get
            {
                return this.iataCode;
            }
            set
            {
                this.iataCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string IATACountryCode
        {
            get
            {
                return this.iataCountryCode;
            }
            set
            {
                this.iataCountryCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
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
