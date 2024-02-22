using System;

namespace United.Mobile.Model.ManagRes
{
    [Serializable()]
    public class MOBConfirmScheduleChangeResponse: MOBResponse
    {
        private string sessionId = string.Empty;
        private string deviceId = string.Empty;
        private string selectedOption = string.Empty;        
        private string recordLocator = string.Empty;
        private string lastName = string.Empty; 
        private string mileagePlusNumber = string.Empty;        
        private string flowType = string.Empty;
        private MOBPNRByRecordLocatorResponse pnrResponse;

        public string SessionId { get { return sessionId; } set { sessionId = value; } }        
        public string DeviceId { get { return deviceId; } set { deviceId = value; } }
        public string SelectedOption { get { return selectedOption; } set { selectedOption = value; } }
        public string RecordLocator { get { return recordLocator; } set { recordLocator = value; } }
        public string LastName { get { return lastName; } set { lastName = value; } } 
        public string MileagePlusNumber { get { return mileagePlusNumber; } set { mileagePlusNumber = value; } }        
        public string FlowType { get { return flowType; } set { flowType = value; } }
        public MOBPNRByRecordLocatorResponse PNRResponse { get { return pnrResponse; } set { pnrResponse = value; } }
    }
}
