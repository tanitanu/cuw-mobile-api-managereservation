
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class ReadMemberInformation : Base
    {


        public string EnrollSourceCode { get; set; }

        public string EnrollSourceDescription { get; set; }

        public string AccountStatusDescription { get; set; }

        public bool? IsClosedPermanently { get; set; }

        public bool? IsClosedTemporarily { get; set; }

        public bool? IsTestAccount { get; set; }

        public DateTime LastActivityDate { get; set; }

        public DateTime LastFlightDate { get; set; }

        public List<PremierQualifyingMetricsItem> PremierQualifyingMetrics { get; set; }

        public bool? IsPClubMember { get; set; }

        public string FrequentFlyerCarrier { get; set; }

        public string SurvivorMileagePlusId { get; set; }

        public long SurvivorCustomerId { get; set; }

        public int? NextStatusLevel { get; set; }

        public string NextStatusLevelDescription { get; set; }

        public int? MilesToNextLevel { get; set; }

        public int BirthMonth { get; set; }

        public int BirthDate { get; set; }

        public int BirthYear { get; set; }

        public decimal? EliteMileageBalance { get; set; }

        public bool IsChaseSpend { get; set; }

        public int? ChaseSpendBalance { get; set; }

        public long? ChaseSpendUpdate { get; set; }

        public bool IsDeceased { get; set; }


        public int? MinimumSegments { get; set; }


        public DateTime? LastStatementDate { get; set; }


        public List<PartnerCreditCard> PartnerCards { get; set; }


        public DateTime EnrollDate { get; set; }


        public decimal? CurrentYearMoneySpent { get; set; }


        public bool? IsLockedOut { get; set; }


        public string OpenClosedStatusDescription { get; set; }
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


        public bool IsChaseCardHolder { get; set; }
        //
        // Summary:
        //     Million miler level


        public int MillionMilerLevel { get; set; }
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


        public bool? CEO { get; set; }
        //
        // Summary:
        //     Gender

        [JsonConverter(typeof(StringEnumConverter))]

        public Constants.Gender Gender { get; set; }


        public string OpenClosedStatusCode { get; set; }
        //
        // Summary:
        //     Million miler level Desc


        public string MillionMilerLevelDesc { get; set; }


        public bool IsChaseClubCardHolder { get; set; }
    }
}
