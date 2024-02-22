using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common.FeatureSettings
{
    public class MOBGetContainerIPAddressesByServiceResponse : MOBResponse
    {
        private List<MOBContainerIPAddressDetails> ipAddresses;

        public List<MOBContainerIPAddressDetails> IpAddresses
        {
            get { return ipAddresses; }
            set { ipAddresses = value; }
        }

    }
}
