using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace United.Mobile.Model.Common
{
    public class Role
    {
        public int RoleId { get; set; }

        public string RoleDescription { get; set; }

        public string RoleAlias { get; set; }

        public bool IsActive { get; set; }

        public string EmployeeID { get; set; }

        public string EmployeeName { get; set; }

        public string Action { get; set; }

        public string LastUpdatedby { get; set; }

        public DateTime LastUpdatedDateTime { get; set; }

        public List<string> SelectedRoleId { get; set; }
    }
}
