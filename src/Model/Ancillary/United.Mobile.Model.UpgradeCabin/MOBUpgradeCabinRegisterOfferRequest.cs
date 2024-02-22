using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.UpgradeCabin
{
    [Serializable]
    public class MOBUpgradeCabinRegisterOfferRequest : MOBRequest
    {
        private string sessionId;
        private string hashPinCode;
        private string mileagePlusNumber;
        private string token;
        private string cartId;
        private string recordLocator;
        private List<MOBUpgradeOption> upgradeProducts;
        private string flowType;

        public string CartId
        { get { return this.cartId; } set { this.cartId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public string SessionId
        { get { return this.sessionId; } set { this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); } }
        public string HashPinCode
        { get { return this.hashPinCode; } set { this.hashPinCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public string MileagePlusNumber
        { get { return this.mileagePlusNumber; } set { this.mileagePlusNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); } }
        public string Token
        { get { return this.token; } set { this.token = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public List<MOBUpgradeOption> UpgradeProducts
        { get { return this.upgradeProducts; } set { this.upgradeProducts = value; } }
        public string FlowType { get { return this.flowType; } set { this.flowType = value; } }
        public string RecordLocator { get { return this.recordLocator; } set { this.recordLocator = value; } }

    }
}
