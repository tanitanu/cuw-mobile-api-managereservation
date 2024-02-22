using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBEmpTravelTypeAndJAProfileRequest : MOBEmpRequest
    {
        //string transactionId, string deviceId, string mpNumber, string employeeID
        private string mpNumber;
        private string employeeID;

        public string MPNumber
        {
            get
            {
                return this.mpNumber;
            }
            set
            {
                this.mpNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string EmployeeID
        {
            get
            {
                return this.employeeID;
            }
            set
            {
                this.employeeID = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
