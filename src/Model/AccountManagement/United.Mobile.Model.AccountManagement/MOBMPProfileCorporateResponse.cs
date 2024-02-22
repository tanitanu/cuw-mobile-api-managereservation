using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.MPSignIn
{
    [Serializable()]
    public class MOBMPProfileCorporateResponse : MOBResponse
    {
        public MOBCorporateTravelType CorporateEligibleTravelType { get; set; }

        public MOBCPCustomerMetrics CustomerMetrics { get; set; }
        public string EmployeeId { get; set; }
        public MOBSHOPResponseStatusItem ResponseStatusItem { get; set; }
        public bool IsYoungAdult { get; set; }
        public MOBCPProfile mpSecurityUpdateDetails { get; set; }
    }
}
