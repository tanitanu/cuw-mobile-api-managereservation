using System;
namespace United.Mobile.Model.Internal.EmployeeProfile
{
    public class EPassDetailAllotment
    {
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int Flown { get; set; }
        public int Pending { get; set; }
        public int Remaining { get; set; }
        public string ProgramYear { get; set; } = null;
        public int TotalCount { get; set; }
    }
}
