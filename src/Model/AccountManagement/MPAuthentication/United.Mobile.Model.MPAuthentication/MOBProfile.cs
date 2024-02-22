using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable()]
    public class MOBProfile
    {
        private string mileagePlusNumber = string.Empty;
        private string customerId;
        private int profileId;
        private MOBName ownerName;
        private List<MOBTraveler> travelers;
        private bool isOneTimeProfileUpdateSuccess;
        private bool isProfileOwnerTSAFlagON;
        private List<MOBTypeOption> disclaimerList = null;

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

        [XmlIgnore]
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

        public List<MOBTraveler> Travelers
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
