using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class ShopSelectResponse : MOBResponse
    {
        public string SessionId { get; set; } = string.Empty;
        public string CartId { get; set; } = string.Empty;
        public ShopAvailability Availability { get; set; } 
    }
}
