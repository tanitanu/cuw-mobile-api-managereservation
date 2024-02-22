using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.ShopSeats;

namespace United.ECommerce.SeatMap
{
    public class FlightSeatMapDetail : FlightSeatmap
    {
        public virtual Collection<Traveler> Travelers { get; set; }
    }  
}
