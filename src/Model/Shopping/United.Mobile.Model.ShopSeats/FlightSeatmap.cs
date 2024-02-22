using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using United.Mobile.Model.ShopSeats;

namespace United.Mobile.Model.ShopSeats
{
    public class FlightSeatmap
    {
        public virtual FlightProfile FlightInfo { get; set; }                
        public virtual Collection<Seatmap> SegmentSeatMap { get; set; }
       // public Collection<Link> Links { get; set; }
        
    }
}
