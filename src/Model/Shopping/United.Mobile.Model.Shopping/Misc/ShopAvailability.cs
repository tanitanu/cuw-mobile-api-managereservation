using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class ShopAvailability
    {
        public string SessionId { get; set; } = string.Empty;
        public string CartId { get; set; } = string.Empty;
        public ShopPricesCommon PriceSummary { get; set; } 
        public List<ShopTrip> Trips { get; set; } 
        public ResReservation Reservation { get; set; } 
        public ShopAvailability()
        {
            Trips = new List<ShopTrip>();
        }
    }
}
