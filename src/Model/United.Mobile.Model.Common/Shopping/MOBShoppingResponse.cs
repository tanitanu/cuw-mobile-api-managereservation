namespace United.Mobile.Model.Common
{
    public class MOBShoppingResponse : MOBResponse
    {
        private string sessionId = string.Empty;
        private string checkinSessionId = string.Empty;
        private string flow = string.Empty;

        public string SessionId
        {
            get
            {
                return this.sessionId;
            }
            set
            {
                this.sessionId = value;
            }
        }
        public string CheckinSessionId
        {
            get
            {
                return this.checkinSessionId;
            }
            set
            {
                this.checkinSessionId = value;
            }
        }

        public string Flow
        {
            get
            {
                return this.flow;
            }
            set
            {
                this.flow = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
    }
}
