using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class PinDownSegment
    {
      
        [XmlAttribute]
        public int FlightNumber { get; set; } 
       
        [XmlAttribute]
        public string Origin { get; set; } = string.Empty;
        
        [XmlAttribute]
        public string Destination { get; set; } = string.Empty;
       
        [XmlAttribute]
        public string DepartDate { get; set; } = string.Empty;
       
        [XmlAttribute]
        public string DestinationDate { get; set; } = string.Empty;
       
        [XmlAttribute]
        public string Carrier { get; set; } = string.Empty;
       
    }
}
