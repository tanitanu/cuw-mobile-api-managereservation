using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable]
    public class MOBPrefSpecialRequest
    {
        private long airPreferenceId;
        private long specialRequestId;
        private string key = string.Empty;
        private string languageCode = string.Empty;
        private string specialRequestCode = string.Empty;
        private string description = string.Empty;
        private long priority;
        private bool isNew;
        private bool isSelected;

        public long AirPreferenceId
        {
            get
            {
                return this.airPreferenceId;
            }
            set
            {
                this.airPreferenceId = value;
            }
        }

        public long SpecialRequestId
        {
            get
            {
                return this.specialRequestId;
            }
            set
            {
                this.specialRequestId = value;
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
                this.key = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
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

        public string SpecialRequestCode
        {
            get
            {
                return this.specialRequestCode;
            }
            set
            {
                this.specialRequestCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public long Priority
        {
            get
            {
                return this.priority;
            }
            set
            {
                this.priority = value;
            }
        }

        public bool IsNew
        {
            get
            {
                return this.isNew;
            }
            set
            {
                this.isNew = value;
            }
        }

        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                this.isSelected = value;
            }
        }
    }
}
