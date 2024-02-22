using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class AirRewardProgram
    {
        private string customerId = string.Empty;
        private string profileId = string.Empty;
        private string programCode = string.Empty;
        private string programDescription = string.Empty;
        private string vendorCode = string.Empty;
        private string vendorDescription = string.Empty;
        private string programMemberId = string.Empty;
        private int eliteLevel = 0;
        private string eliteLevelDescription = string.Empty;

        public string CustomerId
        {
            get
            {
                return this.customerId;
            }
            set
            {
                this.customerId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ProfileId
        {
            get
            {
                return this.profileId;
            }
            set
            {
                this.profileId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public string EliteLevelDescription
        {
            get
            {
                return this.eliteLevelDescription;
            }
            set
            {
                this.eliteLevelDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
