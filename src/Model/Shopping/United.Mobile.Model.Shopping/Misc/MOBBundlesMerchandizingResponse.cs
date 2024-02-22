using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class BundlesMerchandizingResponse :MOBResponse
    {
        private BundlesMerchangdizingRequest request;
        private BundleInfo bundleInfo;

        public BundlesMerchangdizingRequest Request
        {
            get
            {
                return this.request;
            }
            set
            {
                this.request = value;
            }
        }
        public BundleInfo BundleInfo
        {
            get
            {
                return this.bundleInfo;
            }
            set
            {
                this.bundleInfo = value;
            }
        }
    }
}
