using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace United.Mobile.Model.ShopSeats
{
    [DataContract]
    public class SeatRule
    {
        [DataMember(EmitDefaultValue = false)]
        public int FlightNumber { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string FlightDateTime { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string DepartureAirport { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string ArrivalAirport { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string OperatingCarrier { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string CabinType { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string EliteStatus { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string StarEliteStatus { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string FQTVCarrier { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public bool IsChaseCardMember { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public bool IsDeadHeadCrew { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string PassClass { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string COS { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public bool hasSSR { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string LangCode { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public bool IsUnaccompaniedMinor { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public bool IsIrregularOps { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public int NumberOfPassengers { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Segment { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string AirCarrierType { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string DepartureDate { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string DepartureTime { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string ArrivalDate { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string ArrivalTime { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string FareBasisCode { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string FareCode { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public bool IsOxygen { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public bool IsInCabinPet { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public bool IsLapChild { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public bool IsServiceAnimal { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public bool CheckedInWindow { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public bool AwardTravel { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string AwardAccountEliteStatus { get; set; }
    }
}
