using System;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBEmpBuddy
    {
        
        private long buddyId;
        
        private string ownerCarrier;
        
        private string employeeId;
        
        private MOBEmpName name;
        
        private string redress;
        
        private string birthDate;
        
        private string gender;
        
        private string phone;
        
        private string email;
        public MOBEmpRelationship empRelationship;

        public long BuddyID
        {
            get { return this.buddyId; }
            set { this.buddyId = value; }
        }

        public string OwnerCarrier
        {
            get { return this.ownerCarrier; }
            set { this.ownerCarrier = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string EmployeeId
        {
            get { return this.employeeId; }
            set { this.employeeId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public MOBEmpName Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public string Redress
        {
            get { return this.redress; }
            set { this.redress = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string BirthDate
        {
            get { return this.birthDate; }
            set { this.birthDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string Gender
        {
            get { return this.gender; }
            set { this.gender = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string Phone
        {
            get { return this.phone; }
            set { this.phone = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string Email
        {
            get { return this.email; }
            set { this.email = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public MOBEmpRelationship EmpRelationship
        {
            get
            {
                return empRelationship;
            }
            set
            {
                empRelationship = value;
            }
        }
    }
}
