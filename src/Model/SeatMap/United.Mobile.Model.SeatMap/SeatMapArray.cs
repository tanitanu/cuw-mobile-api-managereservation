using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using United.Mobile.Model.ShopSeats;

namespace United.Mobile.Model.SeatMap
{
    [XmlRoot("ArrayOfMOBSeatMapCSL30")]
    public class SeatMapArray
    {
        [XmlElement("MOBSeatMapCSL30")]
        public List<MOBSeatMapCSL30> ArrayOfMOBSeatMapCSL30 { get; set; }

        public SeatMapArray()
        {
            ArrayOfMOBSeatMapCSL30 = new List<MOBSeatMapCSL30>();
        }
    }
}
