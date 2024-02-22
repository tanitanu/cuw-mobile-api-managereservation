using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class Constants
    {

        public enum ServiceName
        {
            GetAddresses = 0,
            CustomerProfile = 1,
            MemberInformation = 2,
            Enrollment = 3,
            UCBBalanceDetails = 4,
            FlightNotifications = 5,
            GetEmail = 6,
            GetPhone = 7,
            Contact = 8,
            SecureTraveler = 9,
            AccountLinkage = 10,
            FormOfPayment = 11,
            ActivityStatement = 12,
            MarketingPreference = 13,
            ValidateAddress = 14,
            DuplicateCheck = 15,
            UclubHistory = 16,
            ValidateMemberName = 17,
            ValidatePhone = 18,
            SeachMemberInfo = 19,
            PartnerCard = 20,
            CustomerMetrics = 21,
            CustomerAttributes = 22,
            Traveler = 23,
            Travelers = 24,
            EmployeeSidaLinkage = 25,
            Pets = 26,
            PartnerLinkage = 27,
            EmployeeLinkage = 28,
            RefreshCache = 29,
            MPTierAndStarAllianceMapping = 30,
            EnrollmentPreferences = 31,
            ServiceAnimal = 32,
            CreditCardData = 33,
            Login = 34,
            CustomerPhotos = 35,
            ReferenceServiceAnimal = 36,
            TravelerBase = 37,
            UpdateLoyaltyIdByEmployeeId = 38,
            UpdateEmployeeIdByLoyaltyId = 39,
            IdentityChange = 40,
            PartnerCards = 41
        }

        public enum ServiceStatus
        {
            Failure = 0,
            Success = 1
        }

        public enum PhoneType
        {
            //
            // Summary:
            //     Home
            H = 0,
            //
            // Summary:
            //     Business
            B = 1,
            //
            // Summary:
            //     Other
            O = 2,
            //
            // Summary:
            //     Unknown
            U = 3
        }

        public enum AddressType
        {
            //
            // Summary:
            //     Home
            H = 0,
            //
            // Summary:
            //     Business
            B = 1,
            //
            // Summary:
            //     Other
            O = 2,
            //
            // Summary:
            //     Unknown
            U = 3
        }

        public enum EmailType
        {
            //
            // Summary:
            //     Home
            H = 0,
            //
            // Summary:
            //     Business
            B = 1,
            //
            // Summary:
            //     Other
            O = 2,
            //
            // Summary:
            //     Unknown
            U = 3
        }

        public enum DeviceType
        {
            PH = 0,
            FX = 1,
            WP = 2,
            P1 = 3,
            P2 = 4,
            OT = 5
        }

        public enum ProgramCurrencyType : byte
        {
            Undefined = 0,
            UBC = 1,
            RPU = 2,
            GPU = 3,
            UGC = 4,
            RDM = 5,
            CAC = 6
        }

        public enum Gender
        {
            //
            // Summary:
            //     Undefined
            Undefined = 0,
            //
            // Summary:
            //     Male
            M = 1,
            //
            // Summary:
            //     Female
            F = 2,
            //
            // Summary:
            //     Unknown
            U = 3,
            //
            // Summary:
            //     Unspecified
            X = 4
        }

        public static class DataSource
        {
            public const string SERVICE = "EXTERNAL SERVICE";
            public const string DB = "DB";
            public const string CACHE = "PERSIST";
        }
    }
}
