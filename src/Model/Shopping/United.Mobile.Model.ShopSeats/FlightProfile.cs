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

namespace United.Mobile.Model.ShopSeats
{
    public class FlightProfile
    {
        public virtual string CarrierCode { get; set; }
        public virtual string FlightNumber { get; set; }
        public virtual string DepartureAirport { get; set; }       
        public virtual string ArrivalAirport { get; set; }
        public virtual string DepartureDate { get; set; }
        public virtual string MarketingCarrierIndicator { get; set; }
        public virtual string OperatingCarrierCode { get; set; }
        public virtual string IsInternational { get; set; }
        //public Collection<Link> Links { get; set; }
        
    }
}
