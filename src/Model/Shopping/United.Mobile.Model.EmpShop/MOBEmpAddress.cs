using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBEmpAddress
    {
        private string key = string.Empty;
        private MOBEmpChannel channel;
        private string companyName = string.Empty;
        private string jobTitle = string.Empty;
        private string line1 = string.Empty;
        private string line2 = string.Empty;
        private string line3 = string.Empty;
        private string apartmentNumber = string.Empty;
        private string city = string.Empty;
        private MOBEmpState state;
        private MOBEmpCountry country;
        private bool isPrivate;
        private bool isDefault;
        private bool isPrimary;
        private string postalCode  = string.Empty;

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

        public MOBEmpChannel Channel
        {
            get
            {
                return this.channel;
            }
            set
            {
                this.channel = value;
            }
        }

        public string CompanyName
        {
            get
            {
                return this.companyName;
            }
            set
            {
                this.companyName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string JobTitle
        {
            get
            {
                return this.jobTitle;
            }
            set
            {
                this.jobTitle = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Line1
        {
            get
            {
                return this.line1;
            }
            set
            {
                this.line1 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Line2
        {
            get
            {
                return this.line2;
            }
            set
            {
                this.line2 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Line3
        {
            get
            {
                return this.line3;
            }
            set
            {
                this.line3 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ApartmentNumber
        {
            get
            {
                return this.apartmentNumber;
            }
            set
            {
                this.apartmentNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public MOBEmpState State
        {
            get
            {
                return this.state;
            }
            set
            {
                this.state = value;
            }
        }

        public MOBEmpCountry Country
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

        public bool IsPrivate
        {
            get
            {
                return this.isPrivate;
            }
            set
            {
                this.isPrivate = value;
            }
        }

        public bool IsDefault
        {
            get
            {
                return this.isDefault;
            }
            set
            {
                this.isDefault = value;
            }
        }

        public bool IsPrimary
        {
            get
            {
                return this.isPrimary;
            }
            set
            {
                this.isPrimary = value;
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
    }
}
