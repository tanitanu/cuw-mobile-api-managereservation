using System;
using System.Xml.Serialization;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    [XmlRoot("MOBSHOPTripShare")]
    public class TripShare : MOBResponse
    {
        public bool ShowShareTrip { get; set; }

        public string PlaceholderTitle { get; set; } 

        public string Url { get; set; } 

        public string CommonCaption { get; set; } 

        public SHOPTripShareMessage Email { get; set; }

    }

    [Serializable]
    [XmlRoot("MOBSHOPTripShareMessage")]
    public class SHOPTripShareMessage
    {
        public string Body { get; set; } = string.Empty;

        public string Subject { get; set; } = string.Empty;

    }
}
