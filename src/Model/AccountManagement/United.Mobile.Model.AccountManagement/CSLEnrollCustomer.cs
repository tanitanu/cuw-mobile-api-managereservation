using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Common
{
    public class CSLEnrollCustomer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string MiddleName { get; set; }
        //public string Suffix { get; set; }
        public string Title { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        //public string Nationality { get; set; }
        public string CountryOfResidency { get; set; }
        public string EnrollmentSourceCode { get; set; }
        //public string UserId { get; set; }
        //public string Password { get; set; }
        public bool UseAddressValidation { get; set; }
        //public string LanguageCode { get; set; }
        public OneClickEnrollmentAddress Address { get; set; }
        public Phone Phone { get; set; }
        public MOBEmail Email { get; set; }        
        public List<CSLMarketingPreference> MarketingPreferences { get; set; }
        public List<CSLSecurityQuestion> SecurityQuestions { get; set; }
    }
}
