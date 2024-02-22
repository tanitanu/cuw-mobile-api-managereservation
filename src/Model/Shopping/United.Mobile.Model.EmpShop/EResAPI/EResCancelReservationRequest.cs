using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    public class EResCancelReservationRequest
    {
        public string TravelPlan { get; set; }
        public string SegmentNumber { get; set; }
        public string PassClass { get; set; }
        public string EmployeeID { get; set; }
        public string TransactionId { get; set; }
    }
}
