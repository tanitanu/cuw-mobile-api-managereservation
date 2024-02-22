using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace United.Mobile.Model.Common
{
    public class EmployeeProfileRequest
    {
        public string EmployeeId { get; set; }
        public bool IsPassRiderLoggedIn { get; set; }
        public bool IsLogOn { get; set; }
        public string PassRiderLoggedInID { get; set; }
        public string PassRiderLoggedInUser { get; set; }
    }
}
    