using System;
namespace United.Mobile.Model.Internal.EmployeeProfile
{
    public class EpassBalanceSummary
    {
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int Count { get; set; } = 0;
        public string Status { get; set; } = string.Empty;
        public string TravelerType { get; set; } = string.Empty;
        public string TravelerTypeDescription { get; set; } = string.Empty;
        public int ProgramYear { get; set; }
    }
}
