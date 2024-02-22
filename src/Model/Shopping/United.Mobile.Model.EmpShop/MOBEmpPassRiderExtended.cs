using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBEmpPassRiderExtended
    {
        MOBEmpName name;
        string userName;        
        string vacationPermitted;        
        string email;        
        string fax;        
        string phone;

        public MOBEmpName Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        public string UserName
        {
            get { return this.userName; }
            set { this.userName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string VacationPermitted
        {
            get { return this.vacationPermitted; }
            set { this.vacationPermitted = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string Email
        {
            get { return this.email; }
            set { this.email = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string Fax
        {
            get { return this.fax; }
            set { this.fax = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string Phone
        {
            get { return this.phone; }
            set { this.phone = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
    }
}
