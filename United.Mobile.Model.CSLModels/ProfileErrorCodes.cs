using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public static class ProfileErrorCodes
    {
        private static List<ProfileDictionary> mcs = null;

        public enum ServiceMethodType
        {
            AccountStatus,
            Email,
            EmailVerification,
            MarketingCommunication,
            FlightNotification,
            CreditCard,
            CustomerSearch,
            FlexPQMActivityDetails,
            FlexPQMActivitySummary,
            Miles,
            FlexPQM,
            Phone,
            PartnerCards,
            PrimaryChannel,
            SecureTraveler,
            Traveler,
            MileagePlus,
            TravelBankServiceGet,
            RecentActivityService,
            SecurityUpdate,
            HomePageLogin,
            CorporateUser,
            AdditionalProfileData,
            Language,
            POS,
            EmployeeID,
            CorpFOPException,
            MP11Lookup,
            Address,
            Pets,
            CustomerAttribute,
            Card,
            SearchMPNumber,
            CustomerProfitScore,
            AccountLinkage,
            CustomerContactDetails,
            CustomerProfileData,
            SolicitOffer,
            UClubHistory,
            ChaseSpendPerYearData,
            FlexPQP,
            CustomerLinkage,
            ReferenceDataLinkage,
            ChaseRPC,
            EmployeeDetails,
            CustomerMetrics,
            ProfileOwner
        }

        public enum ErrorType
        {
            InvalidDataProcess,
            InvalidInput,
            InvalidBusinessProcess,
            CircuitBreakerTrips,
            Other
        }


        public static ProfileErrorInfo InputValidationExceptionMapping(ServiceMethodType smt, string message)
        {
            string majorCode = string.Empty;
            string majorDesc = string.Empty;
            GetErrorMajorCode(smt, ref majorCode, ref majorDesc);
            return new ProfileErrorInfo()
            {
                ErrorType = ErrorType.InvalidInput.ToString(),
                MajorCode = majorCode,
                MajorDescription = majorDesc,
                Message = message,
                MinorCode = "66001",
                MinorDescription = GetErrorMinorCodeDescription("66001")
            };
        }
        public static ProfileErrorInfo BusinessExceptionMapping(ServiceMethodType smt, string message)
        {
            string majorCode = string.Empty;
            string majorDesc = string.Empty;
            GetErrorMajorCode(smt, ref majorCode, ref majorDesc);
            return new ProfileErrorInfo()
            {
                ErrorType = ErrorType.InvalidBusinessProcess.ToString(),
                MajorCode = majorCode,
                MajorDescription = majorDesc,
                Message = message,
                MinorCode = "66002",
                MinorDescription = GetErrorMinorCodeDescription("66002")
            };
        }
        public static ProfileErrorInfo CircuitBreakerExceptionMapping(ServiceMethodType smt, string message)
        {
            string majorCode = string.Empty;
            string majorDesc = string.Empty;
            GetErrorMajorCode(smt, ref majorCode, ref majorDesc);
            return new ProfileErrorInfo()
            {
                ErrorType = ErrorType.CircuitBreakerTrips.ToString(),
                MajorCode = majorCode,
                MajorDescription = majorDesc,
                Message = message,
                MinorCode = "66003",
                MinorDescription = GetErrorMinorCodeDescription("66003")
            };
        }
        public static ProfileErrorInfo DataProcessingExceptionMapping(ServiceMethodType smt, string message)
        {
            string majorCode = string.Empty;
            string majorDesc = string.Empty;
            GetErrorMajorCode(smt, ref majorCode, ref majorDesc);
            return new ProfileErrorInfo()
            {
                ErrorType = ErrorType.InvalidDataProcess.ToString(),
                MajorCode = majorCode,
                MajorDescription = majorDesc,
                Message = message,
                MinorCode = "66004",
                MinorDescription = GetErrorMinorCodeDescription("66004")
            };
        }
        #region GetCodes

        public static void GetErrorMajorCode(ServiceMethodType smt, ref string majorCode, ref string majorDesc)
        {
            switch (smt)
            {
                case ServiceMethodType.MileagePlus:
                    majorCode = "40010.01";
                    majorDesc = "LoyaltyProfile/MileagePlus";
                    break;
                case ServiceMethodType.Email:
                    majorCode = "40010.02";
                    majorDesc = "LoyaltyProfile/Email";
                    break;
                case ServiceMethodType.MarketingCommunication:
                    majorCode = "40010.03";
                    majorDesc = "LoyaltyProfile/MarketingCommunication";
                    break;
                case ServiceMethodType.FlightNotification:
                    majorCode = "41010.04";
                    majorDesc = "LoyaltyProfile/FlightNotifications";
                    break;
                case ServiceMethodType.CreditCard:
                    majorCode = "41010.05";
                    majorDesc = "LoyaltyProfile/CreditCard";
                    break;
                case ServiceMethodType.CustomerSearch:
                    majorCode = "41010.06";
                    majorDesc = "LoyaltyProfile/CustomerSearch";
                    break;
                case ServiceMethodType.FlexPQMActivityDetails:
                    majorCode = "41010.07";
                    majorDesc = "LoyaltyProfile/FlexPQMActivity";
                    break;
                case ServiceMethodType.FlexPQMActivitySummary:
                    majorCode = "41010.08";
                    majorDesc = "LoyaltyProfile/FlexPQMActivitySummary";
                    break;
                case ServiceMethodType.Miles:
                    majorCode = "41010.09";
                    majorDesc = "LoyaltyProfile/Miles";
                    break;
                case ServiceMethodType.FlexPQM:
                    majorCode = "41010.1";
                    majorDesc = "LoyaltyProfile/FlexPQM";
                    break;
                case ServiceMethodType.Phone:
                    majorCode = "41010.11";
                    majorDesc = "LoyaltyProfile/Phone";
                    break;
                case ServiceMethodType.PartnerCards:
                    majorCode = "41010.12";
                    majorDesc = "LoyaltyProfile/PartnerCards";
                    break;
                case ServiceMethodType.PrimaryChannel:
                    majorCode = "41010.13";
                    majorDesc = "LoyaltyProfile/PrimaryChannel";
                    break;
                case ServiceMethodType.SecureTraveler:
                    majorCode = "41010.14";
                    majorDesc = "LoyaltyProfile/SecureTraveler";
                    break;
                case ServiceMethodType.Traveler:
                    majorCode = "41010.15";
                    majorDesc = "LoyaltyProfile/Traveler";
                    break;
                case ServiceMethodType.CorpFOPException:
                    majorCode = "41010.16";
                    majorDesc = "LoyaltyProfile/CorpFOPException";
                    break;
                case ServiceMethodType.MP11Lookup:
                    majorCode = "41010.17";
                    majorDesc = "LoyaltyProfile/MP11Lookup";
                    break;
                case ServiceMethodType.Address:
                    majorCode = "41010.18";
                    majorDesc = "LoyaltyProfile/Address";
                    break;
                case ServiceMethodType.Pets:
                    majorCode = "41010.19";
                    majorDesc = "LoyaltyProfile/Pets";
                    break;
                case ServiceMethodType.Card:
                    majorCode = "41010.19";
                    majorDesc = "LoyaltyProfile/Card";
                    break;
                case ServiceMethodType.SearchMPNumber:
                    majorCode = "41010.20";
                    majorDesc = "LoyaltyProfile/SearchMPNumber";
                    break;
                case ServiceMethodType.AccountLinkage:
                    majorCode = "41010.21";
                    majorDesc = "LoyaltyProfile/AccountLinkage";
                    break;
                case ServiceMethodType.CustomerProfileData:
                    majorCode = "41010.22";
                    majorDesc = "LoyaltyProfile/CustomerProfileData";
                    break;
                case ServiceMethodType.CustomerContactDetails:
                    majorCode = "41010.23";
                    majorDesc = "LoyaltyProfile/CustomerContactDetails";
                    break;
                case ServiceMethodType.EmployeeDetails:
                    majorCode = "41010.24";
                    majorDesc = "LoyaltyProfile/EmployeeDetails";
                    break;
                case ServiceMethodType.CustomerMetrics:
                    majorCode = "41010.25";
                    majorDesc = "LoyaltyProfile/CustomerMetrics";
                    break;
                case ServiceMethodType.EmailVerification:
                    majorCode = "41010.26";
                    majorDesc = "LoyaltyProfile/EmailVerification";
                    break;
                default:
                    majorCode = "(unknown)";
                    majorDesc = "(unknown)";
                    break;
            }
        }

        public static string GetErrorMinorCodeDescription(string minorCode)
        {
            string minorDesc = String.Empty;

            ProfileDictionary code = GetMinorCodeList().FirstOrDefault(c => c.Key == minorCode);
            if (code != null)
            {
                minorDesc = code.Value;
            }
            return minorDesc;
        }

        //TODO: Improve this
        private static List<ProfileDictionary> GetMinorCodeList()
        {
            if (mcs == null)
            {
                mcs = new List<ProfileDictionary>();
                AddMC(mcs, "66001", "Input Validation Failed");
                AddMC(mcs, "66002", "Business Process Failed");
                AddMC(mcs, "66003", "Circuit Breaker Tripped");
                AddMC(mcs, "66004", "Data Processing failed");
                AddMC(mcs, "66005", "MP Number not found");
                AddMC(mcs, "66006", "Multiple MP Numbers found");
            }
            return mcs;
        }

        private static void AddMC(List<ProfileDictionary> codes, string minorCode, string minorDesc)
        {
            codes.Add(new ProfileDictionary(minorCode, minorDesc));
        }

        #endregion GetCodes
    }
}
