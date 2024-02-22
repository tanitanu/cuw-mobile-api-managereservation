using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace United.Mobile.Model.MPAuthentication
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
}
