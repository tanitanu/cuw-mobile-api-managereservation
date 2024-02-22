using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace United.Utility.Enum
{

        public enum MOBErrorCodes
        {
            [EnumMember(Value = "900111")]
            ViewResCFOPSessionExpire = 900111,      //Error code for session expire in View/Manage reservation flow implemented during Common FOP
            [EnumMember(Value = "900112")]
            ViewResCFOP_NullSession_AfterAppUpgradation = 900112,      //Error code for null session if the app get update during the process. Actual Message: "We're sorry, we are currently updating the mobile app. Please reload your reservation."
        };
    
}
