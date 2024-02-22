using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBComLoyaltyProgramProfile
    {
        private string carrierCode = string.Empty;
        private string programId = string.Empty;
        private string memberId = string.Empty;
        private string memberTierDescription = string.Empty;
        private string memberTierLevel = string.Empty;
        private string starEliteLevel = string.Empty;

        public string CarrierCode
        {
            get
            {
                return this.carrierCode;
            }
            set
            {
                this.carrierCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ProgramId
        {
            get
            {
                return this.programId;
            }
            set
            {
                this.programId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string MemberId
        {
            get
            {
                return this.memberId;
            }
            set
            {
                this.memberId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string MemberTierDescription
        {
            get
            {
                return this.memberTierDescription;
            }
            set
            {
                this.memberTierDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string MemberTierLevel
        {
            get
            {
                return this.memberTierLevel;
            }
            set
            {
                this.memberTierLevel = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string StarEliteLevel
        {
            get
            {
                return this.starEliteLevel;
            }
            set
            {
                this.starEliteLevel = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
