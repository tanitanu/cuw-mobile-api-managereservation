using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable]
    public class MOBPrefRewardProgram
    {
        private long customerId;
        private long profileId;
        private long preferenceId;
        private string key = string.Empty;
        private string programMemberId = string.Empty;
        private string sourceDescription = string.Empty;
        private string sourceCode = string.Empty;
        private string vendorCode = string.Empty;
        private string vendorDescription = string.Empty;
        private int programId;
        private string programCode = string.Empty;
        private string programDescription = string.Empty;
        private string programType = string.Empty;
        private bool isSelected;
        private bool isNew;
        private bool isValidNumber;
        private string pin = string.Empty;
        private string languageCode = string.Empty;

        public long CustomerId
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

        public long ProfileId
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

        public long PreferenceId
        {
            get
            {
                return this.preferenceId;
            }
            set
            {
                this.preferenceId = value;
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

        public string ProgramMemberId
        {
            get
            {
                return this.programMemberId;
            }
            set
            {
                this.programMemberId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string SourceDescription
        {
            get
            {
                return this.sourceDescription;
            }
            set
            {
                this.sourceDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string SourceCode
        {
            get
            {
                return this.sourceCode;
            }
            set
            {
                this.sourceCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string VendorCode
        {
            get
            {
                return this.vendorCode;
            }
            set
            {
                this.vendorCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string VendorDescription
        {
            get
            {
                return this.vendorDescription;
            }
            set
            {
                this.vendorDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public int ProgramId
        {
            get
            {
                return this.programId;
            }
            set
            {
                this.programId = value;
            }
        }

        public string ProgramCode
        {
            get
            {
                return this.programCode;
            }
            set
            {
                this.programCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string ProgramDescription
        {
            get
            {
                return this.programDescription;
            }
            set
            {
                this.programDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ProgramType
        {
            get
            {
                return this.programType;
            }
            set
            {
                this.programType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
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

        public bool IsValidNumber
        {
            get
            {
                return this.isValidNumber;
            }
            set
            {
                this.isValidNumber = value;
            }
        }
    }
}
