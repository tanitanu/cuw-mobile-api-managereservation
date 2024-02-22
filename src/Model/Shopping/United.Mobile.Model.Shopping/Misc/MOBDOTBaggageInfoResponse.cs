using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Shopping.Misc;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class DOTBaggageInfoResponse : MOBResponse
    {
        private DOTBaggageInfoRequest request;
        private DOTBaggageInfo dotBaggageInfo;

        public DOTBaggageInfoRequest Request
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
        public DOTBaggageInfo DotBaggageInfo
        {
            get
            {
                return this.dotBaggageInfo;
            }
            set
            {
                this.dotBaggageInfo = value;
            }
        }

    }
}
