using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class CarrierInfoResponse:MOBResponse
    {
        public CarrierInfoResponse()
            : base()
        {
        }

        private List<CarrierInfo> carriers;

        public List<CarrierInfo> Carriers
        {
            get
            {
                return this.carriers;
            }
            set
            {
                this.carriers = value;
            }
        }
    }
    
}
