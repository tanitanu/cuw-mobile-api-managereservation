using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.ShopSeats;

namespace United.Mobile.Model.ShopSeats
{
    public class SeatInfo : Seat
    {
        public virtual string EDDInternalID { get; set; }
        public virtual string EDDTransactionID { get; set; }
    }
}
