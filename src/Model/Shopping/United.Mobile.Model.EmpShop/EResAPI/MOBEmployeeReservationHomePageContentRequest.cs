using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace United.Mobile.Model.Common
{
    public class MOBEmployeeReservationHomePageContentRequest
    {
        public bool IsLogOn { get; set; }
        public bool LoadEmployeeOnly { get; set; }
        public bool IsPartialLoad { get; set; }
        public string TransactionId { get; set; }
        public string SecureToken { get; set; }
        public string BookingTravelType { get; set; }
        public string SearchType { get; set; }
        public bool IsChangeSegment { get; set; }
        public string EmployeeId { get; set; }
        public bool IsAgentToolLogOn { get; set; }
        public string BookingSessionId { get; set; }
        public bool IsPassRiderLoggedIn { get; set; }
        public string BoardDate { get; set; }
        public bool loadPassRider { get; set; }
        public string PassRiderLoggedInID { get; set; }
        public string PassRiderLoggedInUser { get; set; }
    }

}
