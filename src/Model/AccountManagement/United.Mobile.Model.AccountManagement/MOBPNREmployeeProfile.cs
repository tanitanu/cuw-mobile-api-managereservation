using System;

namespace United.Mobile.Model.MPSignIn
{
    [Serializable]
    public class MOBPNREmployeeProfile
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
