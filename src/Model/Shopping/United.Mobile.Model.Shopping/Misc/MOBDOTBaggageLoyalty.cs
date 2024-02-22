using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class DOTBaggageLoyalty
    {
        private string programId = string.Empty;
        private string memberShipId = string.Empty;
        private string loyalLevel;

        public string ProgramId
        {
            get { return this.programId; }
            set { this.programId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string MemberShipId
        {
            get { return this.memberShipId; }
            set { this.memberShipId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string LoyalLevel
        {
            get { return this.loyalLevel; }
            set { this.loyalLevel = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

    }
}
