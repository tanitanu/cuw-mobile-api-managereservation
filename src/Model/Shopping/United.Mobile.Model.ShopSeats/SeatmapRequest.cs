using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace United.Mobile.Model.ShopSeats
{
    [DataContract]
    public class SeatmapRequest
    {
        [DataMember(EmitDefaultValue = false)]
        public List<SeatRule> RuleInfos { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<Traveler> Travelers { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string RecordLocator { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string PNRCreateDate { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public int SegmentNumber { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public int FlightNumber { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string FlightDateTime { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string DepartureAirport { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string ArrivalAirport { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string MarketingCarrier { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string OperatingCarrier { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string CabinType { get; set; }

        //public List<SeatType> SeatTypes { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string TripleA { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string DutyCode { get; set; }

    }
}
