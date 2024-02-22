using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class SecureTraveler
    {
        private string birthDate = string.Empty;
        private string description = string.Empty;
        private string gender = string.Empty;
        private string key = string.Empty;
        private string lastName = string.Empty;
        private string middleName = string.Empty;
        private string suffix = string.Empty;
        private string knownTravelerNumber = string.Empty;
        private string redressNumber = string.Empty;
        private string documentType = string.Empty;

        public string BirthDate
        {
            get
            {
                return this.birthDate;
            }
            set
            {
                this.birthDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public int CustomerId { get; set; }

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

        public string DocumentType
        {
            get
            {
                return this.documentType;
            }
            set
            {
                this.documentType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Title { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string Gender
        {
            get
            {
                return this.gender;
            }
            set
            {
                this.gender = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public string LastName
        {
            get
            {
                return this.lastName;
            }
            set
            {
                this.lastName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public int SequenceNumber { get; set; }

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

        public string RedressNumber
        {
            get
            {
                return this.redressNumber;
            }
            set
            {
                this.redressNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string KnownTravelerNumber
        {
            get
            {
                return this.knownTravelerNumber;
            }
            set
            {
                this.knownTravelerNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string FlaggedType { get; set; } = string.Empty;

    }
}
