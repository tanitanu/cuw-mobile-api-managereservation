using System;

namespace United.Mobile.Model.FligtStatus.Internal
{
    public class PushTokenComplication
    {
        public string DeviceID { get; set; }

        public string PushToken { get; set; }

        public bool ComplicationsEnabled { get; set; }

        public int ApplicationId { get; set; }

        public string AppVersion { get; set; }

        public DateTime InsertDateTime { get; set; }
    }
}