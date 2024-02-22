using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.PassRiderList
{
    public class EmpStandByListPassenger
    {
        public string BoardDate { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool HasCheckedIn { get; set; }
        public string PassengerNumber { get; set; }
        public short NumberOfPassengers { get; set; }
        public string PassClass { get; set; }
        public string Cabin { get; set; }
        public string Position { get; set; }
        public string EmployeeId { get; set; }
        public string IsSpecial { get; set; }
        public string DisplayName { get; set; }
        public string CompanyName { get; set; }
    }
}
