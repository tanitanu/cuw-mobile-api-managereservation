using System;
using System.Collections.Generic;
namespace United.Mobile.Model.MPAuthentication
{
    [Serializable()]
    public class MOBMPPINPWDValidateResponse : MOBResponse
    {
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

        private MOBMPAccountSummary opAccountSummary;
        private MOBProfile profile;
        private bool isUASubscriptionsAvailable;
        private MOBUASubscriptions uaSubscriptions;
        public MOBMPAccountSummary OPAccountSummary
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

        public bool ISUASubscriptionsAvailable
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
}
