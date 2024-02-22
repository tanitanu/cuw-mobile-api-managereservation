using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace United.Mobile.Model.TripPlannerService
{
    [DataContract]
    public class Recipient
    {
        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public IDictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        [DataMember]
        public ICollection<int> ReceivingDevices { get; set; }
        [DataMember]
        public string FeedbackToken { get; set; }
    }
}