using United.Mobile.Model.Internal.Common;
namespace United.Mobile.Model.Internal.EmployeeProfile
{
    public class DependentInfo
    {
        public Name DependentName { get; set; }
        public string DependentId { get; set; }
        public string DateOfBirth { get; set; }
        public int Age { get; set; }
        public RelationShip Relationship { get; set; }
    }
}