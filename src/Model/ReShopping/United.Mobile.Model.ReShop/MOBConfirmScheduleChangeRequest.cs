using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.ReShop
{
    [Serializable()]
    public class MOBConfirmScheduleChangeRequest : MOBRequest
    {
        private string sessionId = string.Empty;
        private string recordLocator;
        private string lastName = string.Empty;
        private string selectedOption;
        private string token;
        private string mileagePlusNumber;
        private string hashKey;
        private string flowType;

        public string SessionId { get { return sessionId; } set { sessionId = value; } }
        public string RecordLocator { get { return recordLocator; } set { recordLocator = value; } }
        public string LastName { get { return lastName; } set { lastName = value; } }
        public string SelectedOption { get { return selectedOption; } set { selectedOption = value; } }
        public string Token { get { return token; } set { token = value; } }
        public string MileagePlusNumber { get { return mileagePlusNumber; } set { mileagePlusNumber = value; } }
        public string HashKey { get { return hashKey; } set { hashKey = value; } }
        public string FlowType { get { return flowType; } set { flowType = value; } }
    }
}




