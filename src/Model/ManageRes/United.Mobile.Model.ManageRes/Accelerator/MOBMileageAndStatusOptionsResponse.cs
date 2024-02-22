using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.ManageRes
{
    [Serializable]
    public class MOBMileageAndStatusOptionsResponse : MOBResponse
    {
        private string sessionId;
        private MOBAccelerators accelerators;

        public string SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }

        public MOBAccelerators Accelerators
        {
            get { return accelerators; }
            set { accelerators = value; }
        }
    }
}
