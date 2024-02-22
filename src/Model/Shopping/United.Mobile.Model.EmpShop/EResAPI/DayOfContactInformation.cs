using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    public class DayOfContactInformation
    {
        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public bool IsNewsAndAnnouncements { get; set; }

        public string DialCode { get; set; }

        public string CountryCode { get; set; }

        public string UpdateType { get; set; }

        public string DependantID { get; set; }
    }
}
