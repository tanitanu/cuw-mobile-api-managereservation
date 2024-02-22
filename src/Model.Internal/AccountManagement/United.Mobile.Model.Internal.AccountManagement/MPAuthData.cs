using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Internal.AccountManagement
{
    public class MPAuthData
    {
        public string AccountNumber { get; set; } = string.Empty;
        public string DeviceID { get; set; } = string.Empty;
        public int ApplicationID { get; set; }
        public string AppVersion { get; set; }
    }
}
