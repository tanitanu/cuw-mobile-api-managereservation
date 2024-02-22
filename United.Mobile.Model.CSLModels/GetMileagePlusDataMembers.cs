using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class GetMileagePlusDataMembers : MileagePlusSharedDataMembers
    {
        //
        // Summary:
        //     Star alliance tier level


        public int StarAllianceTierLevel
        {
            get;
            set;
        }

        //
        // Summary:
        //     Star alliance tier level description


        public string StarAllianceTierLevelDescription
        {
            get;
            set;
        }

        //
        // Summary:
        //     represent is the Employee or not


        public bool IsEmployee
        {
            get;
            set;
        }

        //
        // Summary:
        //     Million miler companion


        public bool MillionMilerCompanion
        {
            get;
            set;
        }

        //
        // Summary:
        //     Million miler level


        public int MillionMilerLevel
        {
            get;
            set;
        }

        //
        // Summary:
        //     MP tier level description


        public string MPTierLevelDescription
        {
            get;
            set;
        }
    }

    public class MileagePlusSharedDataMembers
    {


        public long CustomerID
        {
            get;
            set;
        }



        public string Title
        {
            get;
            set;
        }



        public string FirstName
        {
            get;
            set;
        }



        public string MiddleName
        {
            get;
            set;
        }



        public string LastName
        {
            get;
            set;
        }



        public string Suffix
        {
            get;
            set;
        }



        public string GenderCode
        {
            get;
            set;
        }



        public int? Birth_Month
        {
            get;
            set;
        }



        public int? Birth_Date
        {
            get;
            set;
        }



        public int? Birth_Year
        {
            get;
            set;
        }



        public string FrequentFlyerCarrier
        {
            get;
            set;
        }



        public string MileagePlusId
        {
            get;
            set;
        }



        public DateTime? EnrollDate
        {
            get;
            set;
        }



        public string EnrollId
        {
            get;
            set;
        }



        public string EnrollSourceCode
        {
            get;
            set;
        }



        public string EnrollSourceDescription
        {
            get;
            set;
        }



        public decimal AccountBalance
        {
            get;
            set;
        }



        public decimal EliteMileageBalance
        {
            get;
            set;
        }



        public decimal? EliteSegmentBalance
        {
            get;
            set;
        }



        public string CurrentEliteLevel
        {
            get;
            set;
        }



        public string CurrentEliteLevelDescription
        {
            get;
            set;
        }



        public string FutureEliteLevel
        {
            get;
            set;
        }



        public string FutureEliteLevelDescription
        {
            get;
            set;
        }



        public bool IsCEO
        {
            get;
            set;
        }



        public bool IsInfiniteElite
        {
            get;
            set;
        }



        public bool IsPClubMember
        {
            get;
            set;
        }



        public DateTime? LastActivityDate
        {
            get;
            set;
        }



        public DateTime? LastFlightDate
        {
            get;
            set;
        }



        public string ActiveStatusCode
        {
            get;
            set;
        }



        public string ActiveStatusDescription
        {
            get;
            set;
        }



        public string ClosedStatusCode
        {
            get;
            set;
        }



        public string ClosedStatusDescription
        {
            get;
            set;
        }



        public bool IsMergePending
        {
            get;
            set;
        }



        public bool IsClosedPermanently
        {
            get;
            set;
        }



        public bool IsClosedTemporarily
        {
            get;
            set;
        }



        public DateTime? LastExpiredMileDate
        {
            get;
            set;
        }



        public decimal LastExpiredMile
        {
            get;
            set;
        }



        public bool IsLockedOut
        {
            get;
            set;
        }



        public string Donor
        {
            get;
            set;
        }



        public DateTime? LastStatementDate
        {
            get;
            set;
        }



        public decimal? LastStatementBalance
        {
            get;
            set;
        }



        public decimal SkyTeamEliteLevelCode
        {
            get;
            set;
        }



        public DateTime? InstantEliteExpDate
        {
            get;
            set;
        }



        public bool IsNew
        {
            get;
            set;
        }



        public bool IsSelected
        {
            get;
            set;
        }



        public bool UseCustomer
        {
            get;
            set;
        } = true;




        public decimal AllianceEliteLevel
        {
            get;
            set;
        }



        public decimal CurrentYearMoneySpent
        {
            get;
            set;
        }



        public string PriorUnitedAccountNumber
        {
            get;
            set;
        }



        public DateTime? MileageExpirationDate
        {
            get;
            set;
        }



        public decimal? CurrentYearUASegments
        {
            get;
            set;
        }



        public bool IsChaseSpend
        {
            get;
            set;
        }



        public bool IsPresPlus
        {
            get;
            set;
        }



        public int? ChaseSpendBalance
        {
            get;
            set;
        }



        public int? ChaseSpendUpdate
        {
            get;
            set;
        }



        public decimal? PremierQualifyingPoints
        {
            get;
            set;
        }



        public decimal? PremierQualifyingFlights
        {
            get;
            set;
        }
    }
}
