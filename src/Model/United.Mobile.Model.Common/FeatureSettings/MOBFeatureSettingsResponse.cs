using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common.FeatureSettings
{
    [Serializable()]
    public class MOBFeatureSettingsResponse:MOBResponse
    {
        private List<MOBFeatureSetting> featureSettings;

        public List<MOBFeatureSetting> FeatureSettings
        {
            get { return featureSettings; }
            set { featureSettings = value; }
        }
      
    }
}
