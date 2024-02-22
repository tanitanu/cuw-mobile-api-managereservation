using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Shopping;

namespace United.Mobile.Model.ShopSeats
{
    public class SeatRow
    {
        public virtual Collection<Characteristic> Characteristics { get; set; } 
        public virtual int RowNumber { get; set; }
        public virtual Collection<Seat> Seats { get; set; }
        public virtual string Status { get; set; }
        public virtual string Genre { get; set; }
        public virtual string CabinDescription { get; set; }

    }
}
