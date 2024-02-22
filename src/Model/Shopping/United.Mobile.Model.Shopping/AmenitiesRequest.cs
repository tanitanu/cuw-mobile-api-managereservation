using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class AmenitiesRequest : MOBRequest
    {

        public string SessionId { get; set; } = string.Empty;
      
        public string CartId { get; set; } = string.Empty;
        
        public int LastTripIndexRequested { get; set; }
        public string ObjectName { get; set; }

        public AmenitiesRequest()
            : base()
        {
        }
    }
}

