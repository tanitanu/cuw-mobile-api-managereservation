using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class OwnerResponseModel : SubCommonData
    {

        public ServiceResponse<EmployeeSidaProfileResponse> Employee { get; set; }

        public ServiceResponse<ReadMemberInformationCustomModel> MileagePlus { get; set; }

        public ServiceResponse<ContactDataCustomModel> ContactPoints { get; set; }

        public ServiceResponse<List<PartnerCreditCard>> PartnerCards { get; set; }

        public ServiceResponse<CustomerMetricsDataCustomModel> CustomerMetrics { get; set; }

        public ServiceResponse<TravelerDataModel> Traveler { get; set; }

        public List<ResponseTime> ResponseTimes { get; set; }

        public ResponseSource Source { get; set; }
    }
    public enum ResponseSource
    {
        DB = 0,
        LPS = 1
    }
    public class ReadMemberInformationCustomModel : Base
    {



        public DateTime? EnrollDate { get; set; }


        public string EnrollSourceCode { get; set; }


        public string EnrollSourceDescription { get; set; }


        public string AccountStatusDescription { get; set; }


        public bool IsClosedPermanently { get; set; }


        public bool IsClosedTemporarily { get; set; }


        public bool? IsTestAccount { get; set; }


        public DateTime? LastActivityDate { get; set; }


        public DateTime? LastFlightDate { get; set; }


        public List<PremierQualifyingMetricsItem> PremierQualifyingMetrics { get; set; }


        public bool IsPClubMember { get; set; }


        public string FrequentFlyerCarrier { get; set; }


        public decimal CurrentYearMoneySpent { get; set; }


        public string SurvivorMileagePlusId { get; set; }


        public int NextStatusLevel { get; set; }


        public string NextStatusLevelDescription { get; set; }


        public int? MilesToNextLevel { get; set; }


        public int? BirthMonth { get; set; }


        public int? BirthDate { get; set; }


        public int? BirthYear { get; set; }


        public decimal? EliteMileageBalance { get; set; }


        public bool IsChaseSpend { get; set; }


        public int? ChaseSpendBalance { get; set; }


        public long? ChaseSpendUpdate { get; set; }


        public bool IsDeceased { get; set; }


        public int? MinimumSegments { get; set; }


        public long SurvivorCustomerId { get; set; }


        public bool IsLockedOut { get; set; }


        public string OpenClosedStatusDescription { get; set; }


        public string OpenClosedStatusCode { get; set; }
        //
        // Summary:
        //     Document Id
        public string _id { get; set; }
        //
        // Summary:
        //     Customer Id


        public long CustomerId { get; set; }
        //
        // Summary:
        //     Travel bank account number


        public string TravelBankAccountNumber { get; set; }
        //
        // Summary:
        //     Account status


        public string AccountStatus { get; set; }
        //
        // Summary:
        //     Account type


        public string AccountType { get; set; }
        //
        // Summary:
        //     Title


        public string Title { get; set; }
        //
        // Summary:
        //     Firstname


        public string FirstName { get; set; }
        //
        // Summary:
        //     Lastname


        public string LastName { get; set; }
        //
        // Summary:
        //     Middle name


        public string MiddleName { get; set; }
        //
        // Summary:
        //     Suffix


        public string Suffix { get; set; }
        //
        // Summary:
        //     Mileageplus Id


        public string MileageplusId { get; set; }
        //
        // Summary:
        //     MP tier level


        public int MPTierLevel { get; set; }
        //
        // Summary:
        //     MP tier level description


        public string MPTierLevelDescription { get; set; }
        //
        // Summary:
        //     Million miler level


        public int MillionMilerLevel { get; set; }
        //
        // Summary:
        //     Million miler level Desc


        public string MillionMilerLevelDesc { get; set; }
        //
        // Summary:
        //     Million miler companion


        public bool MillionMilerCompanion { get; set; }
        //
        // Summary:
        //     Lifetime miles


        public decimal LifetimeMiles { get; set; }
        //
        // Summary:
        //     Insert id


        public string InsertId { get; set; }
        //
        // Summary:
        //     Insert date


        public DateTime? InsertDateTime { get; set; }
        //
        // Summary:
        //     Update id


        public string UpdateId { get; set; }
        //
        // Summary:
        //     Update date


        public DateTime? UpdateDateTime { get; set; }
        //
        // Summary:
        //     Balance detail for pluspoint and travelbank cash


        public List<Balances> Balances { get; set; }
        //
        // Summary:
        //     Star alliance tier level


        public int StarAllianceTierLevel { get; set; }
        //
        // Summary:
        //     Star alliance tier level description


        public string StarAllianceTierLevelDescription { get; set; }
        //
        // Summary:
        //     Indicator for the the ID passed in the request is a victim ID or not.


        public bool IdInRequestIsAVictim { get; set; }
        //
        // Summary:
        //     CEO Indicator


        public bool CEO { get; set; }
        //
        // Summary:
        //     Gender

        public Constants.Gender Gender { get; set; }


        public DateTime? LastStatementDate { get; set; }


        public bool IsChaseCardHolder { get; set; }


        public bool IsChaseClubCardHolder { get; set; }


        public decimal SegmentsToNextLevel { get; set; }

    }
    public class MemberInformationV2CustomModel : ReadMemberInformationCustomModel
    {
        public string EnrollmentSourceCode { get; set; }
        public string EnrollmentDate { get; set; }
        public string EnrollmentSourceCodeDescription { get; set; }
        //public List<PremierQualifyingMetric> PremierQualifyingMetrics { get; set; }
        //public List<SubprogramMembership> SubprogramMemberships { get; set; }
        public string LastActivityDateTime { get; set; }
        public string LatestFlightDateTime { get; set; }
        public bool DeceasedIndicator { get; set; }
        public string EliteLevelChangeDate { get; set; }
        public string PriorUAAccount { get; set; }
        public bool IsTestAccount { get; set; }
        public bool UnderAge { get; set; }
    }

    public class TravelerDataModel
    {
        public string CustomerFullName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public Constants.Gender Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Type { get; set; }

        public string TypeDescription { get; set; }

        public string Title { get; set; }
        public string Nationality { get; set; }

        public string CountryOfResidence { get; set; }
        public string UpdateId { set; get; }
        public DateTime UpdateDateTime { set; get; }
        public decimal FailedLoginAttempt { set; get; }
    }


    public class ServiceResponse<T>
    {
        private List<ServiceError> _errors;
        public T Data { get; set; }
        public List<ServiceError> Errors
        {
            get
            {
                if (_errors != null) return _errors;
                else return new List<ServiceError>();
            }
            set
            {
                if (value != null) _errors = value;
                else _errors = new List<ServiceError>();
            }
        }
        public List<ResponseTime> ResponseTimes { get; set; }
    }

    public class ServiceError
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
    }

    public class EmployeeSidaProfileResponse
    {


        public string LocationCode { get; set; }



        public DateTime? SidaExpirationDateTime { get; set; }



        public string SidaId { get; set; }



        public string EmployeeId { get; set; }
    }

    public class ContactDataCustomModel
    {


        public List<Address> Addresses { get; set; }



        public List<Email> Emails { get; set; }



        public List<Phone> Phones { get; set; }
    }

   

    public class CustomerMetricsDataCustomModel
    {

        public decimal ServiceScore { get; set; }


        public decimal ServiceIndex { get; set; }


        public DateTime? ServiceScoreActiveDate { get; set; }


        public DateTime? ServiceScoreInactiveDate { get; set; }


        public string PTCCode { get; set; }


        public string AvailabilityProfile { get; set; }


        public DateTime? AvailabilityActiveDate { get; set; }


        public DateTime? AvailabilityInactiveDate { get; set; }


        public CustomerProfitScoreDataCustomModel CustomerProfitScoreData { get; set; }


        public BehaviorSegmentData BehaviorSegmentData { get; set; }
    }

    public class BehaviorSegmentData
    {
        public string EarnSegment { get; set; }
    }

    public class CustomerProfitScoreDataCustomModel
    {

        public long CustomerId { get; set; }


        public decimal ProfitScore { get; set; }


        public long CentileRank { get; set; }


        public long DecileRank { get; set; }


        public DateTime? EffectiveDate { get; set; }


        public DateTime? ExpirationDate { get; set; }


        public long UaFamilySegment { get; set; }
    }

}
