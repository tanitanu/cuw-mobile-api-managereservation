using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBClubMembership
    {
        private string mpNumber = string.Empty;
        private string firstName = string.Empty;
        private string middleName = string.Empty;
        private string lastName = string.Empty;
        private string membershipTypeCode = string.Empty;
        private string membershipTypeDescription = string.Empty;
        private string effectiveDate = string.Empty;
        private string expirationDate = string.Empty;
        private string companionMPNumber = string.Empty;
        private bool isPrimary;
        private byte[] barCode;
        private string barCodeString;

        public string MPNumber
        {
            get
            {
                return this.mpNumber;
            }
            set
            {
                this.mpNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
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

        public string MembershipTypeCode
        {
            get
            {
                return this.membershipTypeCode;
            }
            set
            {
                this.membershipTypeCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string MembershipTypeDescription
        {
            get
            {
                return this.membershipTypeDescription;
            }
            set
            {
                this.membershipTypeDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string EffectiveDate
        {
            get
            {
                return this.effectiveDate;
            }
            set
            {
                this.effectiveDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ExpirationDate
        {
            get
            {
                return this.expirationDate;
            }
            set
            {
                this.expirationDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string CompanionMPNumber
        {
            get
            {
                return this.companionMPNumber;
            }
            set
            {
                this.companionMPNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
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

        public byte[] BarCode
        {
            get
            {
                return this.barCode;
            }
            set
            {
                this.barCode = value;
            }
        }

        public string BarCodeString
        {
            get
            {
                return this.barCodeString;
            }
            set
            {
                this.barCodeString = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
