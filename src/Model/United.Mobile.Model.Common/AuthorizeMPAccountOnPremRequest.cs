using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common
{
    public class AuthorizeMPAccountOnPremRequest
    {
        private string deviceId;
        private string hashValue;
        private string mpNumber;
        private string applicationId;
        private string appVersion;



        public string DeviceId
        {
            get { return deviceId; }
            set { deviceId = value; }
        }



        public string HashValue
        {
            get { return hashValue; }
            set { hashValue = value; }
        }



        public string MpNumber
        {
            get { return mpNumber; }
            set { mpNumber = value; }
        }



        public string ApplicationId
        {
            get { return applicationId; }
            set { applicationId = value; }
        }



        public string AppVersion
        {
            get { return appVersion; }
            set { appVersion = value; }
        }



    }
}
