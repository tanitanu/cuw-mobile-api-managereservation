using System.Collections.Generic;
namespace United.Mobile.Model.Internal.EmployeeProfile
{
    public class EPassSumaryAllotment : EpassBalanceSummary
    {
        public ExpirationPassDetails ExpirationPassDetail { get; set; } = new ExpirationPassDetails();
        public List<EPassDetailAllotment> EPassDetailAllotments { get; set; }
    }
}
