using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class UnitedClubMemberShipDetails
    {
        private string memberTypeCode = string.Empty;
        private string memberTypeDesc = string.Empty;
        private string effectiveDate = string.Empty;
        private string discontinueDate = string.Empty;

        private string companionMileagePlus = string.Empty;
        private string primaryOrCompanion = string.Empty;


        public string MemberTypeCode
        {
            get
            {
                return this.memberTypeCode;
            }
            set
            {
                this.memberTypeCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string MemberTypeDesc
        {
            get
            {
                return this.memberTypeDesc;
            }
            set
            {
                this.memberTypeDesc = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string EffectiveDate
        {
            get
            {
                return this.effectiveDate;
            }
            set
            {
                this.effectiveDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DiscontinueDate
        {
            get
            {
                return this.discontinueDate;
            }
            set
            {
                this.discontinueDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string CompanionMileagePlus
        {
            get
            {
                return this.companionMileagePlus;
            }
            set
            {
                this.companionMileagePlus = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string PrimaryOrCompanion
        {
            get
            {
                return this.primaryOrCompanion;
            }
            set
            {
                this.primaryOrCompanion = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

    }
}
