using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.UpgradeCabin
{
    [Serializable()]
    public class MOBUpgradeCabinEligibilityRequest : MOBRequest
    {

        //Request defaults 
        private string recordLocator;
        private string lastName;
        private string mileagePlusNumber;
        private string sessionId;
        private string hashPinCode;
        private string token;
        private string flowType;

        public string RecordLocator
        { get { return this.recordLocator; } set { this.recordLocator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); } }
        public string LastName
        { get { return this.lastName; } set { this.lastName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); } }
        public string MileagePlusNumber
        { get { return this.mileagePlusNumber; } set { this.mileagePlusNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper(); } }
        public string SessionId
        { get { return this.sessionId; } set { this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public string HashPinCode
        { get { return this.hashPinCode; } set { this.hashPinCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public string Token
        { get { return this.token; } set { this.token = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public string FlowType
        { get { return this.flowType; } set { this.flowType = value; } }

    }
}
