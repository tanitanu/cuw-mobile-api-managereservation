using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBPerPerson
    {
        private string childIndicator = string.Empty;
        private MOBComCountry countryOfResidence;
        private string customerId = string.Empty;
        private string dateOfBirth = string.Empty;
        private string givenName = string.Empty;
        private string infantIndicator = string.Empty;
        private string key = string.Empty;
        private string middleName = string.Empty;
        private MOBComCountry nationality;
        private string preferredName = string.Empty;
        private string sex = string.Empty;
        private string suffix = string.Empty;
        private string surname = string.Empty;
        private string title = string.Empty;
        private List<Model.Shopping.MOBPerDocument> documents;

        public string ChildIndicator
        {
            get
            {
                return this.childIndicator;
            }
            set
            {
                this.childIndicator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public MOBComCountry CountryOfResidence
        {
            get
            {
                return this.countryOfResidence;
            }
            set
            {
                this.countryOfResidence = value;
            }
        }

        public string CustomerId
        {
            get
            {
                return this.customerId;
            }
            set
            {
                this.customerId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DateOfBirth
        {
            get
            {
                return this.dateOfBirth;
            }
            set
            {
                this.dateOfBirth = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string GivenName
        {
            get
            {
                return this.givenName;
            }
            set
            {
                this.givenName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string InfantIndicator
        {
            get
            {
                return this.infantIndicator;
            }
            set
            {
                this.infantIndicator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public string MiddleName
        {
            get
            {
                return this.middleName;
            }
            set
            {
                this.middleName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public MOBComCountry Nationality
        {
            get
            {
                return this.nationality;
            }
            set
            {
                this.nationality = value;
            }
        }

        public string PreferredName
        {
            get
            {
                return this.preferredName;
            }
            set
            {
                this.preferredName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Sex
        {
            get
            {
                return this.sex;
            }
            set
            {
                this.sex = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public string Surname
        {
            get
            {
                return this.surname;
            }
            set
            {
                this.surname = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

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

        public List<MOBPerDocument> Documents
        {
            get
            {
                return this.documents;
            }
            set
            {
                this.documents = value;
            }
        }
    }
}
