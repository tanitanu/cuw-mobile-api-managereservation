using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class PinDownTrip
    {
        [XmlAttribute]
        public string Origin { get; set; } = string.Empty;
      
        [XmlAttribute]
        public string Destination { get; set; } = string.Empty;
       
        [XmlAttribute]
        public string DepartDate { get; set; } = string.Empty;
      
        public List<PinDownSegment> Segments { get; set; }
        public PinDownTrip()
        {
            Segments = new List<PinDownSegment>();
        }
    }
}
