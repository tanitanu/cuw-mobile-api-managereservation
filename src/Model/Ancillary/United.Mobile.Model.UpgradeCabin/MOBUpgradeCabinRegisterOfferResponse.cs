using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.SeatMap;
using United.Mobile.Model.Shopping;

namespace United.Mobile.Model.UpgradeCabin
{

    [Serializable]
    public class MOBUpgradeCabinRegisterOfferResponse : MOBResponse
    {
        private string cartId;
        private string sessionId;
        private MOBUpgradeCabinEligibilityResponse upgradeEligibility;
        private List<MOBUpgradeOption> upgradeProducts;
        private List<MOBUpgradeCabinOptionContent> tnCs;
        private List<MOBUpgradeCabinAdvisory> cabinOptionMessages;
        private MOBUpgradeCabinPriceDetails priceDetails;
        private MOBShoppingCart shoppingCart;
        private string pKDispenserPublicKey;

        public string CartId { get { return this.cartId; } set { this.cartId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public string SessionId { get { return this.sessionId; } set { this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); } }
        public MOBUpgradeCabinEligibilityResponse UpgradeEligibility { get { return this.upgradeEligibility; } set { this.upgradeEligibility = value; } }
        public List<MOBUpgradeOption> UpgradeProducts { get { return this.upgradeProducts; } set { this.upgradeProducts = value; } }
        public List<MOBUpgradeCabinOptionContent> TnCs { get { return this.tnCs; } set { this.tnCs = value; } }
        public List<MOBUpgradeCabinAdvisory> CabinOptionMessages { get { return this.cabinOptionMessages; } set { this.cabinOptionMessages = value; } }
        public MOBUpgradeCabinPriceDetails PriceDetails { get { return this.priceDetails; } set { this.priceDetails = value; } }
        public MOBShoppingCart ShoppingCart { get { return this.shoppingCart; } set { this.shoppingCart = value; } }
        public string PKDispenserPublicKey { get { return this.pKDispenserPublicKey; } set { this.pKDispenserPublicKey = value; } }
    }
}
