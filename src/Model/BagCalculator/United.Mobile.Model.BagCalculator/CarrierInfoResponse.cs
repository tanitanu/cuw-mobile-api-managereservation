using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.BagCalculator
{
    [Serializable()]
    public class CarrierInfoResponse
    {
        public List<CarrierInfo> Carriers { get; set; }

        public CarrierInfoResponse()
        {
            Carriers = new List<CarrierInfo>();
        }
    }
}
