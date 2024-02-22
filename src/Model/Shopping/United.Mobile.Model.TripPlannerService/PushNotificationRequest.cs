using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;


namespace United.Mobile.Model.TripPlannerService
{
    [DataContract]
    public class PushNotificationRequest
    {
        [DataMember]
        public Application Application { get; set; }
        [DataMember]
        public string TransactionId { get; set; }
        [DataMember]
        public NotificationEnvelope Data { get; set; }
        [DataMember]
        public string LanguageCode { get; set; }
        [DataMember]
        public string DeviceId { get; set; }
        [DataMember]
        public string PushToken { get; set; }
    }
}
