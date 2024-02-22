using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBEmpPassRider 
    {
        
        private MOBEmpRelationship empRelationshipObject;
        private MOBEmpName name;
        private string birthDate;        
        private string gender;        
        private int age = -1;        
        private bool unaccompaniedFirst = true;        
        private bool mustUseCurrentYearPasses = false;        
        private string dependantId;        
        private string firstBookingBuckets;
        private MOBEmpPassRiderExtended empPassRiderExtended;        
        private bool primaryFriend;        
        private MOBEmpTCDInfo empTCDInfo;

        public MOBEmpPassRiderExtended EmpPassRiderExtended
        {
            get { return this.empPassRiderExtended; }
            set { this.empPassRiderExtended = value; }

        }

        public MOBEmpRelationship EmpRelationshipObject
        {
            get { return this.empRelationshipObject; }
            set { this.empRelationshipObject = value; }
        }

        public MOBEmpName Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public string BirthDate
        {
            get { return this.birthDate; }
            set { this.birthDate = value; }
        }

        public string Gender
        {
            get { return this.gender; }
            set { this.gender = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public int Age
        {
            get { return this.age; }
            set { this.age = value; }
        }

        public bool UnaccompaniedFirst
        {
            get { return this.unaccompaniedFirst; }
            set { this.unaccompaniedFirst = value; }
        }

        public bool MustUseCurrentYearPasses
        {
            get { return this.mustUseCurrentYearPasses; }
            set { this.mustUseCurrentYearPasses = value; }
        }

        public string DependantID
        {
            get { return this.dependantId; }
            set { this.dependantId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string FirstBookingBuckets
        {
            get { return this.firstBookingBuckets; }
            set { this.firstBookingBuckets = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public bool PrimaryFriend
        {
            get { return this.primaryFriend; }
            set { this.primaryFriend = value; }
        }
        public MOBEmpTCDInfo EmpTCDInfo
        {
            get { return this.empTCDInfo; }
            set { this.empTCDInfo = value; }
        }
    }
}
