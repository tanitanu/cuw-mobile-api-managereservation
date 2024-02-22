using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common.FeatureSettings
{
    public class MOBGetAllContainerFeatureSettingsResponse:MOBResponse
    {
        private List<MOBContainerFeatureSettings> containerFeatureSettings;

        public List<MOBContainerFeatureSettings> ContainerFeatureSettings
        {
            get { return containerFeatureSettings; }
            set { containerFeatureSettings = value; }
        }

    }
    public class MOBContainerFeatureSettings
    {
        private string ipAddress;

        public string IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }
        private List<MOBFeatureSetting> featureSettings;

        public List<MOBFeatureSetting> FeatureSettings
        {
            get { return featureSettings; }
            set { featureSettings = value; }
        }
        private string exception;

        public string Exception
        {
            get { return exception; }
            set { exception = value; }
        }

    }

    public class GetFeatureSettingsResponse:MOBResponse
    {
        private List<MOBFeatureSetting> featureSettings;

        public List<MOBFeatureSetting> FeatureSettings
        {
            get { return featureSettings; }
            set { featureSettings = value; }
        }

    }
}
