using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{

    public class MOBEmployeeProfileResponse
    {
        public bool IsAllowedToSelectFlight { get; set; }

        public EmployeeJA EmployeeJA { get; set; }

        public EmployeeJA LoggedInJA { get; set; }

        public bool AllowImpersonation { get; set; }

        public string ImpersonateType { get; set; }

        public PassRiderExtended LoggedInPassRider { get; set; }

        public bool FlightWatchOptinStatus { get; set; }

        public List<int> FlightwatchEmailDays { get; set; }

        public List<BaseAlert> ProfileVerbiage { get; set; }

        public List<TravelDocument> TravelDocuments { get; set; }

        public Dictionary<int, string> EmailNotificationDays { get; set; }

        public bool IsMileagePlusEligible { get; set; }

        public bool IsSidaBadgeEligible { get; set; }

        public List<EPassSumaryAllotment> EpassSummaryDetails { get; set; }

        public MileagePlusRequest MileagePlus { get; set; }

        public List<EPassBalance> EPassAccountBalances { get; set; }

        public bool ShowSelectPrimaryFriend { get; set; }

        public string BoardDate { get; set; }

        public EmployeeAddress AddressVerification { get; set; }

        public ErrorInfo Error { get; set; }

        public string LastCallDateTime { get; set; }

        public string ServerName { get; set; }

        public string Status { get; set; }

        public string TransactionID { get; set; }

        public BaseAlert BaseAlert { get; set; }

        public string TransferMessage { get; set; }
    }

}
