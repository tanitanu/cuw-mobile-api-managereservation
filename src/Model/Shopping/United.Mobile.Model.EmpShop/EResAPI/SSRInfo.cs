using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Common
{
    public class SSRInfo
    {
        public int ID { get; set; }

        public string EmployeeID { get; set; }

        public string Description { get; set; }

        public string PaxID { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string NameSuffix { get; set; }

        public string BirthDate { get; set; }

        public string KnownTraveler { get; set; }

        public string Redress { get; set; }

        public string Gender { get; set; }

        public bool IsDefault { get; set; }

        public string PassportIssuedCountry { get; set; }

        public MOBEmpCountry Country { get; set; }

        public TravelDocument TravelDocument { get; set; }

        public string State { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
