using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping.UpgradeCabin
{

    [Serializable()]
    public class UpgradeOption
    {
        private string upgradeType;
        private string upgradeStatus;
        private string cabinUpgradeTypeDesc;
        private string availableSeatCount;
        private string tripRefId;
        private string segmentRefId;
        private string id;
        private string doubleUpgradeTooltip;
        private List<UpgradeCabinAdvisory> messages;
        private List< MOBSHOPTax> taxes;
        private Boolean allowSelection;
        private List< UpgradeCabinTypeDesc> upgradeCabinTypes;
        private List< UpgradePriceOption> priceOption;


        public string UpgradeType { get { return this.upgradeType; } set { this.upgradeType = value; } }
        public string UpgradeStatus { get { return this.upgradeStatus; } set { this.upgradeStatus = value; } }
        public string CabinUpgradeTypeDesc { get { return this.cabinUpgradeTypeDesc; } set { this.cabinUpgradeTypeDesc = value; } }
        public string AvailableSeatCount { get { return this.availableSeatCount; } set { this.availableSeatCount = value; } }
        public string TripRefId { get { return this.tripRefId; } set { this.tripRefId = value; } }
        public string SegmentRefId { get { return this.segmentRefId; } set { this.segmentRefId = value; } }
        public string Id { get { return this.id; } set { this.id = value; } }
        public string DoubleUpgradeTooltip { get { return this.doubleUpgradeTooltip; } set { this.doubleUpgradeTooltip = value; } }
        public List< UpgradeCabinTypeDesc> UpgradeCabinTypes { get { return this.upgradeCabinTypes; } set { this.upgradeCabinTypes = value; } }
        public List< UpgradePriceOption> PriceOption { get { return this.priceOption; } set { this.priceOption = value; } }
        public Boolean AllowSelection { get { return this.allowSelection; } set { this.allowSelection = value; } }
        public List< MOBSHOPTax> Taxes { get { return this.taxes; } set { this.taxes = value; } }
        public List< UpgradeCabinAdvisory> Messages { get { return this.messages; } set { this.messages = value; } }

    }
}
