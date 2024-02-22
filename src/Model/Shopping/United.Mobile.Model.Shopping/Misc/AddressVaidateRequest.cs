using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.Misc
{
    public class AddressVaidateRequest : MOBRequest
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string StateCode { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
    }
    public class AddressResponse
    {
        public bool IsValidAddress { get; set; }
    }
}