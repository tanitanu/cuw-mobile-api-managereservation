using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable]
    public class ShopFlattenedFlight
    {
        public List<ShopFlight> Flights { get; set; }
        public ShopFlattenedFlight()
        {
            Flights = new List<ShopFlight>();
        }
    }
}
