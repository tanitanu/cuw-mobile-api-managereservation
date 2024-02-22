using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBBKSecureTraveler
    {
        private string birthDate = string.Empty;
        private int customerId;
        private string description = string.Empty;
        private string documentType = string.Empty;
        private string title = string.Empty;
        private string firstName = string.Empty;
        private string gender = string.Empty;
        private string key = string.Empty;
        private string lastName = string.Empty;
        private string middleName = string.Empty;
        private int sequenceNumber;
        private string suffix = string.Empty;
        private string redressNumber = string.Empty;
        private string knownTravelerNumber = string.Empty;
        private string flaggedType = string.Empty;

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
            get { return redressNumber; }
            set { redressNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        public string KnownTravelerNumber
        {
            get { return knownTravelerNumber; }
            set { knownTravelerNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        public string FlaggedType
        {
            get
            {
                return this.flaggedType;
            }
            set
            {
                this.flaggedType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
