using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBContact
    {
        private List<MOBEmail> emails;
        private List<MOBCPPhone> phoneNumbers;

        public List<MOBEmail> Emails
        {
            get { return this.emails; }
            set { this.emails = value; }
        }
        public List<MOBCPPhone> PhoneNumbers
        {
            get { return this.phoneNumbers; }
            set { this.phoneNumbers = value; }
        }
    }
}
