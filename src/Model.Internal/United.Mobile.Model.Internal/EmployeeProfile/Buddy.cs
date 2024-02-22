using United.Mobile.Model.Internal.Common;
namespace United.Mobile.Model.Internal.EmployeeProfile
{
    public class Buddy
    {
        public long ID { get; set; } = 0;
        public string OwnerCarrier { get; set; } = null;
        public string EmployeeId { get; set; } = null;
        public string FirstName { get; set; } = null;
        public string KnownTraveler { get; set; } = null;
        public string MiddleName { get; set; } = null;
        public string LastName { get; set; } = null;
        public string NameSuffix { get; set; } = null;
        public string Redress { get; set; } = null;
        public int Age { get; set; }
        public string BirthDate { get; set; } = null;
        public string Gender { get; set; } = null;
        public RelationShip Relationship { get; set; }
        public string DayOfPhone { get; set; } = null;
        public string DayOfEmail { get; set; } = null;
    }
}
