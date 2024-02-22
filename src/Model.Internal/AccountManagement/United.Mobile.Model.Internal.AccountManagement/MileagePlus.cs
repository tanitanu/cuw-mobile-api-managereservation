using System;

namespace United.Mobile.Model.Internal.AccountManagement
{
    public class MileagePlus
    {
        public string MileagePlusNumber { get; set; } = string.Empty;
        public string MPUserName { get; set; } = string.Empty;
        public string HashPincode { get; set; } = string.Empty;
        public string PinCode { get; set; } = string.Empty;
        public string ApplicationID { get; set; } = string.Empty;
        public string AppVersion { get; set; } = string.Empty;
        public string DeviceID { get; set; } = string.Empty;
        public string AuthenticatedToken { get; set; } = string.Empty;
        public string IsTokenValid { get; set; } = string.Empty;
        public string TokenExpiryInSeconds { get; set; } = string.Empty;
        public string TokenExpireDateTime { get; set; }
        public string InsertDateTime { get; set; } 
        public string UpdateDateTime { get; set; }
        public string IsTouchIDSignIn { get; set; } = string.Empty;
        public string IsTokenAnonymous { get; set; } = string.Empty;
        public string PINPWDSecurityPwdUpdate { get; set; } = string.Empty;
        public string CustomerID { get; set; } = string.Empty;
        public string DataPowerAccessToken { get; set; } = string.Empty;
        public string ISVBQWMDisplayed { get; set; } = string.Empty;
        public string CustID { get; set; } = string.Empty;
        public string SystemDate { get; set; }
    }
}
