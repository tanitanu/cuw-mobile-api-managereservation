using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Internal.AccountManagement
{
    [Serializable()]
    public class EmpSession : IPersist
    {

        #region IPersist Members

        private string objectName = "United.Mobile.Model.Internal.AccountManagement.Session";
        public string ObjectName
        {
            get
            {
                return this.objectName;
            }
            set
            {
                this.objectName = value;
            }
        }

        #endregion
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
    }
}
