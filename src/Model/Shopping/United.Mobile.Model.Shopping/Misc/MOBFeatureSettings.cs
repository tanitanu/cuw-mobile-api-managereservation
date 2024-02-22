using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class FeatureSettings : MOBItem
    {
        private Application application;

        public Application Application
        {
            get
            {
                return this.application;
            }
            set
            {
                this.application = value;
            }
        }

    }
}
