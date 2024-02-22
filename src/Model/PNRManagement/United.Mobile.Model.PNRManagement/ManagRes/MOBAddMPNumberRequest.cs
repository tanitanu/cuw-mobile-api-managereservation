using System;

namespace United.Mobile.Model.ManagRes
{
    [Serializable()]
    public class MOBAddMPNumberRequest : MOBRequest
    {
        private string sessionId;
        private string recordLocator;
        private string loyaltyProgramCarrierCode;
        private string loyaltyProgramMemberID;
        private string key;
        private MOBName passengerName;

        public MOBName PassengerName
        {
            get
            {
                return this.passengerName;
            }
            set
            {
                this.passengerName = value;
            }
        }
        public string Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string LoyaltyProgramMemberID
        {
            get
            {
                return this.loyaltyProgramMemberID;
            }
            set
            {
                this.loyaltyProgramMemberID = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string LoyaltyProgramCarrierCode
        {
            get
            {
                return this.loyaltyProgramCarrierCode;
            }
            set
            {
                this.loyaltyProgramCarrierCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string SessionId
        {
            get
            {
                return this.sessionId;
            }
            set
            {
                this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string RecordLocator
        {
            get
            {
                return this.recordLocator;
            }
            set
            {
                this.recordLocator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

    }
}
