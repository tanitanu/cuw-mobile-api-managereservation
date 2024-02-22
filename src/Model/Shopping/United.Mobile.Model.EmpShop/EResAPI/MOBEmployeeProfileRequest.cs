using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace United.Mobile.Model.Common
{
  
    public class MOBEmployeeProfileRequest
    {

            public string EmployeeId { get; set; }
            public bool IsPassRiderLoggedIn { get; set; }
            public bool IsLogOn { get; set; }
            public string PassRiderLoggedInID { get; set; }
            public string PassRiderLoggedInUser { get; set; }

    }
}
