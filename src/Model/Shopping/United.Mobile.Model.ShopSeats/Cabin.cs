using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.ShopSeats
{
    public class Cabin
    {
        public virtual string Key { get; set; }  
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual string Status { get; set; } 
        public virtual string Layout { get; set; } 
        public virtual string IsUpperDeck { get; set; }
        public virtual int RowCount { get; set; } 
        public virtual int ColumnCount { get; set; } 
        public virtual int TotalSeats { get; set; }
        public virtual Collection<SeatRow> SeatRows { get; set; }

    }
}
