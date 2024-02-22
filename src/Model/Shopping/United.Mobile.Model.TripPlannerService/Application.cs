using System;
using System.Runtime.Serialization;

namespace United.Mobile.Model.TripPlannerService
{
    //[Serializable()]
    [DataContract]
    public class Application
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public bool IsProduction { get; set; }
        [DataMember]
        public Version Version { get; set; }
    }
}