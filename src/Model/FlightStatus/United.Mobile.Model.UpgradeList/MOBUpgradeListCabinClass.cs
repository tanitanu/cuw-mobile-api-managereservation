using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping;

namespace United.Mobile.Model.UpgradeList
{
    [Serializable]
    public class MOBUpgradeListCabinClass
    {
        private string cabinTypeDescription = string.Empty;
        private List<MOBTypeOption> cabinBookingStatus;
        private List<LDPassenger> upgraded;
        private List<LDPassenger> standby;

        public List<MOBTypeOption> CabinBookingStatus
        {
            get
            {
                return this.cabinBookingStatus;
            }
            set
            {
                this.cabinBookingStatus = value;
            }
        }

        public List<LDPassenger> Upgraded
        {
            get
            {
                return this.upgraded;
            }
            set
            {
                this.upgraded = value;
            }
        }

        public List<LDPassenger> Standby
        {
            get
            {
                return this.standby;
            }
            set
            {
                this.standby = value;
            }
        }

        public string CabinTypeDescription
        {
            get { return this.cabinTypeDescription; }
            set { this.cabinTypeDescription = value; }
        }
    }
}
