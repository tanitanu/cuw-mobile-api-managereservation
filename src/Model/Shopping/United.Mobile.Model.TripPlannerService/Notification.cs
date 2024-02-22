using System.Collections.Generic;
using System.Runtime.Serialization;

namespace United.Mobile.Model.TripPlannerService
{
    [DataContract]
    public class Notification
    {
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string SubType { get; set; }
        [DataMember]
        public Alert Alert { get; set; }
        [DataMember]
        public int Badge { get; set; }
        [DataMember]
        public string Sound { get; set; }
        [DataMember]
        public bool ContentAvaliable { get; set; }
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public string DestinationType { get; set; }
        [DataMember]
        public string ActionType { get; set; }
        [DataMember]
        public string ThreadId { get; set; }
        [DataMember]
        public IDictionary<string, string> Payload { get; set; } //= new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    }
}
