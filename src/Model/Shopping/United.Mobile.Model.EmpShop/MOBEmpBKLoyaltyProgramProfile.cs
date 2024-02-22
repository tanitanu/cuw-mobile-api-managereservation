    using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBEmpBKLoyaltyProgramProfile
    {
        private string rewardProgramKey = string.Empty;
        private string carrierCode = string.Empty;
        private string programId = string.Empty;
        private string programName = string.Empty;
        private string memberId = string.Empty;
        private string memberTierDescription = string.Empty;
        private string memberTierLevel = string.Empty;
        private string starEliteLevel = string.Empty;
        private int mpEliteLevel = 0;

        public string RewardProgramKey
        {
            get
            {
                return this.rewardProgramKey;
            }
            set
            {
                this.rewardProgramKey = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

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

        public string ProgramName
        {
            get
            {
                return this.programName;
            }
            set
            {
                this.programName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public int MPEliteLevel
        {
            get
            {
                return this.mpEliteLevel;
            }
            set
            {
                this.mpEliteLevel = value;
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
