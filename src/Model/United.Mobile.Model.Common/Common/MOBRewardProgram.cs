using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBRewardProgram
    {
        private string starEliteStatus = string.Empty;
        private int eliteLevel;
        private string programCode = string.Empty;
        private string programDescription = string.Empty;
        private string programType = string.Empty;
        private string otherVendorCodeStatus = string.Empty;
        private string vendorCode = string.Empty;
        private string programMemberId = string.Empty;
        private string vendorDescription = string.Empty;

        public string StarEliteStatus
        {
            get
            {
                return this.starEliteStatus;
            }
            set
            {
                this.starEliteStatus = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public int EliteLevel
        {
            get
            {
                return this.eliteLevel;
            }
            set
            {
                this.eliteLevel = value;
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
                this.programCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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
                this.programType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string OtherVendorCodeStatus
        {
            get
            {
                return this.otherVendorCodeStatus;
            }
            set
            {
                this.otherVendorCodeStatus = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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
                this.vendorCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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
                this.programMemberId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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
    }
}
