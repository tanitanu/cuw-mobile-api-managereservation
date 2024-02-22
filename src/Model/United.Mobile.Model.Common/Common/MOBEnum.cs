using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public enum MOBSHOPResponseStatus
    {
        [EnumMember(Value = "1")]
        ReshopUnableToComplete,
        [EnumMember(Value = "2")]
        ReshopChangePending,
        [EnumMember(Value = "3")]
        ReshopBENonElgible,
        [EnumMember(Value = "4")]
        ReshopUnableToChange,
        [EnumMember(Value = "5")]
        PcuUpgradeFailed,
        [EnumMember(Value = "6")]
        FailedToGetBagChargeInfo,
        [EnumMember(Value = "7")]
        ReshopAgencyCheckinEligible,
        [EnumMember(Value = "8")]
        ReshopCheckinEligible,
        [EnumMember(Value = "9")]
        ReshopChangeOfferBEBuyOut,
        [EnumMember(Value = "10")]
        ReshopOTFShopEligible,
    }


    [Serializable]
    public enum MOBMPErrorScreenType
    {
        [EnumMember(Value = "None")]
        None,
        [EnumMember(Value = "Common")]
        Common,
        [EnumMember(Value = "AccountNotFound")]
        AccountNotFound,
        [EnumMember(Value = "Duplicate")]
        Duplicate,
        [EnumMember(Value = "UnableToReset")]
        UnableToReset
    }


    [Serializable]
    public enum MOBMPSecurityUpdatePath
    {
        [EnumMember(Value = "None")]
        None,
        [EnumMember(Value = "VerifyPrimaryEmail")]
        VerifyPrimaryEmail,
        [EnumMember(Value = "NoPrimayEmailExist")]
        NoPrimayEmailExist,
        [EnumMember(Value = "UpdatePassword")]
        UpdatePassword,
        [EnumMember(Value = "UpdateSecurityQuestions")]
        UpdateSecurityQuestions,
        [EnumMember(Value = "SignInBackWithNewPassWord")]
        SignInBackWithNewPassWord,
        [EnumMember(Value = "ForgotMileagePlusNumber")]
        ForgotMileagePlusNumber,
        [EnumMember(Value = "ForgotMPPassWord")]
        ForgotMPPassWord,
        [EnumMember(Value = "ValidateSecurityQuestions")]
        ValidateSecurityQuestions,
        [EnumMember(Value = "IncorrectSecurityQuestion")]
        IncorrectSecurityQuestion,
        [EnumMember(Value = "IncorrectUserDetails")]
        IncorrectUserDetails,
        [EnumMember(Value = "UnableToResetOnline")]
        UnableToResetOnline,
        [EnumMember(Value = "AccountLocked")]
        AccountLocked,
        [EnumMember(Value = "MultipleAccount")]
        MultipleAccount,
        [EnumMember(Value = "ValidateTFASecurityQuestions")]
        ValidateTFASecurityQuestions,
        [EnumMember(Value = "TFAAccountLocked")]
        TFAAccountLocked,
        [EnumMember(Value = "TFAForgotPasswordEmail")]
        TFAForgotPasswordEmail,
        [EnumMember(Value = "TFAAccountResetEmail")]
        TFAAccountResetEmail,
        [EnumMember(Value = "TFAInvalidAccountResetEmail")]
        TFAInvalidAccountResetEmail

        // when forceSignOut is true for update later is disabled at ValidateMPSignIn() as its time to update the Security Data and will be forced to update data to move forward (As of now here too other than Revenue Booking) - 
        // - than after update password success then need to force client to sign in back with re-enter the new password and sign in.
        //“VerifyPrimaryEmail” means need to verify saved primary email
        //“NoPrimayEmailExist” means no primary email exists
        //“UpdatePassword” means a valid password does not exist
        //“UpdateSecurityQuestions” means the 5 security questions and answers do not exist
    }

    [Serializable]
    public enum MOBMPSignInPath
    {
        [EnumMember(Value = "None")]
        None,
        [EnumMember(Value = "MyAccountPath")]
        MyAccountPath,
        [EnumMember(Value = "AwardTBookingPath")]
        AwardTBookingPath,
        [EnumMember(Value = "RevenueBookingPath")]
        RevenueBookingPath,
        [EnumMember(Value = "CorporateBookingPath")]
        CorporateBookingPath,
        [EnumMember(Value = "CorporateChangePath")]
        CorporateChangePath
    }

    public enum TravelSpecialNeedType
    {
        Unknown,
        SpecialMeal,
        SpecialRequest,
        ServiceAnimal,
        ServiceAnimalType,
        TravelSpecialNeedInfo
    };

    public enum IConType
    {
        None,
        Warning,
        Info,
        Error,
        Question,
    };

    public enum PostPurchasePage
    {
        None,
        PNRRetrival,
        Confirmation,
        SecondaryFormOfPayment
    };

    public enum ErrorCodes
    {
        [EnumMember(Value = "900111")]
        ViewResCFOPSessionExpire = 900111, //Error code for session expire in View/Manage reservation flow implemented during Common FOP
        [EnumMember(Value = "900112")]
        ViewResCFOP_NullSession_AfterAppUpgradation = 900112, //Error code for null session if the app get update during the process. Actual Message: "We're sorry, we are currently updating the mobile app. Please reload your reservation."
    };

    public enum SeatType
    {
        BLUE,
        PREF,
        STANDARD,
        PURPLE,
        FBLEFT,
        FBRIGHT,
        FBBACK,
        FBFRONT,
        DAAFRONTL,
        DAAFRONTR,
        DAAFRONTRM,
        DAALEFT,
        DAARIGHT,
        FRONT
    };

    public enum SeatValue
    {
        X,
        P,
        PZ,
        O
    };

    public enum SeatPosition
    {
        FBB,
        FBF,
        FBL,
        FBR,
        DAFL,
        DAFR,
        DAL,
        DAR,
        DAFRM
    }

    public enum SeatMapName
    {
        FlightStatusPreviewSeatMap,
        EResPreviewSeatMap,
        BookingPreviewSeatMap,
        BookingSeatMap,
        ManageResSeatMap,
        ManageResOfferTile
    }
    public enum MOBIConType
    {
        None,
        Warning,
        Info,
        Error,
        Question,
    };

    [Serializable()]
    public enum TripType
    {
        RoundTrip = 1,
        OneWay = 2
    }

    [Serializable()]
    public enum ReservationType
    {
        All = 0,
        Current = 1,
        Past = 2,
        Cancelled = 3,
        Inactive = 4
    }

    [Serializable()]
    public enum PASegmentType
    {
        [EnumMember(Value = "0")]
        Regular = 0,
        [EnumMember(Value = "1")]
        SoldOut = 1,
        [EnumMember(Value = "2")]
        NotOffered = 2,
        [EnumMember(Value = "3")]
        AlreadyPurchased = 3
    }

    [Serializable()]
    public enum PACustomerType
    {
        [EnumMember(Value = "0")]
        Regular = 0,
        [EnumMember(Value = "1")]
        AlreadyPurchased = 1,
        [EnumMember(Value = "2")]
        AlreadyPremier = 2
    }

    [Serializable()]
    public enum PBSegmentType
    {
        [EnumMember(Value = "0")]
        Regular = 0,
        [EnumMember(Value = "1")]
        AlreadyPurchased = 1,
        [EnumMember(Value = "2")]
        InEligible = 2,
        [EnumMember(Value = "3")]
        Included = 3
    }

    [Serializable()]
    public enum EarningDisplayType
    {
        [EnumMember(Value = "VBQ")]
        VBQ = 0,
        [EnumMember(Value = "LMX")]
        LMX = 1,
        [EnumMember(Value = "LMXVBQ")]
        LMXVBQ = 2
    }

    [Serializable()]
    public enum MOBFSREnhancementType
    {
        [EnumMember(Value = "0")]
        NoResultsSuggestNearByAirports = 0,
        [EnumMember(Value = "1")]
        WithResultsSuggestNearByOrigins = 1,
        [EnumMember(Value = "2")]
        WithResultsSuggestNearByDestinations = 2,
        [EnumMember(Value = "3")]
        WithResultsSuggestNearByOrigsAndDests = 3,
        [EnumMember(Value = "4")]
        NoResultsDestinationSeasonal = 4,
        [EnumMember(Value = "5")]
        NoResultsOriginSeasonal = 5,
        [EnumMember(Value = "6")]
        NoResultsOrigAndDestSeasonal = 6,
        [EnumMember(Value = "7")]
        SuggestNonStopFutureDate = 7,
        [EnumMember(Value = "8")]
        WithResultsForceNearByOrigin = 8,
        [EnumMember(Value = "9")]
        WithResultsForceNearByDestination = 9,
        [EnumMember(Value = "10")]
        WithResultsForceNearByOrigAndDest = 10,
        [EnumMember(Value = "11")]
        WithResultsForceGSTByOrigin = 11,
        [EnumMember(Value = "12")]
        WithResultsShareTripSuggestedByDate = 12,
    }

    [Serializable()]
    public enum MOBPBSegmentType
    {
        [EnumMember(Value = "0")]
        Regular = 0,
        [EnumMember(Value = "1")]
        AlreadyPurchased = 1,
        [EnumMember(Value = "2")]
        InEligible = 2,
        [EnumMember(Value = "3")]
        Included = 3
    }

    [Serializable()]
    public enum MOBPASegmentType
    {
        [EnumMember(Value = "0")]
        Regular = 0,
        [EnumMember(Value = "1")]
        SoldOut = 1,
        [EnumMember(Value = "2")]
        NotOffered = 2,
        [EnumMember(Value = "3")]
        AlreadyPurchased = 3
    }

    [Serializable()]
    public enum MOBPACustomerType
    {
        [EnumMember(Value = "0")]
        Regular = 0,
        [EnumMember(Value = "1")]
        AlreadyPurchased = 1,
        [EnumMember(Value = "2")]
        AlreadyPremier = 2
    }

    [Serializable()]
    public enum MOBReservationType
    {
        All = 0,
        Current = 1,
        Past = 2,
        Cancelled = 3,
        Inactive = 4
    }
    public enum MOBNavigationToScreen
    {
        TRAVELOPTIONS,
        SEATS,
        FINALRTI
    }

    [Serializable()]
    public enum MOBEarningDisplayType
    {
        [EnumMember(Value = "VBQ")]
        VBQ = 0,
        [EnumMember(Value = "LMX")]
        LMX = 1,
        [EnumMember(Value = "LMXVBQ")]
        LMXVBQ = 2
    }
    [Serializable]
    public enum CreditTypeColor
    {
        [EnumMember(Value = "NONE")] //DEFAULT
        NONE,
        [EnumMember(Value = "GREEN")]
        GREEN,
    }

    [Serializable]
    public enum CreditType
    {
        [EnumMember(Value = "NONE")] //DEFAULT
        NONE,
        [Display(Name = "Refund")]
        [EnumMember(Value = "REFUND")]
        REFUND,
        [Display(Name = "Flight credit")]
        [EnumMember(Value = "FLIGHTCREDIT")]
        FLIGHTCREDIT
    }
    public enum CartClearOption
    {
        ClearBundles,
        ClearSeats
    }
    public enum ServiceNames
    {
        PREORDERMEALS,
        UPGRADECABIN,
        CANCELRESERVATION,
        MANAGERESERVATION,
        VIEWRESSEATMAP,
        SEATPREFERENCE
    }
}
