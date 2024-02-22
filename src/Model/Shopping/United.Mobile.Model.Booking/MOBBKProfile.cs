using System;
using System.Collections.Generic;
using United.Mobile.Model.Shopping.Booking;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBBKProfile
    {
        private bool isActiveProfile;
        private string airportCode = string.Empty;
        private string airportNameLong = string.Empty;
        private string airportNameShort = string.Empty;
        private string description = string.Empty;
        private string key = string.Empty;
        private string languageCode = string.Empty;
        
        private List<MOBBKProfileMember>profileMembers;
        private int profileOwnerId;
        private string profileOwnerKey = string.Empty;
        private string quickCreditCardKey = string.Empty;
        private string quickCreditCardNumber = string.Empty;
        private int quickCustomerId;
        private string quickCustomerKey = string.Empty;
        private List<MOBBKProfileTraveler>travelers;

        private string mileagePlusNumber = string.Empty;
        private string customerId;
        private int profileId;
        private MOBName ownerName;
        private bool isOneTimeProfileUpdateSuccess;
        private bool isProfileOwnerTSAFlagON;
        private List<MOBTypeOption> disclaimerList = null;

        public bool IsActiveProfile
        {
            get
            {
                return this.isActiveProfile;
            }
            set
            {
                this.isActiveProfile =  value;
            }
        }

        public string AirportCode
        {
            get
            {
                return this.airportCode;
            }
            set
            {
                this.airportCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string AirportNameLong
        {
            get
            {
                return this.airportNameLong;
            }
            set
            {
                this.airportNameLong = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string AirportNameShort
        {
            get
            {
                return this.airportNameShort;
            }
            set
            {
                this.airportNameShort = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public string LanguageCode
        {
            get
            {
                return this.languageCode;
            }
            set
            {
                this.languageCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public List<MOBBKProfileMember> ProfileMembers
        {
            get
            {
                return this.profileMembers;
            }
            set
            {
                this.profileMembers = value;
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

        public string ProfileOwnerKey
        {
            get
            {
                return this.profileOwnerKey;
            }
            set
            {
                this.profileOwnerKey = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string QuickCreditCardKey
        {
            get
            {
                return this.quickCreditCardKey;
            }
            set
            {
                this.quickCreditCardKey = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string QuickCreditCardNumber
        {
            get
            {
                return this.quickCreditCardNumber;
            }
            set
            {
                this.quickCreditCardNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public int QuickCustomerId
        {
            get
            {
                return this.quickCustomerId;
            }
            set
            {
                this.quickCustomerId = value;
            }
        }

        public string QuickCustomerKey
        {
            get
            {
                return this.quickCustomerKey;
            }
            set
            {
                this.quickCustomerKey = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<MOBBKProfileTraveler> Travelers
        {
            get
            {
                return this.travelers;
            }
            set
            {
                this.travelers = value;
            }
        }

        public MOBName OwnerName
        {
            get
            {
                return this.ownerName;
            }
            set
            {
                this.ownerName = value;
            }
        }
              
        public string MileagePlusNumber
        {
            get
            {
                return this.mileagePlusNumber;
            }
            set
            {
                this.mileagePlusNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string CustomerId
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

        public bool IsOneTimeProfileUpdateSuccess
        {
            get
            {
                return this.isOneTimeProfileUpdateSuccess;
            }
            set
            {
                this.isOneTimeProfileUpdateSuccess = value;
            }
        }

        public bool IsProfileOwnerTSAFlagON
        {
            get
            {
                return this.isProfileOwnerTSAFlagON;
            }
            set
            {
                this.isProfileOwnerTSAFlagON = value;
            }
        }

        public List<MOBTypeOption> DisclaimerList
        {
            get { return disclaimerList; }
            set { disclaimerList = value; }
        }
    }
}
