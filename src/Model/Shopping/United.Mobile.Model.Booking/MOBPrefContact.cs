using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBPrefContact
    {
        private long customerId;
        private long profileOwnerId;
        private long contactId;
        private int contactSequenceNum;
        private string key = string.Empty;
        private string contactTypeCode = string.Empty;
        private string contactTypeDescription = string.Empty;
        private string title = string.Empty;
        private string firstName = string.Empty;
        private string middleName = string.Empty;
        private string lastName = string.Empty;
        private string suffix = string.Empty;
        private string genderCode = string.Empty;
        private string contactMileagePlusId = string.Empty;
        private string languageCode = string.Empty;
        private bool isSelected;
        private bool isNew;
        private bool isVictim;
        private bool isDeceased;
        private List<MOBPrefPhone> phones;

        public long CustomerId
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

        public long ProfileOwnerId
        {
            get
            {
                return this.profileOwnerId;
            }
            set
            {
                this.profileOwnerId = value;
            }
        }

        public long ContactId
        {
            get
            {
                return this.contactId;
            }
            set
            {
                this.contactId = value;
            }
        }

        public int ContactSequenceNum
        {
            get
            {
                return this.contactSequenceNum;
            }
            set
            {
                this.contactSequenceNum = value;
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
                this.key = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string ContactTypeCode
        {
            get
            {
                return this.contactTypeCode;
            }
            set
            {
                this.contactTypeCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string ContactTypeDescription
        {
            get
            {
                return this.contactTypeDescription;
            }
            set
            {
                this.contactTypeDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public string GenderCode
        {
            get
            {
                return this.genderCode;
            }
            set
            {
                this.genderCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string ContactMileagePlusId
        {
            get
            {
                return this.contactMileagePlusId;
            }
            set
            {
                this.contactMileagePlusId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string LanguageCode
        {
            get
            {
                return this.languageCode;
            }
            set
            {
                this.languageCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                this.isSelected = value;
            }
        }

        public bool IsNew
        {
            get
            {
                return this.isNew;
            }
            set
            {
                this.isNew = value;
            }
        }

        public bool IsVictim
        {
            get
            {
                return this.isVictim;
            }
            set
            {
                this.isVictim = value;
            }
        }

        public bool IsDeceased
        {
            get
            {
                return this.isDeceased;
            }
            set
            {
                this.isDeceased = value;
            }
        }

        public List<MOBPrefPhone> Phones
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
    }
}
