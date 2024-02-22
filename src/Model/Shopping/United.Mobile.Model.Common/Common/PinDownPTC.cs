using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class PinDownPTC
    {
        [XmlAttribute]
        public string Type { get; set; } = string.Empty;
      
        [XmlAttribute]
        public int Count { get; set; } 
    }
}
