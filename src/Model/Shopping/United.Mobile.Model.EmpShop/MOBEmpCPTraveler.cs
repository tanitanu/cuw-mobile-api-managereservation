using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBEmpCPTraveler
    {
        private string title = string.Empty;
        private string firstName = string.Empty;
        private string middleName = string.Empty;
        private string lastName = string.Empty;
        private string suffix = string.Empty;
        private string genderCode = string.Empty;
        private string birthDate = string.Empty;
        private bool isProfileOwner;
        private bool isDeceased;
        private bool isExecutive;
        private string key = string.Empty;
        private int customerId;
        private int profileId;
        private int profileOwnerId;
        private int currentEliteLevel;
        private MOBEmpCPMileagePlus mileagePlus;
        private List<MOBEmpEmail> emailAddresses;
        private List<MOBEmpCPPhone> phones = new List<MOBEmpCPPhone>();
        private List<MOBEmpAddress> addresses = new List<MOBEmpAddress>();
        private List<MOBEmpPrefAirPreference> airPreferences = new List<MOBEmpPrefAirPreference>();
        private List<MOBEmpCreditCard> creditCards = new List<MOBEmpCreditCard>();
        private List<MOBEmpCPSecureTraveler> secureTravelers;
        private List<MOBEmpBKLoyaltyProgramProfile> airRewardPrograms;
        private string travelerTypeCode = string.Empty;
        private string travelerTypeDescription = string.Empty;
        private string travelProgramMemberId = string.Empty;
        private string knownTravelerNumber = string.Empty;
        private string redressNumber = string.Empty;

        private string ownerFirstName = string.Empty;
        private string ownerLastName = string.Empty;
        private string ownerMiddleName = string.Empty;
        private string ownerSuffix = string.Empty;
        private string ownerTitle = string.Empty;
        private int paxIndex;
        private List<MOBEmpSeat> seats;
        private MOBEmpUASubscriptions subscriptions;
        private string travelerNameIndex;
        private bool isTSAFlagON;
        private string message = string.Empty;
        private string mpNameNotMatchMessage = string.Empty;
        private bool isMPNameMisMatch = false;
        private List<MOBEmpEmail> reservationEmailAddresses;
        private List<MOBEmpCPPhone> reservationPhones = new List<MOBEmpCPPhone>();


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
                this.genderCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public bool IsProfileOwner
        {
            get
            {
                return this.isProfileOwner;
            }
            set
            {
                this.isProfileOwner = value;
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

        public bool IsExecutive
        {
            get
            {
                return this.isExecutive;
            }
            set
            {
                this.isExecutive = value;
            }
        }

        public int CurrentEliteLevel
        {
            get { return this.currentEliteLevel; }
            set { this.currentEliteLevel = value; }
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

        public int ProfileId
        {
            get
            {
                return this.profileId;
            }
            set
            {
                this.profileId = value;
            }
        }

        public int ProfileOwnerId
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

        public MOBEmpCPMileagePlus MileagePlus
        {
            get
            {
                return this.mileagePlus;
            }
            set
            {
                this.mileagePlus = value;
            }
        }

        public List<MOBEmpCPSecureTraveler> SecureTravelers
        {
            get
            {
                return this.secureTravelers;
            }
            set
            {
                this.secureTravelers = value;
            }
        }

        public List<MOBEmpBKLoyaltyProgramProfile> AirRewardPrograms
        {
            get
            {
                return this.airRewardPrograms;
            }
            set
            {
                this.airRewardPrograms = value;
            }
        }

        public List<MOBEmpCPPhone> Phones
        {
            get
            {
                return phones;
            }
            set
            {
                if (value != null)
                {
                    phones = value;
                }
            }
        }
        
        public List<MOBEmpAddress> Addresses {
            get {
                return addresses;
            }
            set {
                if (value != null) {
                    addresses = value;
                }
            }
        }

        public List<MOBEmpPrefAirPreference> AirPreferences
        {
            get {
                return airPreferences;
                }
            set {
                if(value != null) {
                    airPreferences = value;
                    }
                }
         }

        public List<MOBEmpEmail> EmailAddresses
        {
            get {
                return emailAddresses;
                }
            set {
                if(value != null) {
                    emailAddresses = value;
                }
            }
        }

        public List<MOBEmpCreditCard> CreditCards
        {
            get {
                return creditCards;
                }
            set {
                if(value != null) {
                    creditCards = value;
                }
            }
        }

        public string TravelerTypeCode
        {
            get
            {
                return this.travelerTypeCode;
            }
            set
            {
                this.travelerTypeCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string TravelerTypeDescription
        {
            get
            {
                return this.travelerTypeDescription;
            }
            set
            {
                this.travelerTypeDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string TravelProgramMemberId
        {
            get
            {
                return this.travelProgramMemberId;
            }
            set
            {
                this.travelProgramMemberId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public string OwnerFirstName
        {
            get
            {
                return this.ownerFirstName;
            }
            set
            {
                this.ownerFirstName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string OwnerLastName
        {
            get
            {
                return this.ownerLastName;
            }
            set
            {
                this.ownerLastName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string OwnerMiddleName
        {
            get
            {
                return this.ownerMiddleName;
            }
            set
            {
                this.ownerMiddleName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string OwnerSuffix
        {
            get
            {
                return this.ownerSuffix;
            }
            set
            {
                this.ownerSuffix = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string OwnerTitle
        {
            get
            {
                return this.ownerTitle;
            }
            set
            {
                this.ownerTitle = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public int PaxIndex
        {
            get
            {
                return this.paxIndex;
            }
            set
            {
                this.paxIndex = value;
            }
        }

        public List<MOBEmpSeat> Seats
        {
            get
            {
                return this.seats;
            }
            set
            {
                this.seats = value;
            }
        }

        public MOBEmpUASubscriptions Subscriptions
        {
            get
            {
                return this.subscriptions;
            }
            set
            {
                this.subscriptions = value;
            }
        }

        public string TravelerNameIndex
        {
            get
            {
                return this.travelerNameIndex;
            }
            set
            {
                this.travelerNameIndex = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public bool IsTSAFlagON
        {
            get
            {
                return this.isTSAFlagON;
            }
            set
            {
                this.isTSAFlagON = value;
            }
        }
        public string Message
        {
            get
            {
                return this.message;
            }
            set
            {
                this.message = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string MPNameNotMatchMessage
        {
            get
            {
                return this.mpNameNotMatchMessage;
            }
            set
            {
                this.mpNameNotMatchMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public bool ISMPNameMisMatch
        {
            get
            {
                return this.isMPNameMisMatch;
            }
            set
            {
                this.isMPNameMisMatch = value;
            }
        }

        public List<MOBEmpEmail> ReservationEmailAddresses
        {
            get
            {
                return reservationEmailAddresses;
            }
            set
            {
                if (value != null)
                {
                    reservationEmailAddresses = value;
                }
            }
        }

        public List<MOBEmpCPPhone> ReservationPhones
        {
            get
            {
                return reservationPhones;
            }
            set
            {
                if (value != null)
                {
                    reservationPhones = value;
                }
            }
        }
    }
}
