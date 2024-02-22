using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping.Misc;

namespace United.Mobile.Model.Shopping.Booking
{
    [Serializable()]
    public class MOBBKBookingTravelerInfo
    {
        private string key = string.Empty;

        public string Key
        {
            get { return this.key; }
            set { this.key = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        private MOBName travelerName;

        public MOBName TravelerName
        {
            get { return travelerName; }
            set { travelerName = value; }
        }

        private List<SecureTraveler> ssrName;

        public List<SecureTraveler> SsrName
        {
            get { return ssrName; }
            set { ssrName = value; }
        }

        private List<Phone> phones;

        public List<Phone> Phones
        {
            get { return phones; }
            set { phones = value; }
        }

        private string paxIndex = string.Empty;

        public string PaxIndex
        {
            get { return paxIndex; }
            set { paxIndex = value; }
        }

        private bool isProfileOwner;

        public bool IsProfileOwner
        {
            get { return isProfileOwner; }
            set { isProfileOwner = value; }
        }

        private string travelerTypeCode = string.Empty;

        public string TravelerTypeCode
        {
            get { return travelerTypeCode; }
            set { travelerTypeCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        private string dateOfBirth = string.Empty;

        public string DateOfBirth
        {
            get { return dateOfBirth; }
            set { dateOfBirth = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        private string customerId;

        public string CustomerId
        {
            get { return customerId; }
            set { customerId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }


        private string idType = string.Empty;

        public string IdType
        {
            get { return idType; }
            set { idType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        private string gender = string.Empty;

        public string Gender
        {
            get { return gender; }
            set { gender = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        /*
        private string requestedSeat = string.Empty;

        public string RequestedSeat
        {
            get { return requestedSeat; }
            set { requestedSeat = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }
        */
        private string accountNumber = string.Empty;

        public string AccountNumber
        {
            get { return accountNumber; }
            set { accountNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        private string travelProgram = string.Empty;

        public string TravelProgram
        {
            get { return travelProgram; }
            set { travelProgram = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); }
        }

        private List<AirRewardProgram> airRewardProgram;

        public List<AirRewardProgram> AirRewardProgram
        {
            get { return airRewardProgram; }
            set { airRewardProgram = value; }
        }

        private int currentEliteLevel;

        public int CurrentEliteLevel
        {
            get { return this.currentEliteLevel; }
            set { this.currentEliteLevel = value; }
        }

        private MOBEliteStatus eliteStatus;

        public MOBEliteStatus EliteStatus
        {
            get { return this.eliteStatus; }
            set { this.eliteStatus = value; }
        }

        private bool isGoldEliteAsEPA;

        public bool IsGoldEliteAsEPA
        {
            get { return this.isGoldEliteAsEPA; }
            set { this.isGoldEliteAsEPA = value; }
        }

        private string epaMessageTitle = string.Empty;

        public string EPAMessageTitle
        {
            get { return this.epaMessageTitle; }
            set { this.epaMessageTitle = value; }
        }

        private string epaMessage = string.Empty;

        public string EPAMessage
        {
            get { return this.epaMessage; }
            set { this.epaMessage = value; }
        }

        private Ticket ticket;

        public Ticket Ticket
        {
            get { return this.ticket; }
            set { this.ticket = value; }
        }

        private bool isTSAFlagON;

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

        private string fqtvNameMisMatchText = string.Empty;

        public string FQTVNameMisMatchText
        {
            get { return this.fqtvNameMisMatchText; }
            set { this.fqtvNameMisMatchText = value; }
        }

        private bool isFQTVMadeEmptyForNameMisMatch;

        public bool ISFQTVMadeEmptyForNameMisMatch
        {
            get
            {
                return this.isFQTVMadeEmptyForNameMisMatch;
            }
            set
            {
                this.isFQTVMadeEmptyForNameMisMatch = value;
            }
        }

        private string nameMisMatchFQTVNumber = string.Empty;

        public string NameMisMatchFQTVNumber
        {
            get { return this.nameMisMatchFQTVNumber; }
            set { this.nameMisMatchFQTVNumber = value; }
        }

        private List<MOBPrefAirPreference> airPreferences;
        private List<PrefContact> contacts;

        public List<MOBPrefAirPreference> AirPreferences
        {
            get
            {
                return this.airPreferences;
            }
            set
            {
                this.airPreferences = value;
            }
        }


        public List<PrefContact> Contacts
        {
            get
            {
                return this.contacts;
            }
            set
            {
                this.contacts = value;
            }
        }

        private bool isEPlusSubscriber;

        public bool IsEPlusSubscriber
        {
            get { return this.isEPlusSubscriber; }
            set { this.isEPlusSubscriber = value; }
        }

        private MOBUASubscriptions ePlusSubscriptions;

        public MOBUASubscriptions EPlusSubscriptions
        {
            get { return this.ePlusSubscriptions; }
            set { this.ePlusSubscriptions = value; }
        }
    }
}
