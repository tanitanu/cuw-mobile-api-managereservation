using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common.FeatureSettings
{
    [Serializable()]
    public class MOBFeatureSettingsRequest:MOBRequest
    {
        private string apiName;

        public string ApiName
        {
            get { return apiName; }
            set { apiName = value; }
        }

    }
}
