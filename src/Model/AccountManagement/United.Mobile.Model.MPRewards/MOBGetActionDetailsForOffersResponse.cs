using System;
using United.Mobile.Model;

namespace United.Mobile.Model.MPRewards
{
    [Serializable()]
    public class MOBGetActionDetailsForOffersResponse : MOBResponse
    {
        private string data;
        private string sessionId;
        private string viewName;

        public string ViewName
        {
            get { return viewName; }
            set { viewName = value; }
        }

        public string SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }


        public string Data
        {
            get { return data; }
            set { data = value; }
        }
    }
}
