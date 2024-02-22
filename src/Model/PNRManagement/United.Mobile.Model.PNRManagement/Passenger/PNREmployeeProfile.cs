using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class PNREmployeeProfile
    {
        private string employeeID;
        private string passClassification;
        private string companySeniorityDate;
        public string EmployeeID
        {
            get { return employeeID; }
            set { employeeID = value; }
        }
        public string PassClassification
        {
            get { return passClassification; }
            set { passClassification = value; }
        }
        public string CompanySeniorityDate
        {
            get { return companySeniorityDate; }
            set { companySeniorityDate = value; }
        }
    }
}
