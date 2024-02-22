using United.Mobile.Model.Common;

namespace United.Mobile.Model.BagCalculator
{
    public class MOBBaggageInformationRequest : MOBRequest
    {
        private string sessionId;
        private string flow;
        public string SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }
        public string Flow
        {
            get { return flow; }
            set { flow = value; }
        }
    }
}
