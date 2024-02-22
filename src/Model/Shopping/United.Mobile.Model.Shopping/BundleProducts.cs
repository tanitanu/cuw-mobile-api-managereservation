using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    // Models created for Bundle MockData by prasad
    [Serializable]
   public class BundleProducts
    {
        private string bundleName;
        private string description;
        private string market;
        private string bundlePrise;
        private int bundleUniqueId;
        private string bundleCarrier;
        private string bundleId;

        public string BundleId
        {
            get { return bundleId; }
            set { bundleId = value; }
        }

        public string BundleName
        {
            get { return bundleName; }
            set { bundleName = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string Market
        {
            get { return market; }
            set { market = value; }
        }

        public string BundlePrise
        {
            get { return bundlePrise; }
            set { bundlePrise = value; }
        }

        public string BundleCarrier
        {
            get { return bundleCarrier; }
            set { bundleCarrier = value; }
        }

        public int BundleUniqueId
        {
            get { return bundleUniqueId; }
            set { bundleUniqueId = value; }
        }


    }
     

}
