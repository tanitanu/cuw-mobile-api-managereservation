using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Internal.AccountManagement
{
    [Serializable()]
    public class EmpployeeTravelTypeResponse : IPersist
    {
        public string ObjectName { get; set; }
        public MOBEmpTravelTypeResponse EmpTravelTypeResponse { get; set; }
    }
}
