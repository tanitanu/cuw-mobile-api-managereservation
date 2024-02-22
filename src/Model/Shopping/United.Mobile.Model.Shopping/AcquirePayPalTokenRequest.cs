using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class AcquirePayPalTokenRequest //: Request
    {

        public string MileagePlusNumber { get; set; } = string.Empty;
        
        public string SessionId { get; set; } = string.Empty;

        public double Amount { get; set; } 

        public string CountryCode { get; set; } = string.Empty;

        public string CancelURL { get; set; } = string.Empty;

        public string ReturnURL { get; set; } = string.Empty;

        public string Key { get; set; } = string.Empty;

    }
}
