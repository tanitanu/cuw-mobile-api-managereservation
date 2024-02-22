using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    public class CustomerAuthorization
    {
        public string MileagePlusNumber { get; set; }
        public string CustomerId { get; set; }
        public string EmployeeId { get; set; }
        public string DeviceId { get; set; }
        public string ApplicationId { get; set; }
        public string VersionNumber { get; set; }
        public string CreationDateTimeUtc { get; set; }
        public string HashSalt { get; set; }
        public string Hash { get; set; }
        public bool IsTokenValid { get; set; }
        public int TokenExpiryInSeconds { get; set; }
        public string TokenExpireDateTime { get; set; }
        public bool IsTouchIDSignIn { get; set; }
        public string DataPowerAccessToken { get; set; }
        public string UpdateDateTimeUtc { get; set; }

    }
}
