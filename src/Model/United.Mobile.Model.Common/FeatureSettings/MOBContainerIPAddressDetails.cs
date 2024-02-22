using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common.FeatureSettings
{
    public class MOBContainerIPAddressDetails
    {
        private string ipAddress;

        public string IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }
        private string serviceName;

        public string ServiceName
        {
            get { return serviceName; }
            set { serviceName = value; }
        }
        private bool isManuallyRefreshed;

        public bool IsManuallyRefreshed
        {
            get { return isManuallyRefreshed; }
            set { isManuallyRefreshed = value; }
        }

        private bool status;

        public bool Status
        {
            get { return status; }
            set { status = value; }
        }
        private string exception;

        public string Exception
        {
            get { return exception; }
            set { exception = value; }
        }


    }
}
