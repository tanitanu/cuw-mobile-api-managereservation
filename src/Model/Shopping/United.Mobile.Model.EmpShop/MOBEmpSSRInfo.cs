using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBEmpSSRInfo
    {
        private MOBEmpName name;
        private string id;
        private string employeeID;
        private string description;
        private string paxID;
        private string birthDate;
        private string knownTraveler;
        private string redress;
        private string gender;
        private bool isDefault;

        public string ID
        {
            get { return id; }
            set { id = value; }
        }
        public string EmployeeID
        {
            get { return this.employeeID; }
            set { this.employeeID = value; }
        }
        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }
        public string PaxID
        {
            get { return this.paxID; }
            set { this.paxID = value; }
        }
        public MOBEmpName Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        public string BirthDate
        {
            get { return birthDate; }
            set { birthDate = value; }
        }
        public string KnownTraveler
        {
            get { return knownTraveler; }
            set { knownTraveler = value; }
        }
        public string Redress
        {
            get { return redress; }
            set { redress = value; }
        }
        public string Gender
        {
            get { return gender; }
            set { gender = value; }
        }
        public bool IsDefault
        {
            get { return isDefault; }
            set { isDefault = value; }
        }
    }
}


