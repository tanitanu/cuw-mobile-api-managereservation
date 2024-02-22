using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class RegisterFareLockResponse : MOBResponse
    {
        public RegisterFareLockRequest Request { get; set; }

        public MOBSHOPAvailability Availability { get; set; }

        public List<string> Disclaimer { get; set; }

        public RegisterFareLockResponse()
        {
            Disclaimer = new List<string>();
        }
    }
}
