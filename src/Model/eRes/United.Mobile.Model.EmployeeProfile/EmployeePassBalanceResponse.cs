using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.EmployeeProfile
{
    public class EmployeePassBalanceResponse 
    {
        public IEnumerable<EmployeePassSummary> EPassSummaryAlloatments { get; set; }
        public string PassBalanceHeader { get; set; }
    }
   
}
