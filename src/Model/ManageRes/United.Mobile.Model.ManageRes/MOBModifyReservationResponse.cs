using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.ManageRes
{
    public class MOBModifyReservationResponse : MOBResponse
    {
        private string sessionId = string.Empty;

        public string SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }
    }
}
