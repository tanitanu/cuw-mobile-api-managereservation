using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class AcquirePayPalTokenResponse //:MOBResponse
    {
        
        public AcquirePayPalTokenRequest Request { get; set; }

        public string SessionId { get; set; } = string.Empty;
        
        public string TokenID { get; set; } = string.Empty;
        
    }
}
