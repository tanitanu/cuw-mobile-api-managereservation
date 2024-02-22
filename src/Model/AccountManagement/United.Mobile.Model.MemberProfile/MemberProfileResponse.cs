using System;
using System.Collections.Generic;

namespace United.Mobile.Model.MemberProfile
{
    public class MemberProfileResponse
    {
        public int CustomerId { get; set; }
        public string AccountStatus { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MileageplusId { get; set; }
        public int MPTierLevel { get; set; }
        public string MPTierLevelDescription { get; set; }
        public int MillionMilerLevel { get; set; }
        public bool MillionMilerCompanion { get; set; }
        public int LifetimeMiles { get; set; }
        public string InsertId { get; set; }
        public string UpdateId { get; set; }
        public List<MemberProfileBalance> Balances { get; set; }
        public int StarAllianceTierLevel { get; set; }
        public string StarAllianceTierLevelDescription { get; set; }
        public bool IdInRequestIsAVictim { get; set; }
        public bool CEO { get; set; }
        public bool TrialElite { get; set; }
        public string Gender { get; set; }
        public string OpenClosedStatusCode { get; set; }
        public string OpenClosedStatusDescription { get; set; }
        public bool IsLockedOut { get; set; }
        public decimal EliteSegmentBalance { get; set; }
        public int CurrentYearMoneySpent { get; set; }
        public DateTime EnrollDate { get; set; }
        public string EnrollSourceCode { get; set; }
        public string EnrollSourceDescription { get; set; }
        public string AccountStatusDescription { get; set; }
        public bool IsClosedPermanently { get; set; }
        public bool IsClosedTemporarily { get; set; }
        public DateTime LastActivityDate { get; set; }
        public DateTime LastFlightDate { get; set; }
        public List<PremierQualifyingMetric> PremierQualifyingMetrics { get; set; }
        public bool IsPClubMember { get; set; }
        public string FrequentFlyerCarrier { get; set; }
        public int SurvivorCustomerId { get; set; }
        public string ServiceName { get; set; }
    }

    public class MemberProfileBalance
    {
        public decimal Amount { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Currency { get; set; }
        public List<object> SubBalances { get; set; }
    }

    public class PremierQualifyingMetric
    {
        public string ProgramCurrency { get; set; }
        public decimal Balance { get; set; }
        public int QualifyingYear { get; set; }
    }
}
