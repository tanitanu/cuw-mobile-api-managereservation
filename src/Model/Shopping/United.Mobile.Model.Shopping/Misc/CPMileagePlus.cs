using System;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class CPMileagePlus
    {

        public int AccountBalance { get; set; }

        public string ActiveStatusCode { get; set; } = string.Empty;


        public string ActiveStatusDescription { get; set; } = string.Empty;


        public int AllianceEliteLevel { get; set; }

        public string ClosedStatusCode { get; set; } = string.Empty;

        public string ClosedStatusDescription { get; set; } = string.Empty;

        public int CurrentEliteLevel { get; set; }

        public string CurrentEliteLevelDescription { get; set; } = string.Empty;


        public decimal CurrentYearMoneySpent { get; set; }
        public int EliteMileageBalance { get; set; }

        public int EliteSegmentBalance { get; set; }

        public int EliteSegmentDecimalPlaceValue { get; set; }
        public string EncryptedPin { get; set; } = string.Empty;

        public string EnrollDate { get; set; } = string.Empty;


        public string EnrollSourceCode { get; set; } = string.Empty;


        public string EnrollSourceDescription { get; set; } = string.Empty;

        public int FlexPqmBalance { get; set; }
        public int FutureEliteLevel { get; set; }

        public string FutureEliteDescription { get; set; } = string.Empty;

        public string InstantEliteExpirationDate { get; set; } = string.Empty;

        public bool IsCEO { get; set; }

        public bool IsClosedTemporarily { get; set; }
        public bool IsCurrentTrialEliteMember { get; set; }
        public bool IsFlexPqm { get; set; }
        public bool IsInfiniteElite { get; set; }
        public bool IsLifetimeCompanion { get; set; }
        public bool IsLockedOut { get; set; }

        public bool IsMergePending { get; set; }

        public bool IsUnitedClubMember { get; set; }

        public bool IsPresidentialPlus { get; set; }

        public string Key { get; set; } = string.Empty;


        public string LastActivityDate { get; set; } = string.Empty;

        public int LastExpiredMile { get; set; }
        public string LastFlightDate { get; set; } = string.Empty;


        public int LastStatementBalance { get; set; }
        public string LastStatementDate { get; set; } = string.Empty;


        public int LifetimeEliteLevel { get; set; }

        public int LifetimeEliteMileageBalance { get; set; }
        public string MileageExpirationDate { get; set; } = string.Empty;

        public int NextYearEliteLevel { get; set; }

        public string NextYearEliteLevelDescription { get; set; } = string.Empty;


        public string MileagePlusId { get; set; } = string.Empty;


        public string MileagePlusPin { get; set; } = string.Empty;

        public string PriorUnitedAccountNumber { get; set; } = string.Empty;

        public int StarAllianceEliteLevel { get; set; }
        public int MpCustomerId { get; set; }
        public string VendorCode { get; set; } = string.Empty;

        public string PremierLevelExpirationDate { get; set; } = string.Empty;


        public InstantElite InstantElite { get; set; }
        public string TravelBankAccountNumber { get; set; }
        public double TravelBankBalance { get; set; }
        public string TravelBankCurrencyCode { get; set; }
        public string TravelBankExpirationDate { get; set; }
    }

    [Serializable()]
    public class InstantElite
    {
        private string consolidatedCode = string.Empty;
        private string effectiveDate = string.Empty;
        private int eliteLevel;
        private int eliteYear;
        private string expirationDate = string.Empty;
        private string promotionCode = string.Empty;



        public int EliteLevel
        {
            get
            {
                return this.eliteLevel;
            }
            set
            {
                this.eliteLevel = value;
            }
        }

        public int EliteYear
        {
            get
            {
                return this.eliteYear;
            }
            set
            {
                this.eliteYear = value;
            }
        }

        public string ConsolidatedCode
        {
            get
            {
                return this.consolidatedCode;
            }
            set
            {
                this.consolidatedCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public string PromotionCode
        {
            get
            {
                return this.promotionCode;
            }
            set
            {
                this.promotionCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
