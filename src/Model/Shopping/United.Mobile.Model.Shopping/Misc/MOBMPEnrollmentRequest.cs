using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MPEnrollmentRequest : MOBRequest
    {
        public MPEnrollmentRequest()
            : base()
        {
        }

        private string title = string.Empty;
        private string firstName = string.Empty;
        private string middleInitial = string.Empty;
        private string lastName = string.Empty;
        private string suffixName = string.Empty;
        private string birthDate = string.Empty;
        private string addressLine1 = string.Empty;
        private string addressLine2 = string.Empty;
        private string addressLine3 = string.Empty;
        private string addressType = string.Empty;
        private string city = string.Empty;
        private string stateCodeOrProvinceName = string.Empty;
        private string postalCode = string.Empty;
        private string countryCode = string.Empty;
        private bool validateAddress;
        private string homeAirportCode = string.Empty;
        private string homePhoneNumber = string.Empty;
        private string homePhoneExtension = string.Empty;
        private string homePhoneCountryCode = string.Empty;
        private string businessPhoneNumber = string.Empty;
        private string businessPhoneExtension = string.Empty;
        private string businessPhoneCountryCode = string.Empty;
        private string eMailAddress = string.Empty;
        private string pinCode = string.Empty;
        private string pinReminder = string.Empty;
        private string securityQuestion = string.Empty;
        private string securityQuestionAnswer = string.Empty;
        private string userName = string.Empty;
        private string password = string.Empty;
        private bool onlineStatement;
        private bool newsAndOffers;
        private bool specials;
        private bool tripNotes;
        private string gender;

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

        public string MiddleInitial
        {
            get
            {
                return this.middleInitial;
            }
            set
            {
                this.middleInitial = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public string SuffixName
        {
            get
            {
                return this.suffixName;
            }
            set
            {
                this.suffixName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public string AddressLine1
        {
            get
            {
                return this.addressLine1;
            }
            set
            {
                this.addressLine1 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string AddressLine2
        {
            get
            {
                return this.addressLine2;
            }
            set
            {
                this.addressLine2 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string AddressLine3
        {
            get
            {
                return this.addressLine3;
            }
            set
            {
                this.addressLine3 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string AddressType
        {
            get
            {
                return this.addressType;
            }
            set
            {
                this.addressType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public string StateCodeOrProvinceName
        {
            get
            {
                return this.stateCodeOrProvinceName;
            }
            set
            {
                this.stateCodeOrProvinceName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public string CountryCode
        {
            get
            {
                return this.countryCode;
            }
            set
            {
                this.countryCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool ValidateAddress
        {
            get
            {
                return this.validateAddress;
            }
            set
            {
                this.validateAddress = value;
            }
        }

        public string HomeAirportCode
        {
            get
            {
                return this.homeAirportCode;
            }
            set
            {
                this.homeAirportCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string HomePhoneNumber
        {
            get
            {
                return this.homePhoneNumber;
            }
            set
            {
                this.homePhoneNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string HomePhoneExtension
        {
            get
            {
                return this.homePhoneExtension;
            }
            set
            {
                this.homePhoneExtension = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string HomePhoneCountryCode
        {
            get
            {
                return this.homePhoneCountryCode;
            }
            set
            {
                this.homePhoneCountryCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string BusinessPhoneNumber
        {
            get
            {
                return this.businessPhoneNumber;
            }
            set
            {
                this.businessPhoneNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string BusinessPhoneExtension
        {
            get
            {
                return this.businessPhoneExtension;
            }
            set
            {
                this.businessPhoneExtension = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string BusinessPhoneCountryCode
        {
            get
            {
                return this.businessPhoneCountryCode;
            }
            set
            {
                this.businessPhoneCountryCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string EMailAddress
        {
            get
            {
                return this.eMailAddress;
            }
            set
            {
                this.eMailAddress = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string PinCode
        {
            get
            {
                return this.pinCode;
            }
            set
            {
                this.pinCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string PinReminder
        {
            get
            {
                return this.pinReminder;
            }
            set
            {
                this.pinReminder = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string SecurityQuestion
        {
            get
            {
                return this.securityQuestion;
            }
            set
            {
                this.securityQuestion = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string SecurityQuestionAnswer
        {
            get
            {
                return this.securityQuestionAnswer;
            }
            set
            {
                this.securityQuestionAnswer = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string UserName
        {
            get
            {
                return this.userName;
            }
            set
            {
                this.userName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool OnlineStatement
        {
            get
            {
                return this.onlineStatement;
            }
            set
            {
                this.onlineStatement = value;
            }
        }

        public bool NewsAndOffers
        {
            get
            {
                return this.newsAndOffers;
            }
            set
            {
                this.newsAndOffers = value;
            }
        }

        public bool Specials
        {
            get
            {
                return this.specials;
            }
            set
            {
                this.specials = value;
            }
        }

        public bool TripNotes
        {
            get
            {
                return this.tripNotes;
            }
            set
            {
                this.tripNotes = value;
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
    }
}
