using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.EmployeeProfile
{
   public class EmployeePassSummary : EmployeePassBalanceBase
    {
        public string TravelerTypeDescription { get; set; }
        public IEnumerable<EmployeePassDetail> EmployeePassDetails { get; set; }
    }
    public class EmployeePassBalanceBase
    {
        public string PassBalanceText { get; set; }
        public string PassExpirationText { get; set; }
    }

}
