using System;

namespace United.Mobile.Model.Internal.Common
{
    public class Session
    {
        public string ObjectName { get; set; }        
        public string TokenId { get; set; }
        public string SessionId { get; set; }
        public string MpNumber { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeProfleEmail { get; set; }
        public string EResTransactionId { get; set; }
        public string EResSessionId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastSavedTime { get; set; }
        public bool IsTokenExpired { get; set; }
        public bool IsPayrollDeduct { get; set; }
        public string LastJARefresh { get; set; }
        public DateTime TokenExpireDateTime { get; set; }
        public double TokenExpirationValueInSeconds { get; set; }
        public string CartId { get; set; }
        public string DeviceID { get; set; }
        public int AppID { get; set; }
        public string Token { get; set; }
    }
}
