using System;

namespace United.Mobile.Model.ReShop
{
    [Serializable()]
    public class MOBConfirmScheduleChangeResponse : MOBResponse
    {
        private string sessionId = string.Empty;
        private string deviceId;
        private string selectedOption;
        private string recordLocator;
        private string lastName = string.Empty;
        private string mileagePlusNumber;
        private string flowType;
        private United.Mobile.Model.ManageRes.MOBPNRByRecordLocatorResponse pnrResponse;

        public string SessionId { get { return sessionId; } set { sessionId = value; } }
        public string DeviceId { get { return deviceId; } set { deviceId = value; } }
        public string SelectedOption { get { return selectedOption; } set { selectedOption = value; } }
        public string RecordLocator { get { return recordLocator; } set { recordLocator = value; } }
        public string LastName { get { return lastName; } set { lastName = value; } }
        public string MileagePlusNumber { get { return mileagePlusNumber; } set { mileagePlusNumber = value; } }
        public string FlowType { get { return flowType; } set { flowType = value; } }
        public United.Mobile.Model.ManageRes.MOBPNRByRecordLocatorResponse PNRResponse { get { return pnrResponse; } set { pnrResponse = value; } }
    }
}
