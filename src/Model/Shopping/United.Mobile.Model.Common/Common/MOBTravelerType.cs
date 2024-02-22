using System;
using System.Xml.Serialization;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBTravelerType
    {
        public int Count { get; set; }
        public string TravelerType { get; set; } = string.Empty;
    }

    [Serializable()]
    [XmlRoot("MOBDisplayTravelType")]
    public class DisplayTravelType
    {
        public int PaxID { get; set; }
        public string PaxType { get; set; } = string.Empty;
        public string TravelerDescription { get; set; } = string.Empty;
        public PAXTYPE TravelerType { get; set; }
    }

    public enum PAXTYPE
    {
        Adult,
        Child2To4,
        Child5To11,
        Child12To17,
        InfantLap,
        InfantSeat,
        Senior,
        Child12To14,
        Child15To17
    }
   
}