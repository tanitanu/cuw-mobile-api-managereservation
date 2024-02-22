using System.Collections.Generic;
using United.Mobile.Model.Internal.Common;
using United.Mobile.Model.Internal.EmployeeProfile;

namespace United.Mobile.Model.Internal.PassBalance
{
    public class PassBalanceResponse : EResBaseResponse
    {
        public List<EPassSumaryAllotment> EPassSumaryAllotments { get; set; }
        public List<EPassBalance> EPassBalances { get; set; }
        public string BoardDate { get; set; }
    }
}
