using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping.UpgradeCabin
{
    [Serializable]
    public class UpgradeCabinRegisterOfferResponse :  MOBResponse
    {
        private string cartId;
        private string sessionId;
        private UpgradeCabinEligibilityResponse upgradeEligibility;
        private List< UpgradeOption> upgradeProducts;
        private List< UpgradeCabinOptionContent> tnCs;
        private List<UpgradeCabinAdvisory> cabinOptionMessages;
        private UpgradeCabinPriceDetails priceDetails;
        private  MOBShoppingCart shoppingCart;
        private string pKDispenserPublicKey;       

        public string CartId { get { return this.cartId; } set { this.cartId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public string SessionId { get { return this.sessionId; } set { this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); } }
        public UpgradeCabinEligibilityResponse UpgradeEligibility { get { return this.upgradeEligibility; } set { this.upgradeEligibility = value; } }
        public List< UpgradeOption> UpgradeProducts { get { return this.upgradeProducts; } set { this.upgradeProducts = value; } }
        public List< UpgradeCabinOptionContent> TnCs { get { return this.tnCs; } set { this.tnCs = value; } }
        public List<UpgradeCabinAdvisory> CabinOptionMessages { get { return this.cabinOptionMessages; } set { this.cabinOptionMessages = value; } }
        public UpgradeCabinPriceDetails PriceDetails { get { return this.priceDetails; } set { this.priceDetails = value; } }
        public  MOBShoppingCart ShoppingCart { get { return this.shoppingCart; } set { this.shoppingCart = value; } }
        public string PKDispenserPublicKey { get { return this.pKDispenserPublicKey; } set { this.pKDispenserPublicKey = value; } }
    }
}
