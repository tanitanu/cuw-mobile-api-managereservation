using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.SeatMap
{
    [Serializable()]
    public class MOBBKPerson
    {
        private string key = string.Empty;
        private MOBBKCountry countryOfResidence;
        private string dateOfBirth = string.Empty;
        private string givenName = string.Empty;
        private string middleName = string.Empty;
        private MOBBKCountry nationality;
        private string preferredName = string.Empty;
        private string sex = string.Empty;
        private string suffix = string.Empty;
        private string surname = string.Empty;
        private string title = string.Empty;
        private List<MOBBKDocument> documents;
        private MOBAddress address;
        private List<MOBPhone> phones;
        private string redressNumber = string.Empty;
        private string knownTravelerNumber = string.Empty;
        private string email = string.Empty;


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

        public MOBBKCountry CountryOfResidence
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

        public MOBBKCountry Nationality
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

        public List<MOBBKDocument> Documents
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

        public MOBAddress Address
        {
            get
            {
                return this.address;
            }
            set
            {
                this.address = value;
            }
        }

        public List<MOBPhone> Phones
        {
            get
            {
                return this.phones;
            }
            set
            {
                this.phones = value;
            }
        }
        public string RedressNumber
        {
            get { return redressNumber; }
            set { redressNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        public string KnownTravelerNumber
        {
            get { return knownTravelerNumber; }
            set { knownTravelerNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        public string Email
        {
            get
            {
                return this.email;
            }
            set
            {
                this.email = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
