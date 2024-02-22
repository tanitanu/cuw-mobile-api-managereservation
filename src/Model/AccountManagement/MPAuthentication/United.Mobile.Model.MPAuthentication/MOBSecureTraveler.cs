using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable()]
    public class MOBSecureTraveler
    {
        private string key = string.Empty;
        private MOBName name;
        private string birthDate = string.Empty;
        private string documentType = string.Empty;
        private string gender = string.Empty;
        private string redressNumber = string.Empty;
        private string knownTravelerNumber = string.Empty;
        private string sequenceNumber = string.Empty;
        private string flaggedType = string.Empty;

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

        public MOBName Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

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

        public string Gender
        {
            get { return this.gender; }
            set { this.gender = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
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

        public string SequenceNumber
        {
            get { return sequenceNumber; }
            set { sequenceNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
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
