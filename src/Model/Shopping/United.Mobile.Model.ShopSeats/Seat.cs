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
    public class Seat
    {
        public virtual string Identifier { get; set; }        
        public virtual Collection<Characteristic> Characteristics { get; set; }        
        public virtual string Description { get; set; }        
        public virtual string SeatClass { get; set; }        
        public virtual string SeatType { get; set; }
        public virtual ProductPrice Price { get; set; }
        public string RowSpan { get; set; }
        public string ColSpan { get; set; }
    }
}
