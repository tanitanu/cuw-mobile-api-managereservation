using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping;
using Newtonsoft.Json;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBMPPINPWDValidateResponse : MOBResponse
    {
        [JsonIgnore()]
        public string ObjectName { get; set; } = "United.Definition.MPPINPWD.MOBMPPINPWDValidateResponse";
        private readonly IConfiguration _configuration;
        public MOBMPPINPWDValidateResponse()
            : base()
        {
        }

        private MOBSHOPResponseStatusItem responseStatusItem;
        private MOBYoungAdultTravelType youngAdultTravelType;

        public MOBYoungAdultTravelType YoungAdultTravelType
        {
            get
            {
                return youngAdultTravelType;
            }
            set
            {
                youngAdultTravelType = value;
            }
        }
        public MOBSHOPResponseStatusItem ResponseStatusItem
        {
            get { return responseStatusItem; }
            set { responseStatusItem = value; }
        }

        private MOBMPAccountValidationRequest request;
        public MOBMPAccountValidationRequest Request
        {
            get
            {
                return this.request;
            }
            set
            {
                this.request = value;
            }
        }

        private MOBMPAccountValidation accountValidation;
        public MOBMPAccountValidation AccountValidation
        {
            get
            {
                return this.accountValidation;
            }
            set
            {
                this.accountValidation = value;
            }
        }

        private bool securityUpdate;
        public bool SecurityUpdate
        {
            get { return this.securityUpdate; }
            set { this.securityUpdate = value; }
        }

        private MOBMPPINPWDSecurityUpdateDetails mpSecurityUpdateDetails;
        public MOBMPPINPWDSecurityUpdateDetails MPSecurityUpdateDetails
        {
            get
            {
                return this.mpSecurityUpdateDetails;
            }
            set
            {
                this.mpSecurityUpdateDetails = value;
            }
        }

        private string sessionID = string.Empty;
        public string SessionID
        {
            get
            {
                return this.sessionID;
            }
            set
            {
                this.sessionID = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }

        private List<MOBItem> landingPageMessages;
        public List<MOBItem> LandingPageMessages
        {
            get
            {
                return this.landingPageMessages;
            }
            set
            {
                this.landingPageMessages = value;
            }
        }

        private MPAccountSummary opAccountSummary;
        private MOBProfile profile;
        public bool isUASubscriptionsAvailable { get; set; }
        private MOBUASubscriptions uaSubscriptions;
        public MPAccountSummary OPAccountSummary
        {
            get
            {
                return this.opAccountSummary;
            }
            set
            {
                this.opAccountSummary = value;
            }
        }

        public MOBProfile Profile
        {
            get
            {
                return this.profile;
            }
            set
            {
                this.profile = value;
            }
        }


        public MOBUASubscriptions UASubscriptions
        {
            get
            {
                return this.uaSubscriptions;
            }
            set
            {
                this.uaSubscriptions = value;
            }
        }

        private bool isExpertModeEnabled;
        public bool IsExpertModeEnabled { get { return this.isExpertModeEnabled; } set { this.isExpertModeEnabled = value; } }

        private string employeeId = string.Empty;
        public string EmployeeId
        {
            get
            {
                return this.employeeId;
            }
            set
            {
                this.employeeId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        private string displayEmployeeId = string.Empty;
        public string DisplayEmployeeId
        {
            get
            {
                return this.displayEmployeeId;
            }
            set
            {
                this.displayEmployeeId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        private MOBEmpTravelTypeResponse empTravelTypeResponse;
        public MOBEmpTravelTypeResponse EmpTravelTypeResponse
        {
            get
            {
                return this.empTravelTypeResponse;
            }
            set
            {
                this.empTravelTypeResponse = value;
            }
        }

        private MOBCorporateTravelType corporateEligibleTravelType;

        public MOBCorporateTravelType CorporateEligibleTravelType
        {
            get { return corporateEligibleTravelType; }
            set { corporateEligibleTravelType = value; }
        }

        private MOBMPTFARememberMeFlags rememberMEFlags;
        public MOBMPTFARememberMeFlags RememberMEFlags
        {
            get
            {
                if (rememberMEFlags == null)
                {
                    rememberMEFlags = new MOBMPTFARememberMeFlags();
                }
                return this.rememberMEFlags;
            }
            set
            {
                this.rememberMEFlags = value;
            }
        }

        private bool showContinueAsGuestButton;
        public bool ShowContinueAsGuestButton
        {
            get
            {
                return showContinueAsGuestButton;
            }
            set { showContinueAsGuestButton = value; }
        }

        private MOBCPCustomerMetrics customerMetrics;
        public MOBCPCustomerMetrics CustomerMetrics
        {
            get { return customerMetrics; }
            set { customerMetrics = value; }
        }
    }

    #region MP Enroll

    [Serializable()]
    public class MOBMPEnRollmentRequest : MOBRequest
    {
        private readonly IConfiguration _configuration;
        public MOBMPEnRollmentRequest()
            : base()
        {

        }

        private string sessionID = string.Empty;
        public string SessionID
        {
            get
            {
                return this.sessionID;
            }
            set
            {
                this.sessionID = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }

        private bool getSecurityQuestions;
        public bool GetSecurityQuestions
        {
            get { return this.getSecurityQuestions; }
            set { this.getSecurityQuestions = value; }
        }

        private MOBMPMPEnRollmentDetails mpEnrollmentDetails;
        public MOBMPMPEnRollmentDetails MPEnrollmentDetails
        {
            get
            {
                return this.mpEnrollmentDetails;
            }
            set
            {
                this.mpEnrollmentDetails = value;
            }
        }
    }

    [Serializable()]
    public class MOBMPMPEnRollmentResponse : MOBResponse
    {
        private readonly IConfiguration _configuration;
        public MOBMPMPEnRollmentResponse()
            : base()
        {
            if (_configuration != null)
            {
                needQuestionsCount = _configuration.GetValue<int>("NumberOfSecurityQuestionsNeedatPINPWDUpdate");
            }
        }
        private string mileagePlusNumber = string.Empty;
        public string MileagePlusNumber
        {
            get
            {
                return this.mileagePlusNumber;
            }
            set
            {
                this.mileagePlusNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        private MOBMPEnRollmentRequest request;
        public MOBMPEnRollmentRequest Request
        {
            get
            {
                return this.request;
            }
            set
            {
                this.request = value;
            }
        }

        private string sessionID = string.Empty;
        public string SessionID
        {
            get
            {
                return this.sessionID;
            }
            set
            {
                this.sessionID = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }

        private string mpEnrollMentCompleteMessage;
        public string MPEnrollMentCompleteMessage
        {
            get
            {
                return this.mpEnrollMentCompleteMessage;
            }
            set
            {
                this.mpEnrollMentCompleteMessage = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }

        private MOBYoungAdultTravelType youngAdultTravelType;
        public MOBYoungAdultTravelType YoungAdultTravelType
        {
            get
            {
                return youngAdultTravelType;
            }
            set
            {
                youngAdultTravelType = value;
            }
        }

        private MPAccountSummary opAccountSummary;
        public MPAccountSummary OPAccountSummary
        {
            get
            {
                return this.opAccountSummary;
            }
            set
            {
                this.opAccountSummary = value;
            }
        }

        private int needQuestionsCount;
        public int NeedQuestionsCount
        {
            get
            {
                return this.needQuestionsCount;
            }
            set
            {
                this.needQuestionsCount = value;
            }
        }

        private List<Securityquestion> securityQuestions;
        public List<Securityquestion> SecurityQuestions
        {
            get { return securityQuestions; }
            set { securityQuestions = value; }
        }

        private List<MOBItem> mpEnrollmentMessages;
        public List<MOBItem> MPEnrollmentMessages
        {
            get
            {
                return this.mpEnrollmentMessages;
            }
            set
            {
                this.mpEnrollmentMessages = value;
            }
        }

        private MOBMPAccountValidation accountValidation;
        public MOBMPAccountValidation AccountValidation
        {
            get
            {
                return this.accountValidation;
            }
            set
            {
                this.accountValidation = value;
            }
        }

        private MOBMPTFARememberMeFlags rememberMEFlags;
        public MOBMPTFARememberMeFlags RememberMEFlags
        {
            get
            {
                if (rememberMEFlags == null)
                {
                    rememberMEFlags = new MOBMPTFARememberMeFlags();
                }
                return this.rememberMEFlags;
            }
            set
            {
                this.rememberMEFlags = value;
            }
        }
    }

    [Serializable()]
    public class MOBMPMPEnRollmentDetails
    {
        public MOBMPMPEnRollmentDetails()
            : base()
        {
        }

        private MOBMPEnrollmentPersonalInfo personalInformation;
        public MOBMPEnrollmentPersonalInfo PersonalInformation
        {
            get
            {
                return this.personalInformation;
            }
            set
            {
                this.personalInformation = value;
            }
        }

        private MOBMPEnrollmentContactInfo contactInformation;
        public MOBMPEnrollmentContactInfo ContactInformation
        {
            get
            {
                return this.contactInformation;
            }
            set
            {
                this.contactInformation = value;
            }
        }

        private MOBMPEnrollmentSecurityInfo securityInformation;
        public MOBMPEnrollmentSecurityInfo SecurityInformation
        {
            get
            {
                return this.securityInformation;
            }
            set
            {
                this.securityInformation = value;
            }
        }

        private MOBMPEnrollmentSubscriptions subscriptionPreferences;
        public MOBMPEnrollmentSubscriptions SubscriptionPreferences
        {
            get
            {
                return this.subscriptionPreferences;
            }
            set
            {
                this.subscriptionPreferences = value;
            }
        }
    }

    [Serializable]
    public class MOBMPEnrollmentSubscriptions
    {
        private bool unitedNewsnDeals;
        public bool UnitedNewsnDeals
        {
            get { return unitedNewsnDeals; }
            set { unitedNewsnDeals = value; }
        }

        private bool mpPartnerOffers;
        public bool MPPartnerOffers
        {
            get { return mpPartnerOffers; }
            set { mpPartnerOffers = value; }
        }

        private bool mileagePlusProgram;
        public bool MileagePlusProgram
        {
            get { return mileagePlusProgram; }
            set { mileagePlusProgram = value; }
        }

        private bool mileagePlusStmt;
        public bool MileagePlusStmt
        {
            get { return mileagePlusStmt; }
            set { mileagePlusStmt = value; }
        }

    }

    [Serializable()]
    public class MOBMPEnrollmentSecurityInfo
    {
        private string telephonePIN;
        public string TelephonePIN
        {
            get { return telephonePIN; }
            set { telephonePIN = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string userName;
        public string UserName
        {
            get { return userName; }
            set { userName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string passWord;
        public string PassWord
        {
            get { return passWord; }
            set { passWord = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private List<Securityquestion> selectedSecurityQuestions;
        public List<Securityquestion> SelectedSecurityQuestions
        {
            get { return selectedSecurityQuestions; }
            set { selectedSecurityQuestions = value; }
        }
    }

    [Serializable()]
    public class MOBMPEnrollmentContactInfo
    {
        private string streetAddress;

        public string StreetAddress
        {
            get { return streetAddress; }
            set { streetAddress = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string streetAddress2;

        public string StreetAddress2
        {
            get { return streetAddress2; }
            set { streetAddress2 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string cityRtown;

        public string CityRTown
        {
            get { return cityRtown; }
            set { cityRtown = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private MOBState state;

        public MOBState State
        {
            get { return state; }
            set { state = value; }
        }

        private string zipRpostalCode;

        public string ZipRpostalCode
        {
            get { return zipRpostalCode; }
            set { zipRpostalCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private MOBCountry country;

        public MOBCountry Country
        {
            get { return country; }
            set { country = value; }
        }

        private string phoneNumber;

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string phoneCountryCode = string.Empty;

        public string PhoneCountryCode
        {
            get
            {
                return this.phoneCountryCode;
            }
            set
            {
                this.phoneCountryCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        private string emailAddress;

        public string EmailAddress
        {
            get { return emailAddress; }
            set { emailAddress = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

    }

    [Serializable()]
    public class MOBMPEnrollmentPersonalInfo
    {
        private string title = string.Empty;
        private string firstName = string.Empty;
        private string middleName = string.Empty;
        private string lastName = string.Empty;
        private string suffix = string.Empty;
        private string birthDate = string.Empty;
        private string gender = string.Empty;

        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string FirstName
        {
            get
            {
                return this.firstName;
            }
            set
            {
                this.firstName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string MiddleName
        {
            get
            {
                return this.middleName;
            }
            set
            {
                this.middleName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string LastName
        {
            get
            {
                return this.lastName;
            }
            set
            {
                this.lastName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Suffix
        {
            get
            {
                return this.suffix;
            }
            set
            {
                this.suffix = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string BirthDate
        {
            get
            {
                return this.birthDate;
            }
            set
            {
                this.birthDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Gender
        {
            get
            {
                return this.gender;
            }
            set
            {
                this.gender = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
    #region MPSignInNeedHelp
    [Serializable()]
    public class MOBMPSignInNeedHelpRequest : MOBRequest
    {
        public MOBMPSignInNeedHelpRequest()
            : base()
        {
        }
        public string SessionID { get; set; } = string.Empty;
        public string MileagePlusNumber { get; set; } = string.Empty;
        public MOBMPSecurityUpdatePath SecurityUpdateType { get; set; }
        public MOBMPSignInNeedHelpItems MPSignInNeedHelpItems { get; set; }
    }

    [Serializable()]
    public class MOBMPSignInNeedHelpResponse : MOBResponse
    {
        public MOBMPSignInNeedHelpResponse()
            : base()
        {
        }
        public string MileagePlusNumber { get; set; } = string.Empty;
        public MOBMPSignInNeedHelpRequest Request { get; set; }
        public MOBMPSignInNeedHelpItemsDetails MPSignInNeedHelpDetails { get; set; }
        public string SessionID { get; set; } = string.Empty;
        public string NeedHelpCompleteMessage { get; set; } = string.Empty;
        public MOBMPSecurityUpdatePath NeedHelpSecurityPath { get; set; }
        public MOBMPTFARememberMeFlags RememberMEFlags { get; set; }
        public MOBMPErrorScreenType ErrorScreenType { get; set; }
        public Dictionary<string, string> SearchCriteria { get; set; }

    }


    [Serializable()]
    public class MOBMPSignInNeedHelpItems
    {
        public MOBMPSignInNeedHelpItems()
            : base()
        {

        }
        public List<Securityquestion> AnsweredSecurityQuestions { get; set; }
        public MOBName NeedHelpSignInInfo { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string MileagePlusNumber { get; set; } = string.Empty;
        public string UpdatedPassword { get; set; } = string.Empty;

    }

    [Serializable()]
    public class MOBMPSignInNeedHelpItemsDetails
    {
        public MOBMPSignInNeedHelpItemsDetails()
            : base()
        {
        }
        public List<Securityquestion> SecurityQuestions { get; set; }
        public List<MOBItem> NeedHelpMessages { get; set; }
    }
    #endregion
    #endregion
    [Serializable()]
    public class MOBTFAMPDeviceRequest : MOBRequest
    {
        public MOBTFAMPDeviceRequest()
            : base()
        {
        }

        private string sessionID = string.Empty;
        public string SessionID
        {
            get
            {
                return this.sessionID;
            }
            set
            {
                this.sessionID = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }

        private string mileagePlusNumber = string.Empty;
        public string MileagePlusNumber
        {
            get
            {
                return this.mileagePlusNumber;
            }
            set
            {
                this.mileagePlusNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        private int customerID;
        public int CustomerID
        {
            get
            {
                return this.customerID;
            }
            set
            {
                this.customerID = value;
            }
        }

        private string hashValue = string.Empty;
        public string HashValue
        {
            get
            {
                return this.hashValue;
            }
            set
            {
                this.hashValue = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }

        public MOBMPSecurityUpdatePath tFAMPDeviceSecurityPath
        {
            get; set;
        }


        private List<Securityquestion> answeredSecurityQuestions;
        public List<Securityquestion> AnsweredSecurityQuestions
        {
            get { return answeredSecurityQuestions; }
            set { answeredSecurityQuestions = value; }
        }

        private bool rememberDevice;
        public bool RememberDevice
        {
            get { return rememberDevice; }
            set { rememberDevice = value; }
        }
    }
    [Serializable()]
    public class MOBTFAMPDeviceResponse : MOBResponse
    {
        private readonly IConfiguration _configuration;
        public MOBTFAMPDeviceResponse()
            : base()
        {
            tFAMPDeviceMessages = new List<MOBItem>();
        }

        private string sessionID = string.Empty;
        public string SessionID
        {
            get
            {
                return this.sessionID;
            }
            set
            {
                this.sessionID = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }

        private string mileagePlusNumber = string.Empty;
        public string MileagePlusNumber
        {
            get
            {
                return this.mileagePlusNumber;
            }
            set
            {
                this.mileagePlusNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        private int customerID;
        public int CustomerID
        {
            get
            {
                return this.customerID;
            }
            set
            {
                this.customerID = value;
            }
        }

        private string hashValue = string.Empty;
        public string HashValue
        {
            get { return hashValue; }
            set { hashValue = value; }
        }

        private bool securityUpdate;  // This will be true only if Security Questions are anwered incorrectly and this will make client to check the TFAMPDeviceSecurityPath value if to go display thrid question or Accout is locked 
        public bool SecurityUpdate
        {
            get { return this.securityUpdate; }
            set { this.securityUpdate = value; }
        }

        public MOBMPSecurityUpdatePath tFAMPDeviceSecurityPath { get; set; }
        private MOBTFAMPDeviceRequest request;
        public MOBTFAMPDeviceRequest Request
        {
            get
            {
                return this.request;
            }
            set
            {
                this.request = value;
            }
        }

        private List<Securityquestion> securityQuestions;
        public List<Securityquestion> SecurityQuestions
        {
            get { return securityQuestions; }
            set { securityQuestions = value; }
        }

        public List<MOBItem> tFAMPDeviceMessages { get; set; }


        public string tFAMPDeviceCompleteMessage { get; set; }


        private MOBMPTFARememberMeFlags rememberMEFlags;
        public MOBMPTFARememberMeFlags RememberMEFlags
        {
            get
            {
                if (rememberMEFlags == null)
                {
                    rememberMEFlags = new MOBMPTFARememberMeFlags();
                }
                return this.rememberMEFlags;
            }
            set
            {
                this.rememberMEFlags = value;
            }
        }
        private bool showContinueAsGuestButton;
        public bool ShowContinueAsGuestButton
        {
            get
            {
                return showContinueAsGuestButton;
            }
            set { showContinueAsGuestButton = value; }
        }
        private MPAccountSummary opAccountSummary;
        public MPAccountSummary OPAccountSummary
        {
            get
            {
                return this.opAccountSummary;
            }
            set
            {
                this.opAccountSummary = value;
            }
        }
        private MOBCorporateTravelType corporateEligibleTravelType;
        public MOBCorporateTravelType CorporateEligibleTravelType
        {
            get
            {
                return this.corporateEligibleTravelType;
            }
            set
            {
                this.corporateEligibleTravelType = value;
            }
        }
        private MOBCPCustomerMetrics customerMetrics;
        public MOBCPCustomerMetrics CustomerMetrics
        {
            get
            {
                return this.customerMetrics;
            }
            set
            {
                this.customerMetrics = value;
            }
        }
        private string employeeId;
        public string EmployeeId
        {
            get
            {
                return this.employeeId;
            }
            set
            {
                this.employeeId = value;
            }
        }
        private MOBEmpTravelTypeResponse empTravelTypeResponse;
        public MOBEmpTravelTypeResponse EmpTravelTypeResponse
        {
            get
            {
                return this.empTravelTypeResponse;
            }
            set
            {
                this.empTravelTypeResponse = value;
            }
        }
        private MOBUASubscriptions uaSubscriptions;
        public MOBUASubscriptions UASubscriptions
        {
            get
            {
                return this.uaSubscriptions;
            }
            set
            {
                this.uaSubscriptions = value;
            }
        }
        private bool isUASubscriptionsAvailable;
        public bool IsUASubscriptionsAvailable
        {
            get
            {
                return this.isUASubscriptionsAvailable;
            }
            set
            {
                this.isUASubscriptionsAvailable = value;
            }
        }
        private MOBYoungAdultTravelType youngAdultTravelType;
        public MOBYoungAdultTravelType YoungAdultTravelType
        {
            get
            {
                return this.youngAdultTravelType;
            }
            set
            {
                this.youngAdultTravelType = value;
            }
        }

    }
}
