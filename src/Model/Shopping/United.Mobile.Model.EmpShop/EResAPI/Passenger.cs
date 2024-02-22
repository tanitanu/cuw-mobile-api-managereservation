using System;

namespace United.Mobile.Model.Common
{
    public class Passenger
    {        
        public string BoardYear { get; set; } 
        public string Cabin { get; set; } 
        public string CabinKey { get; set; }
        public string EmployeeID { get; set; } 
        public string FirstName { get; set; } 
        public bool HasCheckedIn { get; set; }
        public DateTime HireDate { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public short NumberOfPassengers { get; set; }
        public string PassClass { get; set; }
        public string PassengerId { get; set; }         
        public string Position { get; set; } 
        public int Priority { get; set; }
        public bool Special { get; set; } 
        public string UPLEnum { get; set; } 
        public string EmpLevel { get; set; } 
        public string CompanyName { get; set; } 
        public string RecordLocator { get; set; } 
        public bool ShowInFlightWatch { get; set; }
        public int SeniorityNo { get; set; }
    }
}
