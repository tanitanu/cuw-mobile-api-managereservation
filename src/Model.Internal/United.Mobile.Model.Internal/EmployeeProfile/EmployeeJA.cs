using System.Collections.Generic;
using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Internal.EmployeeProfile
{
    public class EmployeeJA
    {
        public Employee Employee { get; set; }
        public List<JAByAirline> Airlines { get; set; }
        public List<PassRider> PassRiders { get; set; }
        public List<PassRider> SuspendedPassRiders { get; set; }
        public List<Buddy> Buddies { get; set; }
        public List<EmployeeDelegate> Delegates { get; set; }
        public TaxVerbiage TaxVerbiage { get; set; }
        public bool IsTNCAccepted { get; set; }
        public bool AllowAdvanceBooking { get; set; }
        public List<Role> Role { get; set; }
    }
}
