using Newtonsoft.Json.Converters;

namespace United.Mobile.Model.Internal.Common
{
    public class SSRInfo
    {
        public int ID { get; set; }
        public string EmployeeID { get; set; }
        public string Description { get; set; } = string.Empty;
        public string PaxID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string NameSuffix { get; set; } = string.Empty;
        public string BirthDate { get; set; } = string.Empty; 
        public string KnownTraveler { get; set; } = string.Empty;
        public string Redress { get; set; } = string.Empty;
        public string Gender { get; set; }
        public bool IsDefault { get; set; }
        public string PassportIssuedCountry { get; set; } = string.Empty;
        public Country Country { get; set; }
        public TravelDocument TravelDocument { get; set; }
        public string State { get; set; }
        public string DateCreated { get; set; }
    }
}
