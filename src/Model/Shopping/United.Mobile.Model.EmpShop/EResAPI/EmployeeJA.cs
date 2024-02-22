using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace United.Mobile.Model.Common
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

        public bool isTNCAccepted { get; set; }

        public bool AllowAdvanceBooking { get; set; }

        public List<Role> Role { get; set; }
    }
}
