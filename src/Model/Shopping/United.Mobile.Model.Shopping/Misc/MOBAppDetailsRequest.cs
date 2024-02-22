using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.Misc
{
    public class AppDetailsRequest: MOBRequest
    {
        private string appDetailsRequestID = string.Empty;
        private string appDetailsRequestVersion = string.Empty;

        public string AppDetailsRequestID
        {
            get
            {
                return appDetailsRequestID;
            }
            set
            {
                appDetailsRequestID = value;
            }
        }

        public string AppDetailsRequestVersion
        {
            get
            {
                return appDetailsRequestVersion;
            }
            set
            {
                appDetailsRequestVersion = value;
            }
        }

    }
}
