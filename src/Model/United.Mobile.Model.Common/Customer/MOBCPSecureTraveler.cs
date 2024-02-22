using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBCPSecureTraveler
    {
        private string birthDate = string.Empty;
        private int customerId;
        private string description = string.Empty;
        private string decumentType = string.Empty;
        private string firstName = string.Empty;
        private string gender = string.Empty;
        private string key = string.Empty;
        private string lastName = string.Empty;
        private string middleName = string.Empty;
        private string suffix = string.Empty;
        private string knownTravelerNumber = string.Empty;
        private string redressNumber = string.Empty;
        private int sequenceNumber;

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

        public int CustomerId
        {
            get
            {
                return this.customerId;
            }
            set
            {
                this.customerId = value;
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

        public string DecumentType
        {
            get
            {
                return this.decumentType;
            }
            set
            {
                this.decumentType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string FirstName
        {
            get
            {
                return this.firstName;
            }
            set
            {
                this.firstName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

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

        public int SequenceNumber
        {
            get
            {
                return this.sequenceNumber;
            }
            set
            {
                this.sequenceNumber = value;
            }
        }
    }
}
