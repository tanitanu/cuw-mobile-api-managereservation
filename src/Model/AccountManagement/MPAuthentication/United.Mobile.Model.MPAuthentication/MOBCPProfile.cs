using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable()]
    public class MOBCPProfile
    {
        private string airportCode = string.Empty;
        private string airportNameLong = string.Empty;
        private string airportNameShort = string.Empty;
        private MOBCPCorporate corporateData;
        private string description = string.Empty;
        private string key = string.Empty;
        private string languageCode = string.Empty;
        private int profileId;
        private List<MOBCPProfileMember> profileMembers;
        private int profileOwnerId;
        private string profileOwnerKey = string.Empty;
        private string quickCreditCardKey = string.Empty;
        private string quickCreditCardNumber = string.Empty;
        private int quickCustomerId;
        private string quickCustomerKey = string.Empty;
        private List<MOBCPTraveler> travelers;
        private bool isProfileOwnerTSAFlagON;
        private List<MOBTypeOption> disclaimerList = null;
        private List<MOBKVP> savedTravelersMPList = null;
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

        public List<MOBCPProfileMember> ProfileMembers
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

        public List<MOBCPTraveler> Travelers
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

        public List<MOBKVP> SavedTravelersMPList
        {
            get { return savedTravelersMPList; }
            set { savedTravelersMPList = value; }
        }

        public MOBCPCorporate CorporateData
        {
            get
            {
                return corporateData;
            }
            set
            {
                corporateData = value;
            }
        }

    }
}
