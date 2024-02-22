using System;
using System.Runtime.Serialization;

namespace United.Mobile.Model.TripPlannerService
{
    [DataContract]
    public class Alert
    {
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Body { get; set; }
    }
}