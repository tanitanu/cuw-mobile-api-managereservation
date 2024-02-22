using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.ManageRes
{
    [Serializable]
    public class MOBMileageAndStatusOptionsRequest : MOBRequest
    {
        private string sessionId;

        public string SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }

        private string correlationId;
        public string CorrelationId
        {
            get { return correlationId; }
            set { correlationId = value; }
        }

    }
}
