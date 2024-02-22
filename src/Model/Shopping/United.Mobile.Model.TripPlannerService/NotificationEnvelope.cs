using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace United.Mobile.Model.TripPlannerService
{
    [DataContract]
    public class NotificationEnvelope
    {
        [DataMember]
        public List<Recipient> Recipients { get; set; }
        [DataMember]
        public Notification Notification { get; set; }
    }
}
