using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.SeatMap
{
    [Serializable()]
    public class MOBUpgradeCabinTypeDesc
    {
        private string upgradeType;
        private string upgradeTypeDesc;
        private string upgradeStatus;
        private string id;
        private string availableSeatCount;
        private string availableSeatMsg;
        private string segmentNumber;

        public string UpgradeType { get { return this.upgradeType; } set { this.upgradeType = value; } }
        public string UpgradeTypeDesc { get { return this.upgradeTypeDesc; } set { this.upgradeTypeDesc = value; } }
        public string UpgradeStatus { get { return this.upgradeStatus; } set { this.upgradeStatus = value; } }
        public string Id { get { return this.id; } set { this.id = value; } }
        public string AvailableSeatCount { get { return this.availableSeatCount; } set { this.availableSeatCount = value; } }
        public string AvailableSeatMsg { get { return this.availableSeatMsg; } set { this.availableSeatMsg = value; } }
        public string SegmentNumber { get { return this.segmentNumber; } set { this.segmentNumber = value; } }
    }
}
