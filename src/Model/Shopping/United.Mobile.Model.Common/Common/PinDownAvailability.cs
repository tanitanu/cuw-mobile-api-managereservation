using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class PinDownAvailability
    {
        [XmlAttribute]
        public string Cabin { get; set; } = string.Empty;

        [XmlAttribute]
        public string VendorName { get; set; } = string.Empty;

        [XmlAttribute]
        public string VendorGUID { get; set; } = string.Empty;

        [XmlAttribute]
        public string SearchBy { get; set; } = string.Empty;

        [XmlAttribute]
        public string CountryCode { get; set; } = string.Empty;

        public List<PinDownTrip> Trips { get; set; }

        public List<PinDownPTC> PTCs { get; set; }

        [XmlAttribute]
        public string SearchType { get; set; } = string.Empty;
        public PinDownAvailability()
        {
            Trips = new List<PinDownTrip>();
            PTCs = new List<PinDownPTC>();
        }
    }
}
