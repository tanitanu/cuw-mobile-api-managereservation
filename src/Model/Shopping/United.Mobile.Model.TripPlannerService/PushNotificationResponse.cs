using System;
using System.Collections.Generic;

namespace United.Mobile.Model.TripPlannerService
{
    [Serializable()]
    public class PushNotificationResponse
    {
        public bool Data { get; set; }
        public string DateTimeUtc { get; set; } = DateTime.UtcNow.ToString();
        public string MachineName { get; set; } = System.Environment.MachineName;
        public long Duration { get; set; }
        public IDictionary<string, string> Errors { get; set; }
        public string TransactionId { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public string TraceId { get; set; }

        public PushNotificationResponse()
        {
            Errors = new Dictionary<string, string>();
        }
    }
}
