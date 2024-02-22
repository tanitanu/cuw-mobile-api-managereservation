using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBComCurrency
    {
        private string code = string.Empty;
        private MOBComCountry country;
        private int decimalPlaces;
        private string name = string.Empty;

        public string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public MOBComCountry Country
        {
            get
            {
                return this.country;
            }
            set
            {
                this.country = value;
            }
        }

        public int DecimalPlaces
        {
            get
            {
                return this.decimalPlaces;
            }
            set
            {
                this.decimalPlaces = value;
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
    }
}
