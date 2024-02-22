using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model
{
    [Serializable()]
    public class MOBCarrierInfoResponse: MOBResponse
    {
        public List<MOBCarrierInfo> Carriers { get; set; }

        public MOBCarrierInfoResponse()
        {
            Carriers = new List<MOBCarrierInfo>();
        }
    }
}
