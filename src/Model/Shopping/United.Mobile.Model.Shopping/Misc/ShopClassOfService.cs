using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class ShopClassOfService
    {
        public string FareClass { get; set; } = string.Empty;
        public string SeatAvailable { get; set; } = string.Empty;
    }
}
