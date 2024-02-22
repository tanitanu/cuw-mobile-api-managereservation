using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBBKProfileMember
    {
        private int customerId;
        private string key = string.Empty;
        private int profileId;
        private string languageCode = string.Empty;

        public int CustomerId
        {
            get
            {
                return this.customerId;
            }
            set
            {
                this.customerId = value;
            }
        }

        public string Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public int ProfileId
        {
            get
            {
                return this.profileId;
            }
            set
            {
                this.profileId = value;
            }
        }

        public string LanguageCode
        {
            get
            {
                return this.languageCode;
            }
            set
            {
                this.languageCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
