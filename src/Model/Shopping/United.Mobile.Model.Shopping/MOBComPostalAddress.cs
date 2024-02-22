using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBComPostalAddress
    {
        private List<string> addressLines;
        private string city = string.Empty;
        private MOBComCountry country;
        private string defaultIndicator = string.Empty;
        private string description = string.Empty;
        private int displaySequence;
        private string key = string.Empty;
        private string name = string.Empty;
        private string postalCode = string.Empty;
        private string rigion = string.Empty;
        private string stateProvince = string.Empty;

        public List<string> AddressLines
        {
            get
            {
                return this.addressLines;
            }
            set
            {
                this.addressLines = value;
            }
        }

        public string City
        {
            get
            {
                return this.city;
            }
            set
            {
                this.city = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public string DefaultIndicator
        {
            get
            {
                return this.defaultIndicator;
            }
            set
            {
                this.defaultIndicator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public int DisplaySequence
        {
            get
            {
                return this.displaySequence;
            }
            set
            {
                this.displaySequence = value;
            }
        }

        public string Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public string PostalCode
        {
            get
            {
                return this.postalCode;
            }
            set
            {
                this.postalCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Rigion
        {
            get
            {
                return this.rigion;
            }
            set
            {
                this.rigion = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string StateProvince
        {
            get
            {
                return this.stateProvince;
            }
            set
            {
                this.stateProvince = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
