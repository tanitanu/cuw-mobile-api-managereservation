using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.SSR;

namespace United.Mobile.Model.Travelers
{
    [Serializable()]
    public class CPTraveler
    {
        private string title ;
        private string firstName ;
        private string middleName;
        private string lastName;
        private string suffix ;
        private string genderCode ;
        private string birthDate = string.Empty;
        private bool isProfileOwner;
        private bool isDeceased;
        private bool isExecutive;
        private string key;
        private int customerId;
        private MOBCPCustomerMetrics customerMetrics;
        private int profileId;
        private int profileOwnerId;
        private int currentEliteLevel;
        private MOBCPMileagePlus mileagePlus;
        private List<MOBEmail> emailAddresses;
        private List<MOBCPPhone> phones;
        private List<MOBAddress> addresses;
        private List<MOBPrefAirPreference> airPreferences;
        private List<MOBCreditCard> creditCards;
        private List<MOBCPSecureTraveler> secureTravelers;
        private List<MOBBKLoyaltyProgramProfile> airRewardPrograms;
        private string travelerTypeCode = string.Empty;
        private string travelerTypeDescription ;
        private string travelProgramMemberId;
        private string knownTravelerNumber;
        private string redressNumber;

        private string ownerFirstName;
        private string ownerLastName;
        private string ownerMiddleName;
        private string ownerSuffix;
        private string ownerTitle;
        private int paxIndex;
        private List<MOBSeat> seats;
        private MOBUASubscriptions subscriptions;
        private string TravelerNameIndex;
        private bool isTSAFlagON;
        private string message;
        private string mpNameNotMatchMessage;
        private bool IsMPNameMisMatch = false;
        private List<MOBEmail> reservationEmailAddresses;
        private List<MOBCPPhone> reservationPhones;
        private MOBMPSecurityUpdate mpSecurityUpdate;
        private MOBCPCubaSSR cubaTravelReason;
        private string employeeId;
        private int paxID;
        private bool isPaxSelected;

        private string nationality;
        private string countryOfResidence;
        private List<TravelSpecialNeed> selectedSpecialNeeds;
        private List<MOBItem> selectedSpecialNeedMessages;
        private bool isEligibleForSeatSelection = true;

        private string ptcDescription; //Passenger type code description
        private double individualTotalAmount;
        private string cslReservationPaxTypeCode;
        private string PNRCustomerID;
        private string totalFFCNewValueAfterRedeem;

        public string TotalFFCNewValueAfterRedeem
        {
            get { return totalFFCNewValueAfterRedeem; }
            set { totalFFCNewValueAfterRedeem = value; }
        }

        public string pnrCustomerID
        {
            get { return this.PNRCustomerID; }
            set { this.PNRCustomerID = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private List<MOBFOPFutureFlightCredit> futureFlightCredits;
        public List<MOBFOPFutureFlightCredit> FutureFlightCredits
        {
            get { return futureFlightCredits; }
            set { futureFlightCredits = value; }
        }

        public string CslReservationPaxTypeCode
        {
            get { return cslReservationPaxTypeCode; }
            set { cslReservationPaxTypeCode = value; }
        }


        public double IndividualTotalAmount
        {
            get { return individualTotalAmount; }
            set { individualTotalAmount = value; }
        }

        public string PTCDescription
        {
            get { return this.ptcDescription; }
            set { ptcDescription = value; }
        }
        public bool IsEligibleForSeatSelection
        {
            get { return this.isEligibleForSeatSelection; }
            set { isEligibleForSeatSelection = value; }
        }

        public string Nationality
        {
            get { return nationality; }
            set { nationality = value; }
        }

        public string CountryOfResidence
        {
            get { return countryOfResidence; }
            set { countryOfResidence = value; }
        }

        public string _employeeId
        {
            get;
            set;
        }

        public MOBCPCubaSSR CubaTravelReason
        {
            get { return cubaTravelReason; }
            set { cubaTravelReason = value; }
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

        public MOBCPMileagePlus MileagePlus
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

        public List<MOBCPSecureTraveler> SecureTravelers
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

        public List<MOBBKLoyaltyProgramProfile> AirRewardPrograms
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

        public List<MOBCPPhone> Phones
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

        public List<MOBAddress> Addresses
        {
            get
            {
                return addresses;
            }
            set
            {
                if (value != null)
                {
                    addresses = value;
                }
            }
        }

        public List<MOBPrefAirPreference> AirPreferences
        {
            get
            {
                return airPreferences;
            }
            set
            {
                if (value != null)
                {
                    airPreferences = value;
                }
            }
        }

        public List<MOBEmail> EmailAddresses
        {
            get
            {
                return emailAddresses;
            }
            set
            {
                if (value != null)
                {
                    emailAddresses = value;
                }
            }
        }

        public List<MOBCreditCard> CreditCards
        {
            get
            {
                return creditCards;
            }
            set
            {
                if (value != null)
                {
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
            get;
            set;
        }

        public string RedressNumber
        {
            get;
            set;
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

        public List<MOBSeat> Seats
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

        public MOBUASubscriptions Subscriptions
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

        public string travelerNameIndex
        {
            get
            {
                return this.TravelerNameIndex;
            }
            set
            {
                this.TravelerNameIndex = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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
        public bool isMPNameMisMatch
        {
            get
            {
                return this.IsMPNameMisMatch;
            }
            set
            {
                this.IsMPNameMisMatch = value;
            }
        }

        public List<MOBEmail> ReservationEmailAddresses
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

        public List<MOBCPPhone> ReservationPhones
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
        public MOBMPSecurityUpdate MPSecurityUpdate
        {
            get
            {
                return this.mpSecurityUpdate;
            }
            set
            {
                this.mpSecurityUpdate = value;
            }
        }

        public MOBCPCustomerMetrics CustomerMetrics
        {
            get
            {
                return customerMetrics;
            }
            set
            {
                customerMetrics = value;
            }
        }

        public int PaxID
        {
            get
            {
                return this.paxID;
            }
            set
            {
                this.paxID = value;
            }
        }

        public bool IsPaxSelected
        {
            get
            {
                return this.isPaxSelected;
            }
            set
            {
                this.isPaxSelected = value;
            }
        }

        [XmlArrayItem("MOBTravelSpecialNeed")]
        public List<TravelSpecialNeed> SelectedSpecialNeeds
        {
            get { return selectedSpecialNeeds; }
            set { selectedSpecialNeeds = value; }
        }

        public List<MOBItem> SelectedSpecialNeedMessages
        {
            get { return selectedSpecialNeedMessages; }
            set { selectedSpecialNeedMessages = value; }
        }
        private bool isMustRideTraveler;

        public bool IsMustRideTraveler
        {
            get { return isMustRideTraveler; }
            set { isMustRideTraveler = value; }
        }

    }
}
