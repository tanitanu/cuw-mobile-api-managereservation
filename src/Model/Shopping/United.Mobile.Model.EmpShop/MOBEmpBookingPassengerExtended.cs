using System;
using System.Collections.Generic;


namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBEmpBookingPassengerExtended
    {
        private int sortOrder;
        private int eResSortOrder; //To determine the order of passengers 
        private int travelerNumber;
        private MOBEmpClassOfService selectedCabin;
        private MOBEmpPassType selectedPassType;
        private string passClass; //From PNRS->Passengers[]->PassClass
        private string displayName;
        private string paxId;
        private string id;
        private string index;
        private bool isSecureFlightNeeded;
        private bool isTCDMessageNeeded;
        private bool primaryFriend;
        private List<MOBEmpClassOfService> classOfServices = new List<MOBEmpClassOfService>();
        private List<MOBEmpSSRInfo> empSSRInfo = new List<MOBEmpSSRInfo>();
        private List<MOBEmpPassType> empPassTypes = new List<MOBEmpPassType>();       
        private MOBEmpRelationship empRelationship;
        private MOBEmpTCDInfo empTCDInfo;
        private MOBEmpBookingLapChild lapChild;
        private MOBEmpPassengerPrice empPassengerPrice; //From PNRS->Passengers[]->PassengerPrice
        //private string passengerId; //From PNRS->Passengers[]->PassengerId


        public List<MOBEmpClassOfService> ClassOfServices
        {
            get
            {
                return this.classOfServices;
            }
            set
            {
                this.classOfServices = value;
            }
        }

        public MOBEmpTCDInfo EmpTCDInfo
        {
            get
            {
                return this.empTCDInfo;
            }
            set
            {
                this.empTCDInfo = value;
            }
        }
        public MOBEmpBookingLapChild LapChild
        {
            get
            {
                return this.lapChild;
            }
            set
            {
                this.lapChild = value;
            }
        }
        public List<MOBEmpSSRInfo> EmpSSRInfo
        {
            get
            {
                return this.empSSRInfo;
            }
            set
            {
                this.empSSRInfo = value;
            }
        }
        public MOBEmpRelationship EmpRelationship
        {
            get
            {
                return this.empRelationship;
            }
            set
            {
                this.empRelationship = value;
            }
        }
        public string DisplayName
        {
            get
            {
                return this.displayName;
            }
            set
            {
                this.displayName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool IsSecureFlightNeeded
        {
            get
            {
                return this.isSecureFlightNeeded;
            }
            set
            {
                this.isSecureFlightNeeded = value;
            }
        }

        public bool IsTCDMessageNeeded
        {
            get
            {
                return this.isTCDMessageNeeded;
            }
            set
            {
                this.isTCDMessageNeeded = value;
            }
        }

        public List<MOBEmpPassType> EmpPassTypes
        {
            get
            {
                return this.empPassTypes;
            }
            set
            {
                this.empPassTypes = value;
            }
        }

        public string PaxId
        {
            get
            {
                return this.paxId;
            }
            set
            {
                this.paxId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool PrimaryFriend
        {
            get
            {
                return this.primaryFriend;
            }
            set
            {
                this.primaryFriend = value;
            }
        }
      
        public int SortOrder
        {
            get
            {
                return this.sortOrder;
            }
            set
            {
                this.sortOrder = value;
            }
        }
        public int EResSortOrder
        {
            get
            {
                return this.eResSortOrder;
            }
            set
            {
                this.eResSortOrder = value;
            }
        }
        public string Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string Index
        {
            get
            {
                return this.index;
            }
            set
            {
                this.index = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public int TravelerNumber
        {
            get
            {
                return this.travelerNumber;
            }
            set
            {
                this.travelerNumber = value;
            }
        }
        public MOBEmpClassOfService SelectedCabin
        {
            get
            {
                return this.selectedCabin;
            }
            set
            {
                this.selectedCabin = value;
            }
        }
        public MOBEmpPassType SelectedPassType
        {
            get
            {
                return this.selectedPassType;
            }
            set
            {
                this.selectedPassType = value;
            }
        }

        public string PassClass
        {
            get
            {
                return this.passClass;
            }
            set
            {
                this.passClass = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public MOBEmpPassengerPrice EmpPassengerPrice
        {
            get
            {
                return this.empPassengerPrice;
            }
            set
            {
                this.empPassengerPrice = value;
            }
        }
        
    }
}
