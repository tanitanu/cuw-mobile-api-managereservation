using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MPActivity
    {
        private string transactionDate = string.Empty;
        private string description = string.Empty;
        private string ibeFlag = string.Empty;
        private string detail = string.Empty;
        private string fareCode = string.Empty;
        private string baseMiles = string.Empty;
        private string bonusMiles = string.Empty;
        private string totalMiles = string.Empty;
        private string eliteMiles = string.Empty;
        private string elitePoints = string.Empty;
        private string activitytype = string.Empty;
        private string flightNumber = string.Empty;
        private string origin = string.Empty;
        private string destination = string.Empty;

        private string fareClassBonusMiles = string.Empty;
        private string pqm = string.Empty;
        private string pqs = string.Empty;
        private string pqd = string.Empty;
        private string capIndicator = string.Empty;
        private string redeemableMilesCapMessage = string.Empty;

        private string baseMilesCapIndicator = string.Empty;
        private string bonusMilesCapIndicator = string.Empty;
        private string totalMilesCapIndicator = string.Empty;
        private string upgradeType = string.Empty;
        private List<MOBKVP> pluspoints;
        private string elitePremierFlights = string.Empty;
        private string elitePremierPoints = string.Empty;
        private string yearEarned = string.Empty;
        private string flexPQP = string.Empty;
        private DateTime transactionDateToSort;
        public string TransactionDate
        {
            get
            {
                return this.transactionDate;
            }
            set
            {
                this.transactionDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public string IBEFlag
        {
            get
            {
                return this.ibeFlag;
            }
            set
            {
                this.ibeFlag = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();

            }
        }

        public string Detail
        {
            get
            {
                return this.detail;
            }
            set
            {
                this.detail = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string FareCode
        {
            get
            {
                return this.fareCode;
            }
            set
            {
                this.fareCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string BaseMiles
        {
            get
            {
                return baseMiles;
            }
            set
            {
                this.baseMiles = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string BonusMiles
        {
            get
            {
                return bonusMiles;
            }
            set
            {
                this.bonusMiles = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string TotalMiles
        {
            get
            {
                return totalMiles;
            }
            set
            {
                this.totalMiles = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string EliteMiles
        {
            get
            {
                return eliteMiles;
            }
            set
            {
                this.eliteMiles = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ElitePoints
        {
            get
            {
                return elitePoints;
            }
            set
            {
                this.elitePoints = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ActivityType
        {
            get
            {
                return this.activitytype;
            }
            set
            {
                this.activitytype = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string FlightNumber
        {
            get
            {
                return this.flightNumber;
            }
            set
            {
                this.flightNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Origin
        {
            get
            {
                return this.origin;
            }
            set
            {
                this.origin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Destination
        {
            get
            {
                return this.destination;
            }
            set
            {
                this.destination = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string FareClassBonusMiles
        {
            get
            {
                return fareClassBonusMiles;
            }
            set
            {
                this.fareClassBonusMiles = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string PQM
        {
            get
            {
                return pqm;
            }
            set
            {
                this.pqm = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string PQS
        {
            get
            {
                return pqs;
            }
            set
            {
                this.pqs = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string PQD
        {
            get
            {
                return pqd;
            }
            set
            {
                this.pqd = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string CapIndicator
        {
            get
            {
                return this.capIndicator;
            }
            set
            {
                this.capIndicator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string RedeemableMilesCapMessage
        {
            get
            {
                return this.redeemableMilesCapMessage;
            }
            set
            {
                this.redeemableMilesCapMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string BaseMilesCapIndicator
        {
            get
            {
                return this.baseMilesCapIndicator;
            }
            set
            {
                this.baseMilesCapIndicator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string BonusMilesCapIndicator
        {
            get
            {
                return this.bonusMilesCapIndicator;
            }
            set
            {
                this.bonusMilesCapIndicator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string TotalMilesCapIndicator
        {
            get
            {
                return this.totalMilesCapIndicator;
            }
            set
            {
                this.totalMilesCapIndicator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string UpgradeType
        {
            get
            {
                return this.upgradeType;
            }
            set
            {
                this.upgradeType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public List<MOBKVP> Pluspoints
        {
            get
            {
                return this.pluspoints;
            }
            set
            {
                pluspoints = value;
            }
        }
        public string ElitePremierFlights
        {
            get
            {
                return this.elitePremierFlights;
            }
            set
            {
                this.elitePremierFlights = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string ElitePremierPoints
        {
            get
            {
                return this.elitePremierPoints;
            }
            set
            {
                this.elitePremierPoints = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string YearEarned
        {
            get
            {
                return this.yearEarned;
            }
            set
            {
                this.yearEarned = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string FlexPQP
        {
            get
            {
                return this.flexPQP;
            }
            set
            {
                this.flexPQP = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public DateTime TransactionDateToSort
        {
            get
            {
                return this.transactionDateToSort;
            }
            set
            {
                this.transactionDateToSort = value;
            }
        }
    }
    [Serializable()]
    public class ActivityItem
    {
        private string transactionDate = string.Empty;
        private string description = string.Empty;
        private DateTime transactionDateToSort;
        private List<MOBKVP> lstKVP;
        public string TransactionDate
        {
            get
            {
                return this.transactionDate;
            }
            set
            {
                this.transactionDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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
        public List<MOBKVP> LstKVP
        {
            get { return lstKVP; }
            set { lstKVP = value; }
        }
        public DateTime TransactionDateToSort
        {
            get
            {
                return this.transactionDateToSort;
            }
            set
            {
                this.transactionDateToSort = value;
            }
        }
    }
}

