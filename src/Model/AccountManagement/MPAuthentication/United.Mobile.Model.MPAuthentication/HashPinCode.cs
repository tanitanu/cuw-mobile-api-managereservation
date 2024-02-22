using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.MPAuthentication
{
    public class HashPinCode
    {
        public string AccountNumber { get; set; }
        public string HashPinValue { get; set; }
        public string ApplicationId { get; set; }
        public string deviceId { get; set; }
        public string AppVersion { get; set; }
    }
}
