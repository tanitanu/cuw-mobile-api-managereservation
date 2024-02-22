using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    public class MOBEmpJA
    {
        //private MOBEmployee employee;
        private List<MOBEmpJAByAirline> empJAByAirlines;
        private List<MOBEmpPassRider> empPassRiders;
        private List<MOBEmpBuddy> empBuddies;
        private List<Delegate> delegats;
        private MOBEmpRelationship empRelationshipObject;
        private List<MOBEmpPassRider> empSuspendedPassRiders;

        //public MOBEmployee MEmployee
        //{
        //    get { return this.employee; }
        //    set { this.employee = value; }
        //}

        public List<MOBEmpJAByAirline> EmpJAByAirlines
        {
            get { return this.empJAByAirlines; }
            set { this.empJAByAirlines = value; }
        }

        public List<MOBEmpPassRider> EmpPassRiders
        {
            get { return this.empPassRiders; }
            set { this.empPassRiders = value; }
        }

        public List<MOBEmpPassRider> EmpSuspendedPassRiders
        {
            get { return this.empSuspendedPassRiders; }
            set { this.empSuspendedPassRiders = value; }
        }

        public List<MOBEmpBuddy> EmpBuddies
        {
            get { return this.empBuddies; }
            set { this.empBuddies = value; }
        }

        public List<Delegate> Delegates
        {
            get { return this.delegats; }
            set { this.delegats = value; }
        }

        public MOBEmpRelationship EmpRelationshipObject
        {
            get
            {
                return this.empRelationshipObject;
            }
            set
            {
                this.empRelationshipObject = value;
            }
        }
    }


    public class GetEmpIdByMpNumber
    {
        public string MPNumber { get; set; }
        public string EmployeeId { get; set; }
        public string ContinentalEmployeeId { get; set; }
        public object MPFirstName { get; set; }
        public object MPLastName { get; set; }
        public object MPLinkedDate { get; set; }
        public object MPLinkedBy { get; set; }
        public string MPLinkedId { get; set; }
        public string FileNumber { get; set; }
    }

}
