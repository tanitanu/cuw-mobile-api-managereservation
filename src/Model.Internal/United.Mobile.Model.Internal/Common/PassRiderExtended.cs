namespace United.Mobile.Model.Internal.Common
{
    public class PassRiderExtended
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string NameSuffix { get; set; } = string.Empty;
        public string StatusText { get; set; }        
        public bool Status { get; set; }         
        public bool VacationPermitted { get; set; }       
        public bool PayrollDeductPermitted { get; set; }
        public bool ViewTravelPlan { get; set; }
        public bool ViewEPassDetail { get; set; }
        public DayOfContactInformation DayOfContactInformation { get; set; }
        public string Email { get; set; }         
        public string Fax { get; set; } 
        public string Phone { get; set; }
        public string DependantId { get; set; }
        public string EmployeeId { get; set; }        
        public int UserProfileId { get; set; }
        public RelationShip Relationship { get; set; }
        public bool IsPrimaryFriend { get; set; }
    }
}
