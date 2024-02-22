using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBEmpTCDInfo
    {
        private string phoneNumber;
        private string email;
        private bool newsAnnouncements;
        private string dialCode;
        private string countryCode;

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public bool NewsAnnouncements
        {
            get { return newsAnnouncements; }
            set { newsAnnouncements = value; }
        }

        public string DialCode
        {
            get { return dialCode; }
            set { dialCode = value; }
        }

        public string CountryCode
        {
            get { return countryCode; }
            set { countryCode = value; }
        }
    }
}
